#include "client.h"

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
  cout << name << endl;
  if (m_name == "")
    m_name = name;
  else
    throw "Name is already set";
}

string Client::get_name() {
  if (this->is_register())
    throw "Name isn't set";
  else
    return m_name;
}

int Client::get_points() const { return m_points; }

void Client::add_points(int points) { m_points += points; }

void Client::sendEvent(const string &event) {
  string message = "EVENT " + event + "\n";
  int bytes_sent = m_socket.send(message.c_str(), message.length());
  if (bytes_sent == -1) {
    cout << "SEND ERR";
    // Handle error...
  }
}
