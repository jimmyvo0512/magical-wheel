#include "client.h"
#include <cstring>
#include <iomanip>
#include <regex>

Client::Client(int id, Socket socket)
    : m_id(id), m_socket(socket), m_join_time(chrono::system_clock::now()),
      m_points(0) {
  this->m_name = "";
}

int Client::get_id() const { return m_id; }

Socket &Client::get_socket() { return m_socket; }

chrono::time_point<chrono::system_clock> Client::get_join_time() const {
  return m_join_time;
}

bool Client::is_register() { return this->m_name == ""; }

void Client::client_register(string name) {
  if (m_name == "") {
    regex pattern("[a-zA-Z0-9_]{1,10}");
    if (regex_match(name, pattern)) {
      m_name = name;
      char *message = new char[2];
      message[0] = 0x01;
      message[1] = 0x01;

      this->sendEvent(message, 2);
      delete[] message;
    } else
      throw INVALID_NAME;

  } else
    throw NAME_ALREADY_SET;
}

string Client::get_name() {
  if (this->is_register())
    throw "Name isn't set";
  else
    return m_name;
}

int Client::get_points() const { return m_points; }

void Client::add_points(int points) { m_points += points; }

void Client::sendEvent(char *event, int length) {
  cout << "Send buffer:" << length << endl;
  for (int i = 0; i < length; i++)
    cout << setw(2) << setfill('0') << hex << (int)event[i] << " ";
  cout << endl;

  int bytes_sent = m_socket.send(event, length);
  if (bytes_sent == -1) {
    cout << "SEND ERR";
    // Handle error...
  }
}
