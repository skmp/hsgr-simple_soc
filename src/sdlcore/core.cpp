#include <stdio.h>
#include <stdlib.h>
#include <cstdint>

typedef uint16_t u16;

#define MEM_SIZE 8192
#define MEM_MASK (MEM_SIZE - 1)

union opcode_t {
	u16 data;
	struct {
		u16 is_not_jump: 1;
		u16 s1:3;
		u16 sx:4;
	};

	struct {
		u16 _pad0:1;
		u16 imm15:15;
	};

	struct {
		u16 _pad1:8;
		u16 imm8:8;
	};

	struct {
		u16 _pad1:12;
		u16 imm4:4;
	};

	struct {
		u16 _pad2:4;
		u16 p_r1:4;
		u16 p_r2:4;
		u16 p_r3:4;
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
u16 vga[240][320];

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

bool runop()
{
	opcode_t op;
	
	op.data = readm(state.pc);
	
	state.pc++;

	if (!op.is_not_jump)
	{
		state.pc = op.imm15;
	}
	else
	{
		switch(op.s1) {
			case s1_draw:	break;

			case s1_movh:	break;

			case s1_movl:	break;

			case s1_beq:	break;

			case s1_bgt:	break;

			case s1_ba:		break;
			
			case s1_ext_s2:
				switch(op.sx)
				{
					case s2_mov: break;
					case s2_add: break;
					case s2_sub: break;
					
					case s2_neg: break;
					case s2_not: break;
					
					case s2_and: break;
					case s2_or: break;
					case s2_xor: break;

					case s2_shl:
						break;
					case s2_shr:
						break;
					case s2_sar:
						break;

					case s2_read16:
						break;
					case s2_write16:
						break;

					case s2_addi:
						break;
					case s2_subi:
						break;
				}
			break;

			case s1_ext_s3:
				switch(op.sx)
				{
				case s3_jr:
					break;

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
		runop();
}