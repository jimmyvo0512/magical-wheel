#include "client.h"

Client::Client(int id, Socket socket)
    : m_id(id), m_socket(socket), m_join_time(std::chrono::system_clock::now()),
      m_points(0) {}

int Client::get_id() const { return m_id; }

Socket &Client::get_socket() { return m_socket; }

std::chrono::time_point<std::chrono::system_clock>
Client::get_join_time() const {
  return m_join_time;
}

int Client::get_points() const { return m_points; }

void Client::add_points(int points) { m_points += points; }
