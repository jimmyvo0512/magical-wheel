#include "game.h"
#include "Message.h"
#include "client.h"
#include "message.h"
#include "thread_handler.h"
#include "thread_processor.h"
#include <cstdlib>

bool is_contain(string s, char c) { return s.find(c) != std::string::npos; }

Game::Game(string filename, int port)
    : m_random_engine(chrono::system_clock::now().time_since_epoch().count()) {
  this->is_started = false;
  turn = 0;
  load_questions_and_answers(filename);
  if (start_socket() == 1)
    throw "Failed to start socket";
}

void Game::start() {
  pthread_t listen_to_connection_thread;
  int rc = pthread_create(&listen_to_connection_thread, NULL,
                          handle_socket_connection, (void *)this);
  while (true) {
    if (!this->is_started)
      continue;

    if (turn == 0) {
      sleep(5);
      cout << "Broadcast Start game" << endl;
      string first_player_name = this->m_playing_pool[0]->get_name();
      this->broadcast_playing_pool(
          Message::get_instance().generate_player_turn(1, first_player_name));
      turn += 1;
    }
  }
}

void Game::listen_to_connection() {
  while (true) {
    cout << "Relisten" << endl;

    Socket client_socket;
    if (!m_server_socket.accept(client_socket)) {
      cerr << "Failed to accept incoming client connection" << endl;
      continue;
    }

    // Add the player to the game
    Client *client = new Client(rand(), client_socket);
    int player_id = client->get_id();
    m_clients.insert(make_pair(player_id, client));

    cout << "Player " << player_id << " connected" << endl;

    pthread_t thread;
    ThreadProcessor processor = ThreadProcessor(this, client);
    int rc = pthread_create(&thread, NULL, handle_client, (void *)&processor);
    m_threads.insert(make_pair(player_id, thread));
  }
}

void Game::client_register(Client *client, string name) {
  try {
    // TODO: Check is this name already exists or not
    client->client_register(name);
    if (this->is_started) {
      this->m_waiting_pool.push_back(client);
      cout << " - Add client " << client->get_name() << " to waiting pool"
           << endl;
    } else {
      this->m_playing_pool.push_back(client);
      cout << " - Add client " << client->get_name() << " to playing pool"
           << endl;
    }
    cout << " ==> Playing pool length: " << this->m_playing_pool.size() << endl;

    this->broadcast_playing_pool(
        Message::get_instance().generate_player_joined(name));

    if (this->m_playing_pool.size() == 2) {
      this->is_started = true;
      this->turn = 0;
    }
  } catch (NameError e) {
    cout << "Error: Client register failed. Err: " << e << endl;
    switch (e) {
    case INVALID_NAME: {
      char message[2] = {0x01, 0x01};
      client->sendEvent(message);
      break;
    }
    default:
      break;
    }
  }
}
void Game::validate_guess(char letter, string keyword) {

  if (this->turn > 2) {
    // Check keyword
    if (keyword == this->keyword) {
      // Emit end game
      return;
    }
  }
  // Check letter
  if (is_contain(this->keyword, letter) && !is_contain(this->guested, letter)) {
    // Emit Greate guess

  } else {
    // Emit Failed guess
  }
}

int Game::start_socket() {
  // Create socket
  if (!m_server_socket.create()) {
    cerr << "Failed to create socket" << endl;
    return 1;
  }

  // Bind port
  if (!m_server_socket.bind(PORT_NUMBER)) {
    cerr << "Failed to bind to port " << PORT_NUMBER << endl;
    return 1;
  }

  // Start listen
  if (!m_server_socket.listen(2)) {
    cerr << "Failed to listen on socket" << endl;
    return 1;
  }
  return 0;
}

void Game::load_questions_and_answers(string filename) {
  ifstream file(filename);
  if (file.is_open()) {
    string line1, line2;
    getline(file, line1);
    int numPair = stoi(line1);
    for (int i = 0; i < numPair; ++i)
      if (getline(file, line1) && getline(file, line2))
        m_questions_and_answers.push_back(pair<string, string>(line2, line1));
    file.close();
  } else {
    cerr << "Failed to open file " << filename << endl;
  }
}

void Game::send_question_to_player(Client &client) {
  // Pick a random question
  uniform_int_distribution<int> dist(0, m_questions_and_answers.size() - 1);
  int question_index = dist(m_random_engine);
  string question = m_questions_and_answers[question_index].first;
  string answer = m_questions_and_answers[question_index].second;

  // Send the question to the player
  string message = "Question: " + question + "\n";
  message += "Answer: " + string(answer.length(), '_') + "\n";
  client.get_socket().send(message.c_str(), message.length());
}

void Game::send_game_over_message() {
  string message = "Game over\n";
  broadcast_playing_pool(message);
}

void Game::broadcast_playing_pool(const string &event) {
  for (auto &client : m_playing_pool) {
    cout << "HERE PRINT: " << client->get_name() << " " << event;
    client->sendEvent(event);
  }
}
