Opcodes
-------
#### j - imm15
pc = u15(imm15)
#### draw s1, s2, s3
vram[s1,s2] = s3;
#### movh d,imm8
r[d] = (r[d] & 0x00FF) | (u8(imm8) << 8);
#### mov d,imm8
r[d] = u8(imm8)
### beq s1, s2, imm4
if (r[s1] == r[s2]) pc += s4(imm4) * 2;
### bgt s1, s2, imm4
if (s16(r[s1]) > s16(r[s2])) pc += s4(imm4) * 2;
### ba s1, s2, imm4
if (u16(r[s1]) > u16(r[s2])) pc += s4(imm4) * 2;
#### [add, sub, and, or, xor] d, s1
r[d] = r[d] op r[s1]
#### [shl, shr] d, imm4
r[d] = r[d] op u4(imm4)
#### sar d,imm4
r[d] = s16(r[d]) >> u4(imm4)
#### [not, neg] d, s
r[d] = r[d] op imm4
#### mov d,s
r[d] = r[s]
#### read_s8 d, s
r[d] = s8(mem8[s])
#### read_u8 d, s
r[d] = u8(mem8[s])
#### read_16 d, s
r[d] = mem16[s>>1]
#### write_8 d, s
mem8[s] = r[d]
#### write_16 d, s
mem16[s>>1] = r[d]
#### jr s
pc = r[s]
#### wait imm4
wait for interrupt (imm4 specifies interrupt type)

Encoding
--------
### group 0 (s0)
j

### group 1 (s1)
 x  |  0   |  1   |  2  |  3 
--- | ---  | ---  | --- | --- 
 0  | draw | movh | mov | beq 
 4  | bgt  |  ba  |  ?  | ?

### group 2 (s2)
x   |  0  |  1  |  2  | 3 
--- | --- | --- | --- | --- 
 0  | mov | add | sub | neg 
 4  | and | or  | xor | not  
 8  | shl | shr | saw | read_u8
 C  | read_s8 | read_16 | write_8 | write_16

### group 3 (s3)
jr, wait

### Encoding table

Group | bits                           |  START | END 
----- | ----                           |   ---  | ----
s0 | 0\<1\> : imm15                    |   0000 | 7FFF
s1 | 1\<1\> : s1\<3\> : imm12          |   8000 | DFFF
s2 | 1\<1\> : 6\<3\>  : s2\<4\> : imm8 |   E000 | EFFF
s3 | 1\<1\> : 7\<3\>  : 0\<8\>  : imm4 |   F000 | F00F
s4 | 1\<1\> : 7\<3\>  : 1\<8\>  : imm4 |   F010 | F010

