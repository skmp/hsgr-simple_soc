#include <stdio.h>
#include <stdlib.h>
#include <cstdint>

#pragma comment(lib, "SDL.lib")

int realmain();

int main(int argc, char *argv[]) {
	realmain();
}

typedef uint16_t u16;
typedef int16_t s16;

extern u16 vga[320*240];

void runframe();
void loadfile(FILE* f);

#include <SDL/SDL.h>


/* Print modifier info */
void PrintModifiers( SDLMod mod ){
    printf( "Modifers: " );

    /* If there are none then say so and return */
    if( mod == KMOD_NONE ){
        printf( "None\n" );
        return;
    }

    /* Check for the presence of each SDLMod value */
    /* This looks messy, but there really isn't    */
    /* a clearer way.                              */
    if( mod & KMOD_NUM ) printf( "NUMLOCK " );
    if( mod & KMOD_CAPS ) printf( "CAPSLOCK " );
    if( mod & KMOD_LCTRL ) printf( "LCTRL " );
    if( mod & KMOD_RCTRL ) printf( "RCTRL " );
    if( mod & KMOD_RSHIFT ) printf( "RSHIFT " );
    if( mod & KMOD_LSHIFT ) printf( "LSHIFT " );
    if( mod & KMOD_RALT ) printf( "RALT " );
    if( mod & KMOD_LALT ) printf( "LALT " );
    if( mod & KMOD_CTRL ) printf( "CTRL " );
    if( mod & KMOD_SHIFT ) printf( "SHIFT " );
    if( mod & KMOD_ALT ) printf( "ALT " );
    printf( "\n" );
}

/* Print all information about a key event */
void PrintKeyInfo( SDL_KeyboardEvent *key ){
    /* Is it a release or a press? */
    if( key->type == SDL_KEYUP )
        printf( "Release:- " );
    else
        printf( "Press:- " );

    /* Print the hardware scancode first */
    printf( "Scancode: 0x%02X", key->keysym.scancode );
    /* Print the name of the key */
    printf( ", Name: %s", SDL_GetKeyName( key->keysym.sym ) );
    /* We want to print the unicode info, but we need to make */
    /* sure its a press event first (remember, release events */
    /* don't have unicode info                                */
    if( key->type == SDL_KEYDOWN ){
        /* If the Unicode value is less than 0x80 then the    */
        /* unicode value can be used to get a printable       */
        /* representation of the key, using (char)unicode.    */
        printf(", Unicode: " );
        if( key->keysym.unicode < 0x80 && key->keysym.unicode > 0 ){
            printf( "%c (0x%04X)", (char)key->keysym.unicode,
                    key->keysym.unicode );
        }
        else{
            printf( "? (0x%04X)", key->keysym.unicode );
        }
    }
    printf( "\n" );
    /* Print modifier info */
    PrintModifiers( (SDLMod)key->keysym.mod );
}


void put_pixel32( SDL_Surface *surface, int x, int y, Uint32 pixel )
{
    //Convert the pixels to 32 bit
    Uint32 *pixels = (Uint32 *)surface->pixels;
    
    //Set the pixel
    pixels[ ( y * surface->w ) + x ] = pixel;
}

uint32_t CLUT[8]= {
	0x00000000,			//black
	0x0000FF00,			//green
	0x0FF00000,			//red

	0x0FF000FF,			//red
	0x0FF0FF00,			//red
	0x0FF000F0,			//red

	0x0FFFD0F0,			//red
	0x000FD0F0,			//red
};


int realmain() {

	loadfile(fopen("../../../asm/display_sprite.bin","rb"));

	SDL_Event event;
    int quit = 0;
        
    /* Initialise SDL */
	if( SDL_Init( SDL_INIT_VIDEO | SDL_INIT_NOPARACHUTE) < 0){
        fprintf( stderr, "Could not initialise SDL: %s\n", SDL_GetError() );
        exit( -1 );
    }

	SDL_SetModuleHandle(GetModuleHandle(NULL));

    /* Set a video mode */
	SDL_Surface* screen = SDL_SetVideoMode( 640, 480, 0, 0 );

    if( !screen ){
        fprintf( stderr, "Could not set video mode: %s\n", SDL_GetError() );
        SDL_Quit();
        exit( -1 );
    }

    /* Loop until an SDL_QUIT event is found */
    while( !quit ){

		SDL_PumpEvents();
        /* Poll for events */
        while( SDL_PollEvent( &event ) ){
                
            switch( event.type ){
                /* Keyboard event */
                /* Pass the event data onto PrintKeyInfo() */
                case SDL_KEYDOWN:
                case SDL_KEYUP:
                    PrintKeyInfo( &event.key );
                    break;

                /* SDL_QUIT event (window close) */
                case SDL_QUIT:
                    quit = 1;
                    break;

                default:
                    break;
			}

		}

		if( SDL_MUSTLOCK( screen ) )	{	SDL_LockSurface( screen );	}

		runframe();

		for (int y = 0; y<240; y++) {
			for (int x = 0; x<320; x++) {
				put_pixel32(screen, x*2 + 0, y*2 + 0, CLUT[7&vga[y*320 + x]]);
				put_pixel32(screen, x*2 + 0, y*2 + 1, CLUT[7&vga[y*320 + x]]);
				put_pixel32(screen, x*2 + 1, y*2 + 0, CLUT[7&vga[y*320 + x]]);
				put_pixel32(screen, x*2 + 1, y*2 + 1, CLUT[7&vga[y*320 + x]]);
			}
		}

		if( SDL_MUSTLOCK( screen ) )	{	SDL_UnlockSurface( screen );	}

		SDL_Flip(screen); 

    }

    /* Clean up */
    SDL_Quit();
}
