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
char *Message::generate_question(int len_answer, string desc) {
  int desc_length = desc.length();
  char *result = new char[desc_length + 9];
  result[0] = 0x03;
  memcpy(&result[1], &len_answer, 4);
  memcpy(&result[5], &desc_length, 4);
  memcpy(&(result[9]), desc.c_str(), desc_length);

  return result;
}
char *Message::generate_player_turn(int turn_id, string name) {
  const int len_name = name.length();
  char *result = new char[len_name + 9];
  result[0] = 0x04;
  memcpy(&result[1], &turn_id, 4);
  memcpy(&result[5], &len_name, 4);
  memcpy(&(result[9]), name.c_str(), len_name);

  return result;
}

char *Message::generate_answer_response(int turn_id, vector<Client *> clients) {
  int length = 1 + 4 + 4 + get_score_board_length(clients);

  Encoder ecd = Encoder(length, 0x05);

  ecd.add(&turn_id, sizeof(turn_id));
  ecd.add_score_board(clients);

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

int get_score_board_length(vector<Client *> clients) {
  int length = 0;
  for (auto client : clients) {
    length += 4 + client->get_name().length() + 4;
  }

  return length;
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
