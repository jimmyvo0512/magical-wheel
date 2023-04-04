#include "Message.h"
#include <iostream>

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

char *generate_answer_response() {
  char *result = new char[1];
  result[0] = 0x05;
  return result;
}
