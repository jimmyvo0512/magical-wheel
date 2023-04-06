#ifndef MESSAGE_H
#define MESSAGE_H

#include "client.h"
#include <iomanip>
#include <string>
#include <vector>

using namespace std;

class Message {
public:
  static Message &get_instance(); // Returns the singleton instance
  void send_message(const std::string &message); // Sends a message

  // Generate message
  pair<int, char *> generate_player_joined(string name);

  // Question includes:
  // - Question
  // - Next turn
  pair<int, char *> generate_question(int turn_id, int answer_length,
                                      string desc, string player_name);

  // Using Player turn to notice that:
  // - When game start, it also emit the guessed as a dummy
  // - After one player turn, it will mean:
  //  + This is turn of player have name "john"
  //  + The guessed chars is "p_th_n"
  pair<int, char *> generate_player_turn(int turn_id, string guessed,
                                         string name);

  // Answer response:
  // - True:
  //  + turn_id guessed score_board next_player
  pair<int, char *> generate_answer_response(int turn_id, string guessed,
                                             vector<Client *> clients,
                                             Client *next_player);
  pair<int, char *> generate_end_game(string result_keyword,
                                      vector<Client *> clients);

  // Read message
  string read_register(char *buffer);
  pair<char, string> read_answer(char *buffer);

private:
  Message();                // Private constructor to prevent instantiation
  static Message *instance; // Pointer to the singleton instance
};

#endif
