#include "encoder.h"
#include <cstring>

char *Encoder::get_buffer() { return buffer; }

int Encoder::move_seek_pos(size_t size) {
  seek_pos += size;
  return seek_pos - size;
}

void Encoder::add(const void *__restrict__ ptr, size_t size) {
  memcpy(&buffer[move_seek_pos(size)], ptr, size);
}

void Encoder::addScoreBoard(vector<Client *> clients) {
  int client_cnt = clients.size();
  add(&client_cnt, sizeof(client_cnt));

  for (auto client : clients) {
    int name_len = client->get_name().length();
    add(&name_len, sizeof(name_len));

    add(client->get_name().c_str(), name_len);

    int point = client->get_points();
    add(&point, sizeof(point));
  }
}
