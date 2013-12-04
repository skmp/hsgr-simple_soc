`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date:    20:16:10 09/25/2013 
// Design Name: 
// Module Name:    core 
// Project Name: 
// Target Devices: 
// Tool versions: 
// Description: 
//
// Dependencies: 
//
// Revision: 
// Revision 0.01 - File Created
// Additional Comments: 
//
//////////////////////////////////////////////////////////////////////////////////

`define VGA_HSIZE 640
`define VGA_VSIZE 480

`define HSIZE 256
`define VSIZE 256

`define VGA_PIXELS (VGA_VSIZE * VGA_HSIZE)
`define PIXELS (VSIZE * HSIZE)

module core(CLK_31M25, LED, I_RESET, O_VSYNC, O_HSYNC, O_VIDEO_R, O_VIDEO_B, O_VIDEO_G, I_SW, O_TX, I_RX);

`define s1_draw 0
`define s1_movh 1
`define s1_movl 2
`define s1_beq 3
`define s1_bgt 4
`define s1_ba 5
`define s1_ext_s2 6
`define s1_ext_s3 7


`define s2_mov 0
`define s2_add 1
`define s2_sub 2
`define s2_neg 3

`define s2_and 4
`define s2_or 5
`define s2_xor 6
`define s2_not 7

`define s2_shl 8
`define s2_shr 9
`define s2_sar 10
`define s2_read16 11

`define s2_write16 12
`define s2_addi 13
`define s2_subi 14
`define s2_undefined 15

`define s3_jr 0
`define s3_wait 1
	

`define state_fetch 0
`define state_fetch_addr 1
`define state_fetch_data 2
`define state_execute 3
`define state_memaccess 4
`define state_memaccess_data 5
`define state_vram_write 6
`define state_vwait_in 7
`define state_vwait_out 8

	
	//internal state for
	reg [9:0] pixel; //current pixel, 0 to 831
	reg [9:0] line;  //current line, 0 to 519
	
	//continuous assignment
	//the moment pixel changes in_rgb is "instantly" updated
	wire in_rgb = (pixel < `VGA_HSIZE) && (line < `VGA_VSIZE);
	
	
	input I_RX;
	output O_TX;
	
	input CLK_31M25;
	wire CLK;
	input I_RESET;
	input [3:0] I_SW;

	output reg [3:0] LED;
	output reg O_VSYNC;
	output reg O_HSYNC;
	output reg [3:0] O_VIDEO_R;
	output reg [3:0] O_VIDEO_B;
	output reg [3:0] O_VIDEO_G;

	reg [12:0] ram_address;
	reg [15:0] ram_in;
	wire [15:0] ram_out;
	reg  ram_we;
	
	reg vram_we;
	reg [15:0] vram_address;
	reg [2:0] vram_in;
	wire [2:0] vram_out;
	
	mem ram (
	.clka(CLK), // input clka
	.wea(ram_we), // input [0 : 0] wea
	.addra(ram_address), // input [12 : 0] addra
	.dina(ram_in), // input [15 : 0] dina
	.douta(ram_out) // output [15 : 0] douta
	);
	
	wire [15:0] vram_addrb;
	wire [2:0] vram_doutb;
	
	vram vram (
	  .clka(CLK), // input clka
	  .rsta(I_RESET), // input rsta
	  .wea(vram_we), // input [0 : 0] wea
	  .addra(vram_address), // input [16 : 0] addra
	  .dina(vram_in), // input [2 : 0] dina
	  .douta(vram_out), // output [2 : 0] douta
	  
	  .clkb(CLK), // input clkb
	  .web(0), // input [0 : 0] web
	  .addrb(vram_addrb), // input [16 : 0] addrb
	  .dinb(vram_dinb), // input [2 : 0] dinb
	  .doutb(vram_doutb) // output [2 : 0] doutb
	);
	
	
	reg [15:0] regs[15:0];
	reg [15:0] pc;
	
	reg [3:0] state;
	
	always@ (posedge CLK)
	begin
		//LED = 0;
	end
	
	reg [15:0] opcode;
	
	wire op_is_not_jump = opcode[15];
	
	wire [2:0] op_s1 = opcode[14:12];
	wire [3:0] op_s2 = opcode[11:8];
	wire [7:0] op_s3 = opcode[11:4];
	wire [14:0] op_imm15 = opcode[14:0];
	wire [7:0] op_imm8 = opcode[7:0];
	wire [3:0] op_imm4 = opcode[3:0];
	wire [15:0] op_imm4_s16 = { {12{op_imm4[3]}}, op_imm4[3:0] };
	
	wire [3:0] op_r3 = opcode[3:0];
	wire [3:0] op_r2 = opcode[7:4];
	wire [3:0] op_r1 = opcode[11:8];
	
	always@ (posedge CLK)
	begin
		if (I_RESET == 1)
		begin
			//if reset, then clear the state
			regs[0]=0;
			regs[1]=0;
			regs[2]=0;
			regs[3]=0;
			
			regs[4]=0;
			regs[5]=0;
			regs[6]=0;
			regs[7]=0;
			
			regs[8]=0;
			regs[9]=0;
			regs[10]=0;
			regs[11]=0;
			
			regs[12]=0;
			regs[13]=0;
			regs[14]=0;
			regs[15]=0;
			
			state=`state_fetch;
			pc=0;
			ram_address=0;
			ram_we=0;
			
			vram_we = 0;
			vram_address = 0;
			vram_in = 0;

			//LED = 4;
		end
		else
		begin
			//Do something
			
			case(state)
				`state_fetch:
				begin
					ram_address = pc;
					// advance the state
					state=`state_fetch_addr;
				end
				
				`state_fetch_addr:
				begin
					// advance the state
					state=`state_fetch_data;
				end
				
				`state_fetch_data:
				begin
					state=`state_execute;
					opcode = ram_out;
					pc = pc +1;
				end

				`state_execute:
				begin
					// advance the state
					state = `state_fetch;
					
					if (op_is_not_jump == 0)
					begin
						pc = op_imm15;
					end
					else
					begin
						case(op_s1)
							`s1_draw:
							begin
								state=`state_vram_write;
								vram_address = regs[op_r2]* `HSIZE + regs[op_r1];
								vram_in = regs[op_r3];
								vram_we = 1;
							end
							`s1_movh: regs[op_r1][15:8] = op_imm8;
							`s1_movl: regs[op_r1] = op_imm8;
							`s1_beq: 
							if (regs[op_r1] == regs[op_r2]) begin
								pc = pc + op_imm4_s16 - 1;
							end
							`s1_bgt: 
							if ($signed(regs[op_r1]) > $signed(regs[op_r2])) begin
								pc = pc + op_imm4_s16 - 1;
							end
							`s1_ba: 
							if (regs[op_r1] > regs[op_r2]) begin
								pc = pc + op_imm4_s16 - 1;
							end
							`s1_ext_s2:
							begin
								case (op_s2)
									`s2_mov: regs[op_r2] = regs[op_r3];
									`s2_add: regs[op_r2] = regs[op_r2] + regs[op_r3];
									`s2_sub: regs[op_r2] = regs[op_r2] - regs[op_r3];
									
									`s2_and: regs[op_r2] = regs[op_r2] & regs[op_r3];
									`s2_or:  regs[op_r2] = regs[op_r2] | regs[op_r3];
									`s2_xor: regs[op_r2] = regs[op_r2] ^ regs[op_r3];
									
									`s2_neg: regs[op_r2] = -regs[op_r3];
									`s2_not: regs[op_r2] = ~regs[op_r3];

									`s2_shl: regs[op_r2] =  regs[op_r2] << op_imm4;
									`s2_shr: regs[op_r2] = regs[op_r2] >> op_imm4;
									`s2_sar: regs[op_r2] = regs[op_r2] >>> op_imm4;
									`s2_read16:
									begin
										state = `state_memaccess;
										ram_address = regs[op_r3];
									end

									`s2_write16:
									begin
										state = `state_memaccess;
										ram_address = regs[op_r3];
										ram_in = regs[op_r2];
										ram_we = 1;
										//regs[op_r2] = ram[];
									end
									`s2_addi: regs[op_r2] = regs[op_r2] + op_imm4;
									`s2_subi: regs[op_r2] = regs[op_r2] - op_imm4;
									
									`s2_undefined: pc = pc +2;
								endcase
							end
							`s1_ext_s3: 
							case(op_s3)
							`s3_jr: pc = regs[op_r3];
							`s3_wait: state = `state_vwait_in;
							endcase
						endcase
					end
				end
				
				`state_vwait_in:
				begin
					if (O_VSYNC == 1)
						state = `state_vwait_out;
				end
				
				`state_vwait_out:
				begin
					if (O_VSYNC == 0)
						state = `state_fetch;
				end
				
				`state_memaccess:
				begin
					state = `state_memaccess_data;
				end
				
				`state_memaccess_data:
				begin
					state = `state_fetch;
					if (ram_we == 0)
						regs[op_r2] = ram_out;
					ram_we = 0;
				end
				
				`state_vram_write:
				begin
					state = `state_fetch;
					vram_we=0;
				end
			endcase
		end
	end

	always@ (posedge CLK)
	begin
		
	end
	
	`define LINE_TOP		( (`VGA_VSIZE-`VSIZE)/2 )
	`define LINE_BOTTOM	( `LINE_TOP + `VSIZE )
	
	`define PIXEL_LEFT	( (`VGA_HSIZE-`HSIZE)/2 )
	`define PIXEL_RIGHT	( `PIXEL_LEFT + `HSIZE )
	
	wire in_window =	line >= `LINE_TOP && line < `LINE_BOTTOM && 
							pixel >= `PIXEL_LEFT && pixel < `PIXEL_RIGHT;
							
	assign vram_addrb = in_window ? ( (line-`LINE_TOP) * `HSIZE + (pixel-`PIXEL_LEFT) ) : 0;
	
	reg sw;
	
	//Input switch latch, to avoid flicker
	//this guarantees that the sw value does not change mid-frame
	always@ (posedge CLK)
	begin
		if (pixel == 0 && line == 0)
			sw = I_SW;
	end
	
	//Main processing logic
	//hsync and vsync generation
	//image generation
	//reset logic
	always@ (posedge CLK)
	begin
		if (I_RESET == 1)
		begin
			line = 0;
			pixel = 0;
		end
		
		//count from 0 to 831
		//essentially a clock divider to 32 mhz/832
		pixel = pixel + 1;
		if (pixel == 832) //warp back to 0 on 832
			pixel = 0;
		
		//if at the start of a line, count lines
		if (pixel == 0)
		begin
			//count lines from 0 to 519
			line = line + 1;
			if (line == 520)
				line = 0;
		end
		
		//hsync is zero in [664 ~ 703]
		//vsync is zero in [489 ~ 490]
		O_HSYNC = (pixel >= 664 && pixel <= 703) ? 0 : 1;
		O_VSYNC = (line >= 489 && line <= 490) ? 0 : 1;
		
		if (in_rgb)
		begin
			if (in_window)
			begin
				O_VIDEO_R = /*vram_addrb[15:12]^*/{4{vram_doutb[0]}};
				O_VIDEO_B = /*vram_addrb[11:8]^*/{4{vram_doutb[1]}};
				O_VIDEO_G = /*vram_addrb[7:4]^*/{4{vram_doutb[2]}};		
			end
			else
			begin
				O_VIDEO_R = 3 ^ sw;
				O_VIDEO_B = 3 ^ sw;
				O_VIDEO_G = 3 ^ sw;
			end
		end
		else
		begin
			O_VIDEO_R = 0;
			O_VIDEO_B = 0;
			O_VIDEO_G = 0;
		end
		
		//Make the leds follow the SW and light up if reset
		LED = I_SW ^ {4{I_RESET}};
	end




	reg [15:0] ice_addr;
	reg [15:0] ice_data;


	wire [7:0] ice_in;
	reg en_16_x_baud;
	wire data_present;
	reg [2:0] baud_count;
	
	
	uart_tx uart_tx (
	  .data_in(ice_addr),
	  .write_buffer(data_present),
	  .reset_buffer(0),
	  .en_16_x_baud(en_16_x_baud),
	  .clk(CLK96),
	  .serial_out(O_TX)
	  /*
	  .buffer_half_full(),
	  .buffer_full(),
	  */
	);
	
	uart_rx uart_rx (
		.serial_in(I_RX),
		.read_buffer(1),
		.reset_buffer(0),
		
		.en_16_x_baud(en_16_x_baud),
		.clk(CLK96),
		.data_out(ice_in),
		.buffer_data_present(data_present)
/*	
		.buffer_half_full(rx),
		.buffer_full(rx),
*/
	);
	
	assign ice_in_sf = (4*ice_in[5:4]);
	assign ice_in_data = ice_in[3:0];
	assign ice_in_cmd = ice_in[7:4];
	
	reg ice_core_reset;
	reg [1:0] ice_core_state;
	
	always@ (posedge CLK96)
	begin
		if (data_present == 1)
		begin
			if (ice_in_cmd<4)
			begin
				ice_addr = ice_addr & ~(15<<ice_in_sf) | (ice_in_data<<ice_in_sf); 
			end
			else if (ice_in_cmd<8)
			begin
				ice_data = ice_data & ~(15<<ice_in_sf) | (ice_in_data<<ice_in_sf);
			end
			else if (ice_in_cmd == 8)
			begin
				
			end
			else if (ice_in_cmd == 9)
			begin
				
			end
			else if (ice_in_cmd == 10)
			begin
				
			end
			else if (ice_in_cmd == 11)
			begin
				case (ice_in_data)
					0: ice_core_reset = 0;
					1: ice_core_reset = 1;
					
					2: ice_core_state = 0;
					3: ice_core_state = 1;
					4: ice_core_state = 2;
					
					5: pc = ice_data;
					6: ice_data = pc;
				endcase
			end
		end
	end

	always@ (posedge CLK96)
	begin
		if (baud_count==1)
		begin
			baud_count = 0;
			en_16_x_baud = 1;
		end
		else
		begin
			baud_count = baud_count + 1;
			en_16_x_baud = 0;
		end
	end

	dcm32to96 serial_clock_dcm (
    .CLKIN_IN(CLK_31M25), 
    .CLKFX_OUT(CLK96),
	 .CLK0_OUT(CLK)
	 /*
    .CLKIN_IBUFG_OUT(CLK), 
	 */
    );
	
endmodule

