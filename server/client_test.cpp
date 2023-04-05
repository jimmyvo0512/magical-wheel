#include "core/socket.h"
#include <cstring>
#include <iomanip>
#include <iostream>

using namespace std;

char *generateCharArray(string inputString) {
  int stringLength = inputString.length();
  cout << "String length: " << stringLength << " " << inputString << endl;
  char *result = new char[stringLength + 5];
  result[0] = 0x01;
  memcpy(&result[1], &stringLength, 4);
  memcpy(&(result[5]), inputString.c_str(), stringLength);

  return result;
}

int main() {
  Socket socket;
  if (!socket.create()) {
    cerr << "Failed to create socket" << endl;
    return 1;
  }

  if (!socket.connect("127.0.0.1", 8080)) {
    cerr << "Failed to connect to server" << endl;
    return 1;
  }

  cout << "Connected to server" << endl;

  cout << "Input name: ";
  string name;
  getline(cin, name);
  char *registration_message = generateCharArray(name);
  cout << "Buffer ";
  for (int i = 0; i < name.length() + 5; i++) {
    cout << setw(2) << setfill('0') << hex << (int)registration_message[i]
         << " ";
  }
  cout << endl;

  if (!socket.send(registration_message, name.length() + 5)) {
    cerr << "Failed to send message to server" << endl;
    return 1;
  }

  while (true) {
    // cout << "> ";
    // string message;
    // getline(cin, message);
    //
    // if (message.empty()) {
    //   continue;
    // }
    //
    // if (!socket.send(message.c_str(), message.length())) {
    //   cerr << "Failed to send message to server" << endl;
    //   break;
    // }

    char buffer[1024];
    int received_bytes = socket.receive(buffer, sizeof(buffer));
    if (received_bytes <= 0) {
      cerr << "Failed to receive message from server" << endl;
      break;
    }

    buffer[received_bytes] = '\0';
    cout << "Received message from server: " << buffer << endl;
  }

  socket.close();
  return 0;
}
