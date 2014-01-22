`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date:    20:49:17 01/22/2014 
// Design Name: 
// Module Name:    vga 
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

`define LINE_TOP		( (`VGA_VSIZE-`VSIZE)/2 )
`define LINE_BOTTOM	( `LINE_TOP + `VSIZE )

`define PIXEL_LEFT	( (`VGA_HSIZE-`HSIZE)/2 )
`define PIXEL_RIGHT	( `PIXEL_LEFT + `HSIZE )

module vga(CLK, I_RESET, O_VRAM_ADDR, I_VRAM_DATA, O_VSYNC, O_HSYNC, O_VIDEO_R, O_VIDEO_B, O_VIDEO_G);

	input CLK, I_RESET;
	output reg O_VSYNC;
	output reg O_HSYNC;
	output reg [3:0] O_VIDEO_R;
	output reg [3:0] O_VIDEO_B;
	output reg [3:0] O_VIDEO_G;
	
	input wire [2:0] I_VRAM_DATA;
	output wire [15:0] O_VRAM_ADDR;
	
	//internal state for
	reg [9:0] pixel; //current pixel, 0 to 831
	reg [9:0] line;  //current line, 0 to 519
	
	//continuous assignment
	//the moment pixel changes in_rgb is "instantly" updated
	wire in_rgb = (pixel < `VGA_HSIZE) && (line < `VGA_VSIZE);
	
	wire in_window =	line >= `LINE_TOP && line < `LINE_BOTTOM && 
							pixel >= `PIXEL_LEFT && pixel < `PIXEL_RIGHT;
							
	assign O_VRAM_ADDR = in_window ? ( (line-`LINE_TOP) * `HSIZE + (pixel-`PIXEL_LEFT) ) : 0;
		
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
				O_VIDEO_R = /*vram_addrb[15:12]^*/{4{I_VRAM_DATA[0]}};
				O_VIDEO_B = /*vram_addrb[11:8]^*/{4{I_VRAM_DATA[1]}};
				O_VIDEO_G = /*vram_addrb[7:4]^*/{4{I_VRAM_DATA[2]}};		
			end
			else
			begin
				O_VIDEO_R = 3 ;
				O_VIDEO_B = 3 ;
				O_VIDEO_G = 3 ;
			end
		end
		else
		begin
			O_VIDEO_R = 0;
			O_VIDEO_B = 0;
			O_VIDEO_G = 0;
		end
	end

endmodule
