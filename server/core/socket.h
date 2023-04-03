#ifndef SOCKET_H
#define SOCKET_H

#include <arpa/inet.h>
#include <cstring>
#include <iostream>
#include <string>
#include <sys/socket.h>
#include <unistd.h>

class Socket {
public:
  Socket();
  bool create();
  bool bind(int port);
  bool listen(int backlog = 5);
  bool accept(Socket &client_socket);
  bool connect(const std::string &address, int port);
  bool send(const char *buffer, int size);
  int receive(char *buffer, int size);
  bool close();

private:
  int m_socket;
  struct sockaddr_in m_address;
};

#endif
