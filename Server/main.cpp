#include "core/client.h"
#include "core/game.h"
#include "core/socket.h"

using namespace std;

#define PORT_NUMBER 8080

int main() {
  try {
    Game *game = new Game("./questions.txt", 8080);
    game->start();
    delete game;
  } catch (const char *e) {
    cerr << "Error: " << e << endl;
  }
  pthread_exit(NULL);
  return 0;
}
