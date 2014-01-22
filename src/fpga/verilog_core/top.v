`timescale 1ns / 1ps

module top(CLK_31M25, LED, I_RESET, I_SW, O_TX, I_RX, O_VSYNC, O_HSYNC, O_VIDEO_R, O_VIDEO_B, O_VIDEO_G);

	input CLK_31M25, I_RESET, I_RX;
	output O_TX, O_VSYNC, O_HSYNC;
	output wire [3:0] O_VIDEO_R, O_VIDEO_B, O_VIDEO_G;

	output [3:0] LED;
	input [3:0] I_SW;
	/*
		
		Clock domains
		
			CLK_31M25 - input 31.25 for DCM
			
			Output from DCM
			- CLK_CPU/96mhz - CPU & Serial & VRAM/0 & RAM
			- CLK_VGA/31.25mhz in phase - VGA & VRAM/1
			
			
			In phase clocks
			
			CLKin     |||___|||___|||
			
			CLK_CPU    |_|_|_|_|_|_|_|
			CLK_VGA	  |||___|||___|||
	*/

	wire CLK_CPU;
	wire CLK_VGA;

	wire [15:0] CPU_VBUS_ADDR;
	wire [2:0]	CPU_VBUS_DATA_TOVRAM;
	wire [2:0]	CPU_VBUS_DATA_FROMVRAM;
	wire 			CPU_VBUS_WE;
	
	wire [15:0] VGA_VBUS_ADDR;
	wire [2:0]	VGA_VBUS_DATA;
	
	wire [7:0]	ICE_BUS_CMD;
	wire [7:0]	ICE_BUS_RESP;
	wire [15:0]	ICE_BUS_TOICE;
	wire [15:0]	ICE_BUS_FROMICE;
	
	assign I_NRESET = ~I_RESET;

	assign LED = I_SW;	
	
	dcm32to96 serial_clock_dcm (
		.CLKIN_IN(CLK_31M25), 
		.CLKFX_OUT(CLK_CPU),	//3*31.25 mhz
		.CLK0_OUT(CLK_VGA)		//same as input, in phase with FX_OUT
	);

	vram vram (
		.rsta(I_RESET), // input rsta

		//vram/cpu
		.clka(CLK_CPU), // input clka  
		.addra(CPU_VBUS_ADDR), // input [16 : 0] addra
		.douta(CPU_VBUS_DATA_FROMVRAM), // output [2 : 0] douta
		.wea(CPU_VBUS_WE), // input [0 : 0] wea
		.dina(CPU_VBUS_DATA_TOVRAM), // input [2 : 0] dina

		//vram/vga ramdac
		.clkb(CLK_VGA),
		.addrb(VGA_BUS_ADDR),
		.doutb(VGA_BUS_DATA), // output [2 : 0] doutb
		.web(0),	//no writes
		.dinb(0)
	);
	
	core core (
		.CLK(CLK_CPU),
		.O_LED(O_LED),
		.I_RESET(I_RESET),
		.I_SW(I_SW),

		.O_VBUS_ADDR(CPU_VBUS_ADDR),
		.O_VBUS_WE(CPU_VBUS_WE),
		.O_VBUS_DATA_TOVRAM(CPU_VBUS_DATA_TOVRAM),
		.I_VBUS_DATA_FROMVRAM(CPU_VBUS_DATA_FROMVRAM),

		.I_VGA_VSYNC(O_VSYNC),

		.I_ICE_BUS_CMD(ICE_BUS_CMD),
		.O_ICE_BUS_RESP(ICE_BUS_RESP),
		.O_ICE_BUS_TOICE(ICE_BUS_TOICE),
		.I_ICE_BUS_FROMICE(ICE_BUS_FROMICE)
	);
	
	vga vga (
		.CLK(CLK_VGA),
		.I_RESET(I_RESET),
		
		.O_VRAM_ADDR(VGA_VBUS_ADDR),
		.I_VRAM_DATA(VGA_VBUS_DATA),
		
		.O_VSYNC(O_VSYNC),
		.O_HSYNC(O_HSYNC),
		.O_VIDEO_R(O_VIDEO_R),
		.O_VIDEO_B(O_VIDEO_B),
		.O_VIDEO_G(O_VIDEO_G)
	);
	
	ice ice(
		.CLK(CLK_CPU),
		.O_TX(O_TX),
		.I_RX(I_RX),
		
		.ICE_BUS_CMD(ICE_BUS_CMD),
		.ICE_BUS_RESP(ICE_BUS_RESP),
		.ICE_BUS_TOICE(ICE_BUS_TOICE),
		.ICE_BUS_FROMICE(ICE_BUS_FROMICE)
	);
	
endmodule
