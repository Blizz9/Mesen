using System;
using SDL2;

namespace Pong
{
	internal class Game
	{
		private IntPtr _sdlRenderer;

		// sprites
		private IntPtr _ballTexture;
		private IntPtr _paddleTexture;
		private IntPtr _wallTexture;

		internal Game()
		{
			// initialize SDL renderer
			SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
			IntPtr sdlWindow = SDL.SDL_CreateWindow("Pong v0.1", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 256, 240, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
			_sdlRenderer = SDL.SDL_CreateRenderer(sdlWindow, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

			// initialize ported version textures
			_ballTexture = SDL_image.IMG_LoadTexture(_sdlRenderer, "ball.png");
			_paddleTexture = SDL_image.IMG_LoadTexture(_sdlRenderer, "paddle.png");
			_wallTexture = SDL_image.IMG_LoadTexture(_sdlRenderer, "wall.png");
		}

		internal void OnOpExecSync(IntPtr p1, IntPtr p2, IntPtr p3, IntPtr p4, IntPtr p5, IntPtr p6, IntPtr p7, IntPtr p8)
		{
			ushort addr = (ushort)p1;
			ushort frame = (ushort)p2;
			ushort p1CtrlState = (ushort)p3;
			ushort p2CtrlState = (ushort)p4;
			ushort ballX = (ushort)p5;
			ushort ballY = (ushort)p6;
			ushort p1PaddleY = (ushort)p7;
			ushort p2PaddleY = (ushort)p8;
			//Console.WriteLine(string.Format("[{0}] {1}: {2}|{3} ({4}, {5})", addr, frame, p1CtrlState, p2CtrlState, ballX, ballY));

			// clear screen with black
			SDL.SDL_SetRenderDrawColor(_sdlRenderer, 0, 0, 0, 255);
			SDL.SDL_RenderClear(_sdlRenderer);

			// draw static walls (roof and floor)
			SDL.SDL_Rect wallRect = new SDL.SDL_Rect { x = 0, y = 64, w = 256, h = 8 };
			SDL.SDL_RenderCopy(_sdlRenderer, _wallTexture, IntPtr.Zero, ref wallRect);
			wallRect = new SDL.SDL_Rect { x = 0, y = 224, w = 256, h = 8 };
			SDL.SDL_RenderCopy(_sdlRenderer, _wallTexture, IntPtr.Zero, ref wallRect);

			// draw ball and paddles
			SDL.SDL_Rect ballRect = new SDL.SDL_Rect { x = (ballX + 1), y = (ballY + 1), w = 8, h = 8 };
			SDL.SDL_RenderCopy(_sdlRenderer, _ballTexture, IntPtr.Zero, ref ballRect);
			SDL.SDL_Rect p1PaddleRect = new SDL.SDL_Rect { x = 25, y = (p1PaddleY + 1), w = 8, h = 32 };
			SDL.SDL_RenderCopy(_sdlRenderer, _paddleTexture, IntPtr.Zero, ref p1PaddleRect);
			SDL.SDL_Rect p2PaddleRect = new SDL.SDL_Rect { x = 225, y = (p2PaddleY + 1), w = 8, h = 32 };
			SDL.SDL_RenderCopy(_sdlRenderer, _paddleTexture, IntPtr.Zero, ref p2PaddleRect);

			// render
			SDL.SDL_RenderPresent(_sdlRenderer);

			//DebugState state = new DebugState();
			//InteropEmu.DebugGetState(ref state);
			//Console.WriteLine(state.ClockRate);
		}
	}
}
