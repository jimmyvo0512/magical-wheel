#include "game.h"
#include "thread_handler.h"
#include "thread_processor.h"

Game::Game(string filename, int port)
    : m_random_engine(chrono::system_clock::now().time_since_epoch().count()) {
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
      break;

    this->broadcast("Game started");
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
    Client *client = new Client(1092, client_socket);
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
  cout << "hea" << client->get_id() << endl;
  try {
    // m_mutex.lock();
    client->client_register(name);
    cout << "healsda" << endl;
    if (this->is_started) {
      this->m_waiting_pool.push_back(client);
    } else {
      this->m_playing_pool.push_back(client);
    }

    if (this->m_playing_pool.size() == 3) {
      this->is_started = true;
    }
    // m_mutex.unlock();

    this->broadcast(name + " joined");
  } catch (const char *e) {
    cout << "Error: Client register failed. Err: " << e << endl;
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
    while (getline(file, line1) && getline(file, line2)) {
      m_questions_and_answers.push_back(pair<string, string>(line1, line2));
    }
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
  broadcast(message);
}

void Game::broadcast(const string &event) {
  // lock_guard<mutex> lock(m_clientMutex);
  for (auto &client : m_clients) {
    client.second->sendEvent(event);
  }
}
