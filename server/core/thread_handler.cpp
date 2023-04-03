#include "thread_handler.h"
#include "thread_processor.h"

void *handle_client(void *wrapped_processor) {
  // TODO:
  ThreadProcessor *processor = (ThreadProcessor *)wrapped_processor;
  Client *client = processor->get_client();
  std::cout << "Accepted new client connection with ID " << client->get_id()
            << std::endl;

  char buffer[1024];
  while (true) {
    int received_bytes = client->get_socket().receive(buffer, sizeof(buffer));
    if (received_bytes <= 0) {
      break;
    }

    buffer[received_bytes] = '\0';
    std::cout << "Received message from client " << client->get_id() << ": "
              << buffer << std::endl;

    // Process the message here...

    if (!client->get_socket().send(buffer, received_bytes)) {
      std::cerr << "Failed to send message back to client " << client->get_id()
                << std::endl;
    }
  }

  std::cout << "Client " << client->get_id() << " disconnected" << std::endl;
  return 0;
}

void *handle_socket_connection(void *wrapped_game) {
  Game *game = (Game *)wrapped_game;
  game->listen_to_connection();
}
