              ORG 0x0000
	      
init          LI r0, 0x0    ; 8    max_color
              LI r1, 0xff   ; 255  max_pixel   
              LI r2, 0x1    
              LI r3, 0x0    ; X
              LI r4, 0x0    ; Y
              LI r5, 0x1    ; Color Red

start	      LI r13, bs1
	      J blank_screen

bs1	      MOV r6, r5
              DRAW r3, r4, r6

	      MOV r7, r3
	      MOV r8, r4
	      BGT r7, r0, X_greater
	      J bs2

X_greater     SUB r7, r2
              MOVL r9, 7
              DRAW r7, r8, r9
	      Wait 0
	      ; if X>0 
	      ;LI r13, bs2
	      ;J blank_screen

              ; walk through the columns of the row
bs2           ADD r3, r2   ; X = X+1
              BGT r3, r1, next_row
              J bs1

next_row      MOVL r3, 0   ; X = 0
              ADD r4, r2   ; Y = Y + 1
              BGT r4, r1, next_iteration
              J bs1
	 
next_iteration    MOVL r3, 0   ; X = 0
              MOVL r4, 0   ; Y = 0

	      WAIT 0


	      J start

	      
	      
;
; r1 holds the rectangle size
;
; r13 return address
; r14 temp var
; r15 temp var
;
blank_screen_return_address DW 0 ; this memory location holds the return address for the blank screen sub-routine

blank_screen  li r15, bs_start
              j save_reg

bs_start      li r15, blank_screen_return_address
              write_16, r13, r15  ; return address saved	     

              LI r2, 0x1    
              LI r3, 0x0    ; X start
              LI r4, 0x0    ; Y start
	      li r5, 0x7    ; black color

bs_set_pixel  MOV r6, r5
              DRAW r3, r4, r6

              ; walk through the columns of the row
	      ADD r3, r2   ; X = X+1
              BGT r3, r1, bs_next_row
              J bs_set_pixel

bs_next_row   MOVL r3, 0   ; X = 0
              ADD r4, r2   ; Y = Y + 1
              BGT r4, r1, bs_finish
              J bs_set_pixel

bs_finish     WAIT 0
              li r15, bs_return
              j load_reg

bs_return     li  r15, blank_screen_return_address
              read_16 r13, r15
              JR r13      ; return
	     
	     
	     
	    	     
	     
	     
save_reg     LI r14, save_reg_0
             WRITE_16 r0, r14             
             LI r14, save_reg_1
             WRITE_16 r1, r14
             LI r14, save_reg_2
             WRITE_16 r2, r14
             LI r14, save_reg_3
             WRITE_16 r3, r14
             LI r14, save_reg_4
             WRITE_16 r4, r14
             LI r14, save_reg_5
             WRITE_16 r5, r14
             LI r14, save_reg_6
             WRITE_16 r6, r14
             LI r14, save_reg_7
             WRITE_16 r7, r14
             LI r14, save_reg_8
             WRITE_16 r8, r14
             LI r14, save_reg_9
             WRITE_16 r9, r14
             LI r14, save_reg_10
             WRITE_16 r10, r14
             LI r14, save_reg_11
             WRITE_16 r11, r14
             LI r14, save_reg_12
             WRITE_16 r12, r14
             LI r14, save_reg_13
             WRITE_16 r13, r14

             JR r15      ; return

	     
load_reg     LI r14, save_reg_0
             READ_16 r0, r14             
             LI r14, save_reg_1
             READ_16 r1, r14
             LI r14, save_reg_2
             READ_16 r2, r14
             LI r14, save_reg_3
             READ_16 r3, r14
             LI r14, save_reg_4
             READ_16 r4, r14
             LI r14, save_reg_5
             READ_16 r5, r14
             LI r14, save_reg_6
             READ_16 r6, r14
             LI r14, save_reg_7
             READ_16 r7, r14
             LI r14, save_reg_8
             READ_16 r8, r14
             LI r14, save_reg_9
             READ_16 r9, r14
             LI r14, save_reg_10
             READ_16 r10, r14
             LI r14, save_reg_11
             READ_16 r11, r14
             LI r14, save_reg_12
             READ_16 r12, r14
             LI r14, save_reg_13
             READ_16 r13, r14

             JR r15      ; return

	     
; Data section
; Data section
; Data section
; Data section
save_reg_0 DW 0
save_reg_1 DW 0
save_reg_2 DW 0
save_reg_3 DW 0
save_reg_4 DW 0
save_reg_5 DW 0
save_reg_6 DW 0
save_reg_7 DW 0
save_reg_8 DW 0
save_reg_9 DW 0
save_reg_10 DW 0
save_reg_11 DW 0
save_reg_12 DW 0
save_reg_13 DW 0

