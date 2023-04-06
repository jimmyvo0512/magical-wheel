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

void Encoder::addStr(string str) {
  int str_len = str.length();
  add(&str_len, sizeof(str_len));

  add(str.c_str(), str_len);
}

void Encoder::add_score_board(vector<Client *> clients) {
  int client_cnt = clients.size();
  add(&client_cnt, sizeof(client_cnt));

  for (auto client : clients) {
    addStr(client->get_name());

    int point = client->get_points();
    add(&point, sizeof(point));
  }
}
