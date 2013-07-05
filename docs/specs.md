SoC specs
===========

vga
-----

* 320x240, 3 bits per pixel
* Output as 640x480x72hz/32.5 mhz pixel clock

cpu
-----

* Target 32.5mhz (vga clock)
* 16 bit registers
* 16 registers
* 16 bit address bus
* dedicated vga bus
* 4K rom x16 bits
* 4K ram x16 bits

for more details check the docs/cpu folder

Sound
-------

* Todo
* Will communicate via mmio

vga
-----
* Todo
* Will use a special bus (can't fit into the 16 bit address space)


Input
-------
* Todo
* via mmio


CPU BUS
---------

NAME | address space | used size | start  |  end
---  | ------------- | --------- | ------ | -----
RAM  |      16K      |     4K    | 0x0000 | 0x3FFF
ROM  |      16K      |     4K    | 0x4000 | 0x7FFF
MMIO |      32K      |     ??    | 0x8000 | 0xFFFF

VGA BUS
---------
* x, y, color
