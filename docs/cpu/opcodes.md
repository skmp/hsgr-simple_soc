Opcodes
-------
* j - imm15
* draw s1, s2, s3
* movh d,imm8
* mov d,imm8
* beq, bgt, ba: s1, s2, imm4
* add, sub, and, or, xor: d, s1
* shl, shr: d, imm4
* not, neg: d, s
* mov d,s
* readm d, s
* writem d, s
* jr s
* wait

Encoding
--------
group 0 (s0)
j

group 1 (s1)
draw, movh, mov, beq, bgt, ba, ?, ?

group 2 (s2)
add, sub, and, or,
xor shl, shr, not,
neg, mov, readm, writem,
?, ?, ?, ?

group 3 (s3)
jr

group 4 (s4)
wait

OPCODE SPACE                          START | END 
j  -> 0 | imm15                        0000 | 7FFF

s1 -> 1 | op3  | imm12                 8000 | DFFF

s2 -> 1 | 6<3> | op4   | imm8          E000 | EBFF

s3 -> 1 | 6<3> | 12<4> | 0<4> | imm4   EC00 | EC0F

s4 -> 1 | 6<3> | 12<4> | 1<4> | 0<4>   EC10 | EC10