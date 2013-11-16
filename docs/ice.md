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

8:0 -> read addr[0]         -> 0:addr[0]
8:1 -> read addr[1]         -> 1:addr[1]
8:2 -> read addr[2]         -> 2:addr[2]
8:3 -> read addr[3]         -> 3:addr[3]

8:4 -> read data[0]         -> 4:data[0]
8:5 -> read data[1]         -> 5:data[1]
8:6 -> read data[2]         -> 6:data[2]
8:7 -> read data[3]         -> 7:data[3]

8:8 -> read mem to data     -> echo
8:9 -> write data to mem    -> echo

9:n  -> reg[n] = data       -> echo
10:n -> data = reg[n]       -> echo

11:0 -> reset = 0           -> echo
11:1 -> reset = 1           -> echo

11:2 -> halt                -> echo
11:3 -> resume              -> echo
11:4 -> step                -> echo
11:5 -> pc = data           -> echo
11:6 -> data = pc           -> echo