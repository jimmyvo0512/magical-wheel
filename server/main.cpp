#include "core/client.h"
#include "core/socket.h"
#include <cstring>
#include <iostream>
#include <pthread.h>
#include <thread>
#include <vector>

using namespace std;

void *handle_client(void *data) {
  Client *client = (Client *)data;
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

int main() {
  Socket server_socket;
  int port_number = 8080;
  vector<Client> client_sockets;
  vector<pthread_t> client_threads;
  int next_client_id = 1;
  int rc;

  // Create socket
  if (!server_socket.create()) {
    std::cerr << "Failed to create socket" << std::endl;
    return 1;
  }

  // Bind port
  if (!server_socket.bind(port_number)) {
    std::cerr << "Failed to bind to port " << port_number << std::endl;
    return 1;
  }

  // Start listen
  if (!server_socket.listen()) {
    std::cerr << "Failed to listen on socket" << std::endl;
    return 1;
  }

  std::cout << "Server started. Listening on port " << port_number << "..."
            << std::endl;

  while (true) {
    std::cout << "relisten" << std::endl;

    Socket client_socket;
    if (!server_socket.accept(client_socket)) {
      std::cerr << "Failed to accept incoming client connection" << std::endl;
      continue;
    }

    Client *client = new Client(next_client_id++, std::move(client_socket));

    pthread_t thread;
    rc = pthread_create(&thread, NULL, handle_client, (void *)client);

    if (rc) {
      cout << "Error:unable to create thread," << rc << endl;
      exit(-1);
    }

    client_threads.push_back(thread);
  }

  pthread_exit(NULL);
  server_socket.close();
  return 0;
}
