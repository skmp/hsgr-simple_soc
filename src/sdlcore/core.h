#pragma once

#define MEM_SIZE 8192
#define MEM_MASK (MEM_SIZE - 1)

#define VSIZE 256
#define HSIZE 256
#define SCREEN_PIXELS (VSIZE*HSIZE)

typedef uint16_t u16;
typedef int16_t s16;

extern u16 vga[SCREEN_PIXELS];