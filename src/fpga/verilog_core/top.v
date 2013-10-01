module VGAFUN(CLK, LED, I_RESET, O_VSYNC, O_HSYNC, O_VIDEO_R, O_VIDEO_B, O_VIDEO_G, I_SW);

	input CLK;
	input I_RESET;
	input [3:0] I_SW;

	output reg [3:0] LED;
	output reg O_VSYNC;
	output reg O_HSYNC;
	output reg [3:0] O_VIDEO_R;
	output reg [3:0] O_VIDEO_B;
	output reg [3:0] O_VIDEO_G;
	
	//internal storage for I_SW latch
	reg [3:0] sw;

	//internal state for
	reg [9:0] pixel; //current pixel, 0 to 831
	reg [9:0] line;  //current line, 0 to 519
	
	//is 1 in the Display Active region and 0 in the blanking region
	//used to mask the RGB output
	wire in_rgb;
	
	//continuous assignment
	//the moment pixel changes in_rgb is "instantly" updated
	assign in_rgb = (pixel < 640) && (line < 480);
	
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
		
		//Generate a pseudo random image, or a solid filled image, or a mix
		//depending on the sw inputs
		O_VIDEO_R = {4{in_rgb}} & ((pixel ^ (line >> 0)) & ~{4{sw[3]}} | {4{sw[0]}});
		O_VIDEO_B = {4{in_rgb}} & ((pixel ^ (line >> 2)) & ~{4{sw[3]}} | {4{sw[1]}});
		O_VIDEO_G = {4{in_rgb}} & ((pixel ^ (line >> 4)) & ~{4{sw[3]}} | {4{sw[2]}});
		
		//Make the leds follow the SW and light up if reset
		LED = I_SW ^ {4{I_RESET}};
	end
	
endmodule
