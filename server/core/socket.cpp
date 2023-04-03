#include "socket.h"

Socket::Socket() : m_socket(0) {}

bool Socket::create() {
  m_socket = socket(AF_INET, SOCK_STREAM, 0);
  return m_socket >= 0;
}

bool Socket::bind(int port) {
  m_address.sin_family = AF_INET;
  m_address.sin_addr.s_addr = INADDR_ANY;
  m_address.sin_port = htons(port);
  return ::bind(m_socket, (struct sockaddr *)&m_address, sizeof(m_address)) >=
         0;
}

bool Socket::listen(int backlog) { return ::listen(m_socket, backlog) >= 0; }

bool Socket::accept(Socket &client_socket) {
  struct sockaddr_in client_address;
  socklen_t client_address_len = sizeof(client_address);
  int client_socket_fd = ::accept(m_socket, (struct sockaddr *)&client_address,
                                  &client_address_len);
  if (client_socket_fd >= 0) {
    client_socket.m_socket = client_socket_fd;
    return true;
  }
  return false;
}

bool Socket::connect(const std::string &address, int port) {
  m_address.sin_family = AF_INET;
  m_address.sin_port = htons(port);
  if (inet_pton(AF_INET, address.c_str(), &m_address.sin_addr) <= 0) {
    return false;
  }
  return ::connect(m_socket, (struct sockaddr *)&m_address,
                   sizeof(m_address)) >= 0;
}

bool Socket::send(const char *buffer, int size) {
  return ::send(m_socket, buffer, size, 0) >= 0;
}

int Socket::receive(char *buffer, int size) {
  return ::recv(m_socket, buffer, size, 0);
}

bool Socket::close() {
  if (m_socket >= 0) {
    ::close(m_socket);
    m_socket = -1;
    return true;
  }
  return false;
}
