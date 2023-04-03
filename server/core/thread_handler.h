#ifndef THREAD_HANDLER_H
#define THREAD_HANDLER_H

#include "client.h"
#include "game.h"

using namespace std;

void *handle_client(void *wrapped_processor);
void *handle_socket_connection(void *wrapped_game);

#endif // THREAD_HANDLER_H
