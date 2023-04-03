#include "thread_handler.h"
#include "thread_processor.h"
#include <iomanip>

string readCharArray(char *inputCharArray) {
  int stringLength;
  memcpy(&stringLength, &inputCharArray[1],
         4); // read the length from the char array
  stringLength = stringLength;
  char *stringData = new char[stringLength + 1];
  memcpy(stringData, &inputCharArray[5], stringLength);
  stringData[stringLength] = '\0';

  string result = stringData;
  cout << stringLength << stringData << " ajsldfja" << endl;

  delete[] stringData; // free the memory allocated for the string
  return result;
}

void *handle_client(void *wrapped_processor) {
  // TODO:
  ThreadProcessor *processor = (ThreadProcessor *)wrapped_processor;
  Client *client = processor->get_client();
  cout << "Accepted new client connection with ID " << client->get_id() << endl;

  while (true) {
    char buffer[1024];
    int received_bytes = client->get_socket().receive(buffer, sizeof(buffer));

    if (received_bytes <= 0) {
      break;
    }

    // Print out buffer
    cout << "Buffer ";
    for (int i = 0; i < received_bytes; i++) {
      cout << setw(2) << setfill('0') << hex << (int)buffer[i] << " ";
    }
    cout << endl;

    buffer[received_bytes] = '\0';
    cout << "Received message from client " << client->get_id() << ": "
         << buffer << endl;

    // Process the message here...

    // Registration
    if (buffer[0] == 0x01) {
      // string name = "";
      // for (int i = 5; i < received_bytes; ++i) {
      //   name += buffer[i];
      // }
      string name = readCharArray(buffer);
      cout << "Receive registration event, name: " << name << endl;
    }

    if (!client->get_socket().send(buffer, received_bytes)) {
      cerr << "Failed to send message back to client " << client->get_id()
           << endl;
    }
  }

  // Todo: handle disconnected
  cout << "Client " << client->get_id() << " disconnected" << endl;
  return 0;
}

void *handle_socket_connection(void *wrapped_game) {
  Game *game = (Game *)wrapped_game;
  game->listen_to_connection();
}
