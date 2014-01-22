`timescale 1ns / 1ps

module ice(
	CLK, O_TX, I_RX, ICE_BUS_CMD, ICE_BUS_RESP, ICE_BUS_TOICE, ICE_BUS_FROMICE
);
	
	input CLK, I_RX, ICE_BUS_RESP, ICE_BUS_TOICE;
	output O_TX, ICE_BUS_CMD, ICE_BUS_FROMICE;
	
	reg [15:0] ice_addr;
	reg [15:0] ice_data;


	wire [7:0] ice_in;
	reg en_16_x_baud;
	wire data_present;
	reg delay_data_present;
	reg [2:0] baud_count;
	
	
	uart_tx uart_tx (
	  .data_in(ice_addr),
	  .write_buffer(delay_data_present),
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
	
	wire [3:0] ice_in_sf;
	wire [3:0] ice_in_data;
	wire [3:0] ice_in_cmd;
	
	assign ice_in_sf = (4*ice_in[5:4]);
	assign ice_in_data = ice_in[3:0];
	assign ice_in_cmd = ice_in[7:4];
	
	reg ice_core_reset;
	reg [1:0] ice_core_state;
	
	always@ (posedge CLK96)
	begin
		delay_data_present = data_present;
	
		if (data_present == 1)
		begin
			if (ice_in_cmd<4)
			begin
				ice_addr = ice_addr & ~(16'hf<<ice_in_sf) | ({{12{0}},ice_in_data}<<ice_in_sf); 
			end
			else if (ice_in_cmd<8)
			begin
				ice_data = ice_data & ~(16'hf<<ice_in_sf) | ({{12{0}},ice_in_data}<<ice_in_sf);
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
					
					//5: pc = ice_data;
					//6: ice_data = pc;
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

endmodule
