#ifndef CLIENT_H
#define CLIENT_H

#include "socket.h"
#include <chrono>

using namespace std;

class Client {
public:
  Client(int id, Socket socket);
  int get_id() const;
  Socket &get_socket();
  chrono::time_point<chrono::system_clock> get_join_time() const;
  int get_points() const;
  void add_points(int points);
  void sendEvent(const std::string &event);

private:
  int m_id;
  Socket m_socket;
  chrono::time_point<chrono::system_clock> m_join_time;
  int m_points;
};

#endif
