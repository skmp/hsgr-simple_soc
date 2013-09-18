#include <stdio.h>
#include <stdlib.h>
#include <cstdint>

typedef uint16_t u16;
typedef int16_t s16;

#define MEM_SIZE 8192
#define MEM_MASK (MEM_SIZE - 1)

union opcode_t {
	u16 data;
	struct {
		u16 _padx:8;
		u16 s2:4;
		u16 s1:3;
		u16 is_not_jump: 1;
	};

	struct {
		u16 _padxy:4;
		u16 s3:8;
		u16 _padxx:3;
		u16 _padxxk: 1;
	};

	struct {
		u16 imm15:15;
		u16 _pad0:1;
	};

	struct {
		u16 imm8:8;
		u16 _pad1:8;
	};

	struct {
		u16 imm4:4;
		u16 _pad1:12;
	};

	struct {
		u16 p_r3:4;
		u16 p_r2:4;
		u16 p_r1:4;
		u16 _pad2:4;
	};
};

enum s1_codes {
	s1_draw = 0,
	s1_movh,
	s1_movl,
	
	s1_beq,
	s1_bgt,
	s1_ba,
	s1_ext_s2,
	s1_ext_s3,
};

enum s2_codes {
	s2_mov = 0,
	s2_add,
	s2_sub,
	s2_neg,

	s2_and,
	s2_or,
	s2_xor,
	s2_not,

	s2_shl,
	s2_shr,
	s2_sar,
	s2_read16,

	s2_write16,
	s2_addi,
	s2_subi,
	s2_undefined,
};

enum s3_codes {
	s3_jr = 0,
	s3_wait,
};


u16 mem[MEM_SIZE];
u16 vga[320*240];

u16 readm(u16 addr) {
	return mem[addr & MEM_MASK];
}

void writem(u16 addr, u16 data) {
	mem[addr & MEM_MASK] = data;
}

struct {
	u16 regs[16];
	u16 pc;
} state;

u16 decode_u15(u16 v) { return v; }
u16 decode_s4(u16 v) { return v&8?v-16:v; }
u16 decode_u4(u16 v) { return v; }

#define r state.regs
#define pc state.pc

bool runop()
{
	opcode_t op;
	
	op.data = readm(pc);
	
	pc++;

	if (!op.is_not_jump)
	{
		pc = op.imm15;
	}
	else
	{
		switch(op.s1) {
		case s1_draw:	vga[r[op.p_r2]*320 + r[op.p_r1]] = r[op.p_r3]; break;

		case s1_movh:	r[op.p_r1] = (r[op.p_r1] & 0xFF) | (op.imm8<<8);  break;

		case s1_movl:	r[op.p_r1] = op.imm8;  break;

		case s1_beq:	
			if (r[op.p_r1] == r[op.p_r2]) 
				pc+=decode_s4(op.imm4)-1; 
			break;

		case s1_bgt:
			if ((s16)r[op.p_r1] > (s16)r[op.p_r2]) 
				pc+=decode_s4(op.imm4)-1; 
			break;

		case s1_ba:
			if ((u16)r[op.p_r1] > (u16)r[op.p_r2]) 
				pc+=decode_s4(op.imm4)-1; 
			break;
			
		case s1_ext_s2:
			switch(op.s2)
			{
				case s2_mov: r[op.p_r2] = r[op.p_r3]; break;
				case s2_add: r[op.p_r2] += r[op.p_r3]; break;
				case s2_sub: r[op.p_r2] -= r[op.p_r3]; break;
					
				case s2_and: r[op.p_r2] &= r[op.p_r3]; break;
				case s2_or: r[op.p_r2] |= r[op.p_r3]; break;
				case s2_xor: r[op.p_r2] ^= r[op.p_r3]; break;

				case s2_neg: r[op.p_r2] = -r[op.p_r3]; break;
				case s2_not: r[op.p_r2] = ~r[op.p_r3]; break;

				case s2_shl: r[op.p_r2] <<= op.imm4;  break;
				case s2_shr: r[op.p_r2] >>= op.imm4;  break;
				case s2_sar: (s16&)r[op.p_r2] >>= op.imm4;  break;

				case s2_read16: r[op.p_r2] = readm(r[op.p_r3]); break;
				case s2_write16: writem(r[op.p_r3], r[op.p_r2]);break;

				case s2_addi: r[op.p_r2]+=op.imm4; break;
				case s2_subi: r[op.p_r2]-=op.imm4; break;
			}
		break;

		case s1_ext_s3:
			switch(op.s3)
			{
			case s3_jr:	pc = r[op.p_r3]; break;

			case s3_wait:
				return true;
			}
		break;


		}
	}

	return false;
}

void runframe() {
	for (int i=0;i<10000;i++)
		if (runop())
			break;
}

void loadfile(FILE* f) {
	if (f) {
		fread(mem,1,sizeof(mem),f);
		fclose(f);
	}
}