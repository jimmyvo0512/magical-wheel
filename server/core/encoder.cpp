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
