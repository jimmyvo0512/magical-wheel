#include "message.h"
#include "encoder.h"
#include <cstring>
#include <iostream>
#include <utility>

// Initialize the singleton instance pointer to null
Message *Message::instance = nullptr;

// Private constructor to prevent instantiation
Message::Message() {}

// Returns the singleton instance
Message &Message::get_instance() {
  if (instance == nullptr)
    instance = new Message();

  return *instance;
}

char *Message::generate_player_joined(string name) {
  int len_name = name.length();
  char *result = new char[len_name + 5];
  result[0] = 0x02;
  memcpy(&result[1], &len_name, 4);
  memcpy(&(result[5]), name.c_str(), len_name);

  return result;
}

char *Message::generate_question(int turn_id, int len_answer, string desc,
                                 string player_name) {
  int length = 1 + 4 + 4 + 4 + desc.length() + 4 + player_name.length();
  Encoder ecd = Encoder(length, 0x03);

  ecd.add(&turn_id, sizeof(turn_id));
  ecd.add(&len_answer, sizeof(len_answer));
  ecd.addStr(desc);
  ecd.addStr(player_name);

  return ecd.get_buffer();
}

char *Message::generate_player_turn(int turn_id, string guessed, string name) {
  int length = 1 + 4 + 4 + guessed.length() + 4 + name.length();
  Encoder ecd = Encoder(length, 0x03);

  ecd.add(&turn_id, sizeof(turn_id));
  ecd.addStr(guessed);
  ecd.addStr(name);

  return ecd.get_buffer();
}

int get_score_board_length(vector<Client *> clients) {
  int length = 4;
  for (auto client : clients) {
    length += 4 + client->get_name().length() + 4;
  }

  return length;
}

char *Message::generate_answer_response(int turn_id, string guessed,
                                        vector<Client *> clients,
                                        Client *next_player) {
  // <0x05><turn_id><guess_char><score_board><next_player_name_length><next_player_name>
  string next_client_name = next_player->get_name();
  int name_length = next_client_name.length();
  int length = 1 + 4 + guessed.length() + get_score_board_length(clients) + 4 +
               name_length;

  Encoder ecd = Encoder(length, 0x05);

  ecd.add(&turn_id, sizeof(turn_id));
  ecd.addStr(guessed);
  ecd.add_score_board(clients);
  ecd.add(&name_length, sizeof(name_length));
  ecd.add(&next_client_name, sizeof(next_client_name));

  return ecd.get_buffer();
}

char *Message ::generate_end_game(string result_keyword,
                                  vector<Client *> clients) {
  int length =
      1 + 4 + result_keyword.length() + 4 + get_score_board_length(clients);

  Encoder ecd = Encoder(length, 0x06);

  ecd.addStr(result_keyword);
  ecd.add_score_board(clients);

  return ecd.get_buffer();
}

string Message::read_register(char *buffer) {
  int len_name;
  memcpy(&len_name, &buffer[1], 4);
  char *name = new char[len_name + 1];
  memcpy(name, &buffer[5], len_name);
  name[len_name] = '\0';

  string result = name;
  delete[] name;

  return result;
}

pair<char, string> Message::read_answer(char *buffer) {
  int len_keyword;
  char letter;
  memcpy(&letter, &buffer[1], 1);      // read the length from the char array
  memcpy(&len_keyword, &buffer[2], 4); // read the length from the char array
  char *name = new char[len_keyword + 1];
  memcpy(name, &buffer[5], len_keyword);
  name[len_keyword] = '\0';

  string keyword = name;
  delete[] name; // free the memory allocated for the string

  return make_pair(letter, keyword);
}
