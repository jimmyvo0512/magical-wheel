#ifndef GAME_H
#define GAME_H

#include "client.h"
#include "socket.h"
#include <chrono>
#include <cstdlib>
#include <fstream>
#include <iostream>
#include <map>
#include <mutex>
#include <pthread.h>
#include <random>
#include <stdlib.h>
#include <string>
#include <utility>
#include <vector>

using namespace std;

#define PORT_NUMBER 8080
#define MAX_PLAYER 10
#define MIN_PLAYER 2

class Game {
public:
  Game(string filename, int port);
  void start();
  void listen_to_connection();
  void client_register(Client *client, string name);

private:
  mutex m_mutex;
  Socket m_server_socket;
  map<int, Client *> m_clients;

  vector<Client *> m_playing_pool;
  vector<Client *> m_waiting_pool;

  map<int, pthread_t> m_threads;

  mutex m_clientMutex;

  // Game State
  bool is_started;
  int turn;

  vector<pair<string, string> > m_questions_and_answers;

  default_random_engine m_random_engine;

  void load_questions_and_answers(string filename);
  int start_socket();
  // void *handle_client(void* data);
  void send_question_to_player(Client &client);
  void send_game_over_message();

  void broadcast_playing_pool(const string &event);
};

#endif
