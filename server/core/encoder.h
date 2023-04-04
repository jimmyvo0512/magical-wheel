#ifndef ENCODER_H
#define ENCODER_H

#include <iostream>

using namespace std;

class Encoder {
public:
  Encoder(int length) {
    buffer = new char[length];
    seek_pos = 0;
  }

  char *get_buffer();

  void add(const void *__restrict__ ptr, size_t size);

private:
  char *buffer;
  size_t seek_pos;

  int move_seek_pos(size_t size);
};

#endif
