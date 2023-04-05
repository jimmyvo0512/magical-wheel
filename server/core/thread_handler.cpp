#include "thread_handler.h"
#include "message.h"
#include "thread_processor.h"
#include <utility>

void *handle_client(void *wrapped_processor) {
  // TODO:
  ThreadProcessor *processor = (ThreadProcessor *)wrapped_processor;
  Game *game = processor->get_game();
  Client *client = processor->get_client();

  cout << "Accepted new client connection with ID " << client->get_id() << endl;

  while (true) {
    char buffer[1024];
    int received_bytes = client->get_socket().receive(buffer, sizeof(buffer));
    if (received_bytes <= 0)
      break;

    cout << endl << "--------------------------" << endl;
    buffer[received_bytes] = '\0';
    cout << "Received message from client " << client->get_id() << ": "
         << buffer << endl;

    // Print out buffer
    cout << "Buffer:\n";
    for (int i = 0; i < received_bytes; i++)
      cout << setw(2) << setfill('0') << hex << (int)buffer[i] << " ";
    cout << endl;

    // Process the message here...
    switch (buffer[0]) {
    case 0x01: { // Registration
      string name = Message::get_instance().read_register(buffer);
      cout << "Receive registration event, name: " << name << endl;
      game->client_register(client, name);
      break;
    }
    case 0x02: { // Answer question
      pair<char, string> pair = Message::get_instance().read_answer(buffer);
      cout << "Receive guess event, letter: " << pair.first
           << ", keyword: " << pair.second << endl;
      game->validate_guess(pair.first, pair.second);
      break;
    }
    default:
      break;
    }

    cout << "--------------------------" << endl;
  }

  // Todo: handle disconnected
  cout << "Client " << client->get_id() << " disconnected" << endl;

  return 0;
}

void *handle_socket_connection(void *wrapped_game) {
  Game *game = (Game *)wrapped_game;
  game->listen_to_connection();
  return 0;
}
