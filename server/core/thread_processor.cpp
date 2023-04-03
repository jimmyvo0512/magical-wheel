#include "thread_processor.h"

ThreadProcessor::ThreadProcessor(Game *game, Client *client)
    : game(game), client(client) {}

Client *ThreadProcessor::get_client() { return this->client; }
