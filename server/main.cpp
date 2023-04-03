#include "core/client.h"
#include "core/game.h"
#include "core/socket.h"
#include <cstring>
#include <iostream>
#include <pthread.h>
#include <thread>
#include <vector>

using namespace std;

#define PORT_NUMBER 8080

int main() {
  try {
    Game game("../questions.txt", 8080);
    game.start();
  } catch (const char *e) {
    cerr << "Error: " << e << endl;
  }
  // // Create socket
  // if (!server_socket.create()) {
  //   cerr << "Failed to create socket" << endl;
  //   return 1;
  // }
  //
  // // Bind port
  // if (!server_socket.bind(PORT_NUMBER)) {
  //   cerr << "Failed to bind to port " << PORT_NUMBER << endl;
  //   return 1;
  // }
  //
  // // Start listen
  // if (!server_socket.listen()) {
  //   cerr << "Failed to listen on socket" << endl;
  //   return 1;
  // }
  //
  // cout << "Server started. Listening on port " << PORT_NUMBER << "..."
  //           << endl;
  //
  // while (true) {
  //   cout << "relisten" << endl;
  //
  //   Socket client_socket;
  //   if (!server_socket.accept(client_socket)) {
  //     cerr << "Failed to accept incoming client connection" <<
  //     endl; continue;
  //   }
  //
  //   Client *client = new Client(next_client_id++, move(client_socket));
  //
  //   pthread_t thread;
  //   rc = pthread_create(&thread, NULL, handle_client, (void *)client);
  //
  //   if (rc) {
  //     cout << "Error:unable to create thread," << rc << endl;
  //     exit(-1);
  //   }
  //
  //   client_threads.push_back(thread);
  // }

  pthread_exit(NULL);
  return 0;
}
