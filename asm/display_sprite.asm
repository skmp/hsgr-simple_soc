              ORG 0x0000

	      LI r0, 0
	      LI r1, 1
	      LI r2, 100
	      
	      LI r11, 100   ; X coordinate
	      LI r12, 100   ; Y coordinate

s1	      LI r10, mario_right_1
	      LI r13, c1    ; return address
	      j draw_sprite  
c1            LI r13, s2    ; return address
              j clear_sprite
	      
s2	      LI r10, mario_right_2
	      LI r13, c2
	      j draw_sprite
c2            LI r13, s3    ; return address
              j clear_sprite
	      
s3	      LI r10, mario_right_3
	      LI r13, c3
	      j draw_sprite  
c3            LI r13, s4    ; return address
              j clear_sprite
	      
s4 	      sub r2, r1
              beq r2, r0, start
	      j s1


	      
start         LI r0, 0x8    ; 8    max_color
              LI r1, 0xc7   ; 200  max_pixel   
              LI r2, 0x1    
              LI r3, 0x0    ; X
              LI r4, 0x0    ; Y
              LI r5, 0x1    ; Color
         
	 
set_pixel     MOV r6, r5
              DRAW r3, r4, r6
              

              ; walk through the columns of the row
              ADD r3, r2   ; X = X+1
              BGT r3, r1, next_row
              J set_pixel

next_row      MOVL r3, 0   ; X = 0
              ADD r4, r2   ; Y = Y + 1
              BGT r4, r1, next_color
              J set_pixel
	 
next_color    MOVL r3, 0   ; X = 0
              MOVL r4, 0   ; Y = 0
	      ADD r5, r2   ; next color
              BGT r5 r0, zero_color
              J set_pixel

zero_color    MOVL r5, 0
              J set_pixel



;; Sub routines
;
;
;
;
;
; r10 holds the sprite to be drawn
; r11 holds the X coordinate
; r12 holds the Y coordinate
; r13 return address
; r14 temp var
; r15 temp var
;
draw_sprite  li r15, draw_sprite_start
             j save_reg

draw_sprite_start li r15, draw_sprite_return_address
             write_16, r13, r15  ; return address saved	     

             li r15, draw_sprite_width
             read_u8 r13, r10    ; r13 is the width
             write_8 r13, r15
	     
	     li r0, 0
	     li r1, 1
             add r10, r1
             li r15, draw_sprite_height
             read_u8 r14, r10    ; r14 is the height
             write_8 r14, r15

	     add r10, r1
	     
draw_sprite_loop read_u8 r15, r10    ; r15 is the pixel value
             DRAW r11, r12, r15
	     
	     sub r13, r1         ; pixels width left 
	     add r10, r1         ; next pixel
	     add r11, r1         ; next col
	     beq r13, r0, draw_sprite_next_row
	     J draw_sprite_loop

draw_sprite_next_row add r12, r1
             li r15, draw_sprite_width
             read_u8 r13, r15
	     sub r11, r13
	     
	     sub r14, r1         ; pixels height left 
	     beq r14, r0, draw_sprite_finish
	     J draw_sprite_loop


draw_sprite_finish li r15, draw_sprite_return
             j load_reg

draw_sprite_return li r15, draw_sprite_return_address
             read_16 r13, r15
             JR r13      ; return



	     
	     
	     
	     
	     
	     
	     
	     
	     
	     
	     
	     
clear_sprite  li r15, clear_sprite_start
             j save_reg

clear_sprite_start li r15, draw_sprite_return_address
             write_16, r13, r15  ; return address saved	     

             li r15, draw_sprite_width
             read_u8 r13, r10    ; r13 is the width
             write_8 r13, r15
	     
	     li r0, 0
	     li r1, 1
             add r10, r1
             li r15, draw_sprite_height
             read_u8 r14, r10    ; r14 is the height
             write_8 r14, r15

	     add r10, r1
	     
clear_sprite_loop mov r15, r0    ; r15 is the pixel value
             DRAW r11, r12, r15        ; r15 always black
	     
	     sub r13, r1         ; pixels width left 
	     add r10, r1         ; next pixel
	     add r11, r1         ; next col
	     beq r13, r0, clear_sprite_next_row
	     J clear_sprite_loop

clear_sprite_next_row add r12, r1
             li r15, draw_sprite_width
             read_u8 r13, r15
	     sub r11, r13
	     
	     sub r14, r1         ; pixels height left 
	     beq r14, r0, clear_sprite_finish
	     J clear_sprite_loop


clear_sprite_finish li r15, clear_sprite_return
             j load_reg

clear_sprite_return li r15, draw_sprite_return_address
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

draw_sprite_return_address DW 0 ; this memory location holds the return address for the draw sprite sub-routine
draw_sprite_width DB 0 ; this memory location holds the sprite width
draw_sprite_height DB 0 ; this memory location holds the sprite height

org 0x4000

mario_left_1 DB 15  ; Width
DB 16  ; Height

DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 3
DB 2
DB 2
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 2
DB 2
DB 2
DB 2
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 0
DB 0
DB 0
DB 3
DB 3
DB 1
DB 1
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 2
DB 2
DB 2
DB 3
DB 3
DB 3
DB 1
DB 1
DB 1
DB 3
DB 3
DB 3
DB 3
DB 2
DB 2
DB 2
DB 2
DB 3
DB 3
DB 1
DB 1
DB 1
DB 2
DB 1
DB 3
DB 3
DB 0
DB 2
DB 2
DB 2
DB 0
DB 3
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 2
DB 2
DB 0
DB 3
DB 3
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 3
DB 3
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 1
DB 1
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 0
DB 0


mario_left_2 DB 15  ; Width
DB 16  ; Height

DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 3
DB 2
DB 2
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 2
DB 2
DB 2
DB 2
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 0
DB 1
DB 3
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 3
DB 3
DB 3
DB 3
DB 3
DB 3
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 3
DB 3
DB 3
DB 3
DB 1
DB 1
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 3
DB 3
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0


mario_left_3 DB 12  ; Width
DB 16  ; Height

DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 0
DB 2
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 3
DB 2
DB 2
DB 3
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 2
DB 2
DB 2
DB 2
DB 3
DB 3
DB 3
DB 0
DB 0
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 1
DB 1
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 2
DB 1
DB 1
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 2
DB 2
DB 2
DB 3
DB 1
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 2
DB 2
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 0
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 0
DB 0
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 0
DB 3
DB 3
DB 3
DB 3
DB 0


mario_right_1 DB 12  ; Width
DB 16  ; Height

DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 2
DB 0
DB 0
DB 3
DB 2
DB 2
DB 3
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 2
DB 3
DB 3
DB 3
DB 2
DB 2
DB 2
DB 2
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 1
DB 1
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 1
DB 1
DB 2
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 1
DB 3
DB 2
DB 2
DB 2
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 1
DB 1
DB 2
DB 2
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 0
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 0
DB 0
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 0
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0


mario_right_2 DB 15  ; Width
DB 16  ; Height

DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 2
DB 2
DB 3
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 2
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 2
DB 2
DB 2
DB 2
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 3
DB 1
DB 0
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 3
DB 3
DB 3
DB 3
DB 3
DB 3
DB 2
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 1
DB 1
DB 3
DB 3
DB 3
DB 3
DB 2
DB 2
DB 0
DB 0
DB 0
DB 3
DB 3
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 3
DB 3
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 3
DB 3
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0


mario_right_3 DB 15  ; Width
DB 16  ; Height

DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 2
DB 2
DB 3
DB 3
DB 2
DB 2
DB 3
DB 2
DB 2
DB 2
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 2
DB 2
DB 2
DB 2
DB 3
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 3
DB 1
DB 1
DB 3
DB 3
DB 0
DB 0
DB 0
DB 2
DB 2
DB 2
DB 2
DB 3
DB 3
DB 3
DB 3
DB 1
DB 1
DB 1
DB 3
DB 3
DB 3
DB 2
DB 2
DB 2
DB 2
DB 2
DB 2
DB 0
DB 3
DB 3
DB 1
DB 2
DB 1
DB 1
DB 1
DB 3
DB 3
DB 2
DB 2
DB 2
DB 2
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 0
DB 0
DB 3
DB 0
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 3
DB 3
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 1
DB 3
DB 3
DB 0
DB 0
DB 3
DB 3
DB 3
DB 1
DB 1
DB 0
DB 0
DB 0
DB 1
DB 1
DB 1
DB 3
DB 3
DB 0
DB 0
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 3
DB 3
DB 3
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0
DB 0

