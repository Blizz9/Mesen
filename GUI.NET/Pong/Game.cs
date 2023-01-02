using System;
using SDL2;

namespace Pong
{
	internal class Game
	{
		// main objects
		private GameState _state;
		private Controller _player1Controller;
		private Controller _player2Controller;
		private Ball _ball;
		private Paddle _player1Paddle;
		private Paddle _player2Paddle;

		// SDL
		private IntPtr _sdlRenderer;

		// sprites
		private IntPtr _ballTexture;
		private IntPtr _paddleTexture;
		private IntPtr _wallTexture;

		internal Game()
		{
			_state = GameState.Start;

			_player1Controller = new Controller();
			_player2Controller = new Controller();

			_ball = new Ball();
			_player1Paddle = new Paddle(25);
			_player2Paddle = new Paddle(225);

			// initialize SDL renderer
			SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
			IntPtr sdlWindow = SDL.SDL_CreateWindow("Pong v0.1", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 256, 240, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
			_sdlRenderer = SDL.SDL_CreateRenderer(sdlWindow, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

			// load sprites
			_ballTexture = SDL_image.IMG_LoadTexture(_sdlRenderer, "ball.png");
			_paddleTexture = SDL_image.IMG_LoadTexture(_sdlRenderer, "paddle.png");
			_wallTexture = SDL_image.IMG_LoadTexture(_sdlRenderer, "wall.png");

			// clear screen with black
			SDL.SDL_SetRenderDrawColor(_sdlRenderer, 0, 0, 0, 255);
			SDL.SDL_RenderClear(_sdlRenderer);

			// draw static walls (roof and floor)
			SDL.SDL_Rect wallRect = new SDL.SDL_Rect { x = 0, y = 64, w = 256, h = 8 };
			SDL.SDL_RenderCopy(_sdlRenderer, _wallTexture, IntPtr.Zero, ref wallRect);
			wallRect = new SDL.SDL_Rect { x = 0, y = 224, w = 256, h = 8 };
			SDL.SDL_RenderCopy(_sdlRenderer, _wallTexture, IntPtr.Zero, ref wallRect);

			// draw ball
			SDL.SDL_Rect ballRect = new SDL.SDL_Rect { x = _ball.XPosition, y = _ball.YPosition, w = 8, h = 8 };
			SDL.SDL_RenderCopy(_sdlRenderer, _ballTexture, IntPtr.Zero, ref ballRect);

			// draw paddles
			SDL.SDL_Rect p1PaddleRect = new SDL.SDL_Rect { x = _player1Paddle.XPosition, y = _player1Paddle.YPosition, w = 8, h = 32 };
			SDL.SDL_RenderCopy(_sdlRenderer, _paddleTexture, IntPtr.Zero, ref p1PaddleRect);
			SDL.SDL_Rect p2PaddleRect = new SDL.SDL_Rect { x = _player2Paddle.XPosition, y = _player2Paddle.YPosition, w = 8, h = 32 };
			SDL.SDL_RenderCopy(_sdlRenderer, _paddleTexture, IntPtr.Zero, ref p2PaddleRect);

			// render
			SDL.SDL_RenderPresent(_sdlRenderer);
		}

		internal void Tick(IntPtr p1, IntPtr p2, IntPtr p3, IntPtr p4, IntPtr p5, IntPtr p6, IntPtr p7, IntPtr p8)
		{
			ushort addr = (ushort)p1;
			ushort frame = (ushort)p2;
			ushort player1ControllerState = (ushort)p3;
			ushort player2ControllerState = (ushort)p4;
			ushort ballX = (ushort)p5;
			ushort ballY = (ushort)p6;
			ushort p1PaddleY = (ushort)p7;
			ushort p2PaddleY = (ushort)p8;
			//Console.WriteLine(string.Format("[{0}] {1}: {2}|{3} ({4}, {5}) {6} {7}", addr, frame, player1ControllerState, player2ControllerState, ballX, ballY, p1PaddleY, p2PaddleY));

			_player1Controller.SetState((byte)player1ControllerState);
			_player2Controller.SetState((byte)player2ControllerState);

			gameLoop();

			//DebugState state = new DebugState();
			//InteropEmu.DebugGetState(ref state);
			//Console.WriteLine(state.ClockRate);
		}

		private void gameLoop()
	   {
			switch (_state)
			{
				case GameState.Start:
					gameLoopStart();
					break;

				case GameState.Playing:
					gameLoopPlaying();
					break;

				case GameState.Finished:
					gameLoopFinished();
					break;
			}
		}

		private void gameLoopStart()
	   {
			if (_player1Controller.Start)
			{
				_state = GameState.Playing;
			}
		}

		private void gameLoopPlaying()
		{
			// clear screen with black
			SDL.SDL_SetRenderDrawColor(_sdlRenderer, 0, 0, 0, 255);
			SDL.SDL_RenderClear(_sdlRenderer);

			// draw static walls (roof and floor)
			SDL.SDL_Rect wallRect = new SDL.SDL_Rect { x = 0, y = 64, w = 256, h = 8 };
			SDL.SDL_RenderCopy(_sdlRenderer, _wallTexture, IntPtr.Zero, ref wallRect);
			wallRect = new SDL.SDL_Rect { x = 0, y = 224, w = 256, h = 8 };
			SDL.SDL_RenderCopy(_sdlRenderer, _wallTexture, IntPtr.Zero, ref wallRect);

			// draw player 1 score
			// check if player 1 has won
			// draw player 2 score
			// check if player 2 has won

			// check if the ball has crossed player 1's racquet
			// check if the ball has crossed player 2's racquet

			// move both player's paddles based on input (XOR-ed)
			if (_player2Controller.Up)
			{
				_player2Paddle.MoveUp();
			}
			else if (_player1Controller.Up)
			{
				_player1Paddle.MoveUp();
			}
			else if (_player2Controller.Down)
			{
				_player2Paddle.MoveDown();
			}
			else if (_player1Controller.Down)
			{
				_player1Paddle.MoveDown();
			}

			// draw paddles
			SDL.SDL_Rect p1PaddleRect = new SDL.SDL_Rect { x = _player1Paddle.XPosition, y = _player1Paddle.YPosition, w = 8, h = 32 };
			SDL.SDL_RenderCopy(_sdlRenderer, _paddleTexture, IntPtr.Zero, ref p1PaddleRect);
			SDL.SDL_Rect p2PaddleRect = new SDL.SDL_Rect { x = _player2Paddle.XPosition, y = _player2Paddle.YPosition, w = 8, h = 32 };
			SDL.SDL_RenderCopy(_sdlRenderer, _paddleTexture, IntPtr.Zero, ref p2PaddleRect);

			// check paddle bounces

			_ball.MoveBall();

			// draw ball
			SDL.SDL_Rect ballRect = new SDL.SDL_Rect { x = _ball.XPosition, y = _ball.YPosition, w = 8, h = 8 };
			SDL.SDL_RenderCopy(_sdlRenderer, _ballTexture, IntPtr.Zero, ref ballRect);

			// render
			SDL.SDL_RenderPresent(_sdlRenderer);
		}

		private void gameLoopFinished()
	   {
		}
	}
}
