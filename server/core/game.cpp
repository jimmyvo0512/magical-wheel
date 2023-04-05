#include "game.h"
#include "client.h"
#include "message.h"
#include "thread_handler.h"
#include "thread_processor.h"
#include <cstdlib>
#include <utility>

bool is_contain(string s, char c) { return s.find(c) != std::string::npos; }

Game::Game(string filename, int port)
    : m_random_engine(chrono::system_clock::now().time_since_epoch().count()) {
  m_is_started = false;
  m_turn = 0;
  load_questions_and_answers(filename);
  if (start_socket() == 1)
    throw "Failed to start socket";
}

void Game::start() {
  // Listening to client connection
  pthread_t listen_to_connection_thread;
  int rc = pthread_create(&listen_to_connection_thread, NULL,
                          handle_socket_connection, (void *)this);

  // Maintain game logic
  while (true) {
    if (!m_is_started)
      continue;

    if (m_turn == 0) {
      sleep(1);
      cout << "Broadcast Start game" << endl;
      string first_player_name = this->m_playing_pool[0]->get_name();

      pair<int, string> question = generate_question();
      this->broadcast_playing_pool(Message::get_instance().generate_question(
          1, question.first, question.second, first_player_name));

      m_turn += 1;
      m_num_guess_in_turn = 0;
    }

    if (m_num_guess_in_turn >= m_playing_pool.size()) {
      // Moving to next turn stem from no right guesses
      m_turn += 1;
      m_num_guess_in_turn = 0;
    }
  }
}

// Listen to client connection
//  - If game do not start, client will be put into playing pool
//  - If game started, client would be put into waiting pool
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

// Handle register action
void Game::client_register(Client *client, string name) {
  try {
    // TODO: Check is this name already exists or not

    client->client_register(name);
    if (m_is_started) {
      this->m_waiting_pool.push_back(client);
      cout << " - Add client " << client->get_name() << " to waiting pool"
           << endl;
    } else {
      this->m_playing_pool.push_back(client);
      cout << " - Add client " << client->get_name() << " to playing pool"
           << endl;
    }
    cout << " ==> Playing pool length: " << this->m_playing_pool.size() << endl;

    // char *message =
    //     Message::get_instance().generate_player_joined(client->get_name());
    // cout << "Buffer ";
    // for (int i = 0; i < 100; i++) {
    //   cout << setw(2) << setfill('0') << hex << (int)message[i] << " ";
    // }
    // cout << endl;

    this->broadcast_playing_pool(
        Message::get_instance().generate_player_joined(client->get_name()));

    if (this->m_playing_pool.size() == 2) {
      m_is_started = true;
      m_turn = 0;
    }
  } catch (NameError e) {
    cout << "Error: Client register failed. Err: " << e << endl;
    switch (e) {
    case INVALID_NAME: {
      char message[2] = {0x01, 0x02};
      client->sendEvent(message, 2);
      break;
    }
    default:
      break;
    }
  }
}
void Game::validate_guess(char letter, string keyword) {
  if (this->m_turn > 2) {
    // Check keyword
    if (keyword == this->m_keyword) {
      // Emit end game
      this->get_cur_player()->add_points(5);
      this->broadcast_playing_pool(Message::get_instance().generate_end_game(
          keyword, this->m_playing_pool));
      return;
    }
  }
  // Check letter
  if (is_contain(this->m_keyword, letter) &&
      !is_contain(this->m_guessed, letter)) { // True
    this->get_cur_player()->add_points(1);
    // Emit Greate guess
    for (int i = 0; i < m_keyword.length(); i++) {
      if (m_keyword[i] == letter) {
        m_guessed[i] = letter;
      }
    }
    m_cur_player_index += 1;
    this->broadcast_playing_pool(
        Message::get_instance().generate_answer_response(
            m_turn, m_guessed, m_playing_pool, this->get_cur_player()));
    m_turn += 1;
    m_cur_player_index += 1;
    m_num_guess_in_turn = 0;
  } else {
    // Emit Failed guess
    m_cur_player_index += 1;
    this->broadcast_playing_pool(Message::get_instance().generate_player_turn(
        m_turn, m_guessed, this->get_cur_player()->get_name()));
    m_num_guess_in_turn += 1;
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

pair<int, string> Game::generate_question() {
  int num_question = this->m_questions_and_answers.size();
  int random_id = rand() % num_question;
  pair<string, string> question_answer;

  m_keyword = question_answer.second;
  m_guessed = string(m_keyword.length(), '_');

  return make_pair(m_keyword.length(), question_answer.first);
}

// void Game::send_game_over_message() {
//   string message = "Game over\n";
//   broadcast_playing_pool(message);
// }

void Game::broadcast_playing_pool(pair<int, char *> event) {
  for (auto &client : m_playing_pool) {
    cout << "Sent to " << client->get_name() << endl;
    client->sendEvent(event.second, event.first);
  }
}

Client *Game::get_cur_player() {
  if (m_cur_player_index >= m_playing_pool.size()) {
    m_cur_player_index %= m_playing_pool.size();
  }
  return m_playing_pool[m_cur_player_index];
}
