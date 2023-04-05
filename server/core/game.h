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
  void validate_guess(char letter, string keyword);

private:
  mutex m_mutex;
  Socket m_server_socket;
  map<int, Client *> m_clients;

  vector<Client *> m_playing_pool;
  vector<Client *> m_waiting_pool;

  map<int, pthread_t> m_threads;

  mutex m_clientMutex;

  // Game State
  int num_player;
  bool m_is_started;
  int m_turn;
  int m_num_guess_in_turn;
  int m_cur_player_index;
  string m_keyword;
  string m_guessed;

  vector<pair<string, string> > m_questions_and_answers;

  default_random_engine m_random_engine;

  void load_questions_and_answers(string filename);
  int start_socket();
  // void *handle_client(void* data);
  void send_question_to_player(Client &client);
  pair<int, string> generate_question();

  void send_game_over_message();

  void broadcast_playing_pool(pair<int, char *> event);

  Client *get_cur_player();
};

#endif
