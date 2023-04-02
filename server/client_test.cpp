#include "core/socket.h"
#include <cstring>
#include <iostream>

int main() {
  Socket socket;
  if (!socket.create()) {
    std::cerr << "Failed to create socket" << std::endl;
    return 1;
  }

  if (!socket.connect("127.0.0.1", 8080)) {
    std::cerr << "Failed to connect to server" << std::endl;
    return 1;
  }

  std::cout << "Connected to server" << std::endl;

  while (true) {
    std::cout << "> ";
    std::string message;
    std::getline(std::cin, message);

    if (message.empty()) {
      continue;
    }

    if (!socket.send(message.c_str(), message.length())) {
      std::cerr << "Failed to send message to server" << std::endl;
      break;
    }

    char buffer[1024];
    int received_bytes = socket.receive(buffer, sizeof(buffer));
    if (received_bytes <= 0) {
      std::cerr << "Failed to receive message from server" << std::endl;
      break;
    }

    buffer[received_bytes] = '\0';
    std::cout << "Received message from server: " << buffer << std::endl;
  }

  socket.close();
  return 0;
}
