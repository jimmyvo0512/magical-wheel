#ifndef CLIENT_H
#define CLIENT_H

#include "socket.h"
#include <chrono>

using namespace std;

enum NameError { NAME_ALREADY_SET, INVALID_NAME };

class Client {
public:
  Client(int id, Socket socket);
  int get_id() const;
  Socket &get_socket();
  chrono::time_point<chrono::system_clock> get_join_time() const;

  bool is_register();

  void client_register(string name);
  string get_name();

  int get_points() const;
  void add_points(int points);

  void sendEvent(const std::string &event);

private:
  int m_id;
  Socket m_socket;
  chrono::time_point<chrono::system_clock> m_join_time;

  string m_name;
  int m_points;
};

#endif
