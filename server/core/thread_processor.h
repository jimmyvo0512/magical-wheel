#ifndef THREAD_PROCESSOR_H
#define THREAD_PROCESSOR_H

#include "client.h"
#include "game.h"

class ThreadProcessor {
public:
  ThreadProcessor(Game *game, Client *client);
  Client *get_client();

private:
  Game *game;
  Client *client;
};

#endif // THREAD_PROCESSOR_H
