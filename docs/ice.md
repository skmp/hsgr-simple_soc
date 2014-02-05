Simple SoC ICE protocol
=======================

Registers
---------

0:addr[0]                   -> echo
1:addr[1]                   -> echo
2:addr[2]                   -> echo
3:addr[3]                   -> echo

4:data[0]                   -> echo
5:data[1]                   -> echo
6:data[2]                   -> echo
7:data[3]                   -> echo

commands
--------

0x80 -> read addr[0]         -> 0:addr[0]
0x81 -> read addr[1]         -> 1:addr[1]
0x82 -> read addr[2]         -> 2:addr[2]
0x83 -> read addr[3]         -> 3:addr[3]

0x84 -> read data[0]         -> 4:data[0]
0x85 -> read data[1]         -> 5:data[1]
0x86 -> read data[2]         -> 6:data[2]
0x87 -> read data[3]         -> 7:data[3]

0x90 -> read ram to data     -> echo
0x91 -> write data to ram    -> echo

0x92 -> read vram to data     -> echo
0x93 -> write data to vram    -> echo

0x94  -> reg[addr] = data       -> echo
0x95 -> data = reg[addr]       -> echo

/*
11:0 -> reset = 0           -> echo
11:1 -> reset = 1           -> echo

11:2 -> halt                -> echo
11:3 -> resume              -> echo
11:4 -> step                -> echo
11:5 -> pc = data           -> echo
11:6 -> data = pc           -> echo
*/
