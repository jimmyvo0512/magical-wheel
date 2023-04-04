#ifndef MESSAGE_H
#define MESSAGE_H

#include <iomanip>
#include <string>

using namespace std;

class Message {
public:
  static Message &get_instance(); // Returns the singleton instance
  void send_message(const std::string &message); // Sends a message

  // Generate message
  char *generate_player_joined(string name);
  char *generate_question(int answer_length, string desc);
  char *generate_player_turn(int turn_id, string name);

  // Read message
  string read_register(char *buffer);
  pair<char, string> read_answer(char *buffer);

private:
  Message();                // Private constructor to prevent instantiation
  static Message *instance; // Pointer to the singleton instance
};

#endif
