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
  char *generate_player_joined(string name);
  char *generate_question(int answer_length, string desc);
  char *generate_player_turn(int turn_id, string name);
  char *generate_answer_response(int turn_id, vector<Client *> clients);
  char *generate_end_game(string result_keyword, vector<Client *> clients);

  // Read message
  string read_register(char *buffer);
  pair<char, string> read_answer(char *buffer);

private:
  Message();                // Private constructor to prevent instantiation
  static Message *instance; // Pointer to the singleton instance
};

#endif
