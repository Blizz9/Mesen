using System;
using SDL2;

namespace Pong
{
	internal class Game
	{
		private const int PLAYER_1_GOAL_X_POSITION = 8;
		private const int PLAYER_2_GOAL_X_POSITION = 240;

		// main objects
		private GameState _state;
		private Controller _player1Controller;
		private Controller _player2Controller;
		private Ball _ball;
		private Paddle _player1Paddle;
		private Paddle _player2Paddle;
		private int _player1Score = 0;
		private int _player2Score = 0;

		// SDL
		private IntPtr _sdlRenderer;

		// sprites
		private IntPtr _ballTexture;
		private IntPtr _paddleTexture;
		private IntPtr _wallTexture;
		private IntPtr _0Texture;
		private IntPtr _1Texture;
		private IntPtr _2Texture;
		private IntPtr _3Texture;
		private IntPtr _4Texture;
		private IntPtr _5Texture;
		private IntPtr _6Texture;
		private IntPtr _7Texture;
		private IntPtr _8Texture;
		private IntPtr _9Texture;

		internal Game()
		{
			_state = GameState.Start;

			_player1Controller = new Controller();
			_player2Controller = new Controller();

			_ball = new Ball();
			_player1Paddle = new Paddle(24);
			_player2Paddle = new Paddle(224);

			// initialize SDL renderer
			SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
			IntPtr sdlWindow = SDL.SDL_CreateWindow("Pong v0.1", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 768, 720, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
			_sdlRenderer = SDL.SDL_CreateRenderer(sdlWindow, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
			SDL.SDL_RenderSetScale(_sdlRenderer, 3, 3);

			// load sprites
			_ballTexture = SDL_image.IMG_LoadTexture(_sdlRenderer, "ball.png");
			_paddleTexture = SDL_image.IMG_LoadTexture(_sdlRenderer, "paddle.png");
			_wallTexture = SDL_image.IMG_LoadTexture(_sdlRenderer, "wall.png");
			_0Texture = SDL_image.IMG_LoadTexture(_sdlRenderer, "0.png");
			_1Texture = SDL_image.IMG_LoadTexture(_sdlRenderer, "1.png");
			_2Texture = SDL_image.IMG_LoadTexture(_sdlRenderer, "2.png");
			_3Texture = SDL_image.IMG_LoadTexture(_sdlRenderer, "3.png");
			_4Texture = SDL_image.IMG_LoadTexture(_sdlRenderer, "4.png");
			_5Texture = SDL_image.IMG_LoadTexture(_sdlRenderer, "5.png");
			_6Texture = SDL_image.IMG_LoadTexture(_sdlRenderer, "6.png");
			_7Texture = SDL_image.IMG_LoadTexture(_sdlRenderer, "7.png");
			_8Texture = SDL_image.IMG_LoadTexture(_sdlRenderer, "8.png");
			_9Texture = SDL_image.IMG_LoadTexture(_sdlRenderer, "9.png");

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
		}

		internal void VBlank(IntPtr p1, IntPtr p2, IntPtr p3, IntPtr p4, IntPtr p5, IntPtr p6, IntPtr p7, IntPtr p8)
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

			//Console.WriteLine(string.Format("({0},{1}) - ({2},{3})", (ballX + 1), (ballY + 1), _ball.XPosition, _ball.YPosition));

			if ((_ball.XPosition != ballX) || (_ball.YPosition != (ballY + 1)))
		   {
				Console.WriteLine("MISMATCH!!!");
		   }

			_player1Controller.SetState((byte)player1ControllerState);
			_player2Controller.SetState((byte)player2ControllerState);

			// render
			SDL.SDL_RenderPresent(_sdlRenderer);

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
			IntPtr scoreTexture = IntPtr.Zero;
			switch (_player1Score)
		   {
				case 0:
					scoreTexture = _0Texture;
					break;
				case 1:
					scoreTexture = _1Texture;
					break;
				case 2:
					scoreTexture = _2Texture;
					break;
				case 3:
					scoreTexture = _3Texture;
					break;
				case 4:
					scoreTexture = _4Texture;
					break;
				case 5:
					scoreTexture = _5Texture;
					break;
				case 6:
					scoreTexture = _6Texture;
					break;
				case 7:
					scoreTexture = _7Texture;
					break;
				case 8:
					scoreTexture = _8Texture;
					break;
				case 9:
					scoreTexture = _9Texture;
					break;
			}
			SDL.SDL_Rect scoreRect = new SDL.SDL_Rect { x = 32, y = 17, w = 24, h = 40 };			
			SDL.SDL_RenderCopy(_sdlRenderer, scoreTexture, IntPtr.Zero, ref scoreRect);
			// check if player 1 has won
			if (_player1Score == 9)
		   {
				_state = GameState.Finished;
		   }

			// draw player 2 score
			switch (_player2Score)
			{
				case 0:
					scoreTexture = _0Texture;
					break;
				case 1:
					scoreTexture = _1Texture;
					break;
				case 2:
					scoreTexture = _2Texture;
					break;
				case 3:
					scoreTexture = _3Texture;
					break;
				case 4:
					scoreTexture = _4Texture;
					break;
				case 5:
					scoreTexture = _5Texture;
					break;
				case 6:
					scoreTexture = _6Texture;
					break;
				case 7:
					scoreTexture = _7Texture;
					break;
				case 8:
					scoreTexture = _8Texture;
					break;
				case 9:
					scoreTexture = _9Texture;
					break;
			}
			scoreRect = new SDL.SDL_Rect { x = 200, y = 17, w = 24, h = 40 };
			SDL.SDL_RenderCopy(_sdlRenderer, scoreTexture, IntPtr.Zero, ref scoreRect);
			// check if player 2 has won
			if (_player2Score == 9)
			{
				_state = GameState.Finished;
			}

			// check if the ball has crossed either players' racquet
			if (_ball.XPosition <= PLAYER_1_GOAL_X_POSITION)
		   {
				_player2Score++;
				_ball.ResetLocation();
			}
			else if (_ball.XPosition >= PLAYER_2_GOAL_X_POSITION)
		   {
				_player1Score++;
				_ball.ResetLocation();
			}

			// move both player's paddles based on input (XOR-ed)
			if (_player2Controller.Up)
			{
				_player2Paddle.MoveUp();
			}
			if (_player1Controller.Up)
			{
				_player1Paddle.MoveUp();
			}
			if (_player2Controller.Down)
			{
				_player2Paddle.MoveDown();
			}
			if (_player1Controller.Down)
			{
				_player1Paddle.MoveDown();
			}

			// draw paddles
			SDL.SDL_Rect p1PaddleRect = new SDL.SDL_Rect { x = _player1Paddle.XPosition, y = _player1Paddle.YPosition, w = 8, h = 32 };
			SDL.SDL_RenderCopy(_sdlRenderer, _paddleTexture, IntPtr.Zero, ref p1PaddleRect);
			SDL.SDL_Rect p2PaddleRect = new SDL.SDL_Rect { x = _player2Paddle.XPosition, y = _player2Paddle.YPosition, w = 8, h = 32 };
			SDL.SDL_RenderCopy(_sdlRenderer, _paddleTexture, IntPtr.Zero, ref p2PaddleRect);

			// check paddle bounces
			if (_ball.Left() < _player1Paddle.Right())
		   {
				if (_ball.Right() >= _player1Paddle.Left())
			   {
					if (_ball.Top() < _player1Paddle.Bottom())
			      {
						if (_ball.Bottom() >= _player1Paddle.Top())
						{
							if (_ball.Angle == BallAngle.DownLeft)
							{
								_ball.Angle = BallAngle.DownRight;
							}
							else if (_ball.Angle == BallAngle.UpLeft)
							{
								_ball.Angle = BallAngle.UpRight;
							}
						}
					}
				}
		   }
			if (_ball.Left() < _player2Paddle.Right())
			{
				if (_ball.Right() >= _player2Paddle.Left())
				{
					if (_ball.Top() < _player2Paddle.Bottom())
					{
						if (_ball.Bottom() >= _player2Paddle.Top())
						{
							if (_ball.Angle == BallAngle.DownRight)
							{
								_ball.Angle = BallAngle.DownLeft;
							}
							else if (_ball.Angle == BallAngle.UpRight)
							{
								_ball.Angle = BallAngle.UpLeft;
							}
						}
					}
				}
			}

			_ball.MoveBall();

			// draw ball
			SDL.SDL_Rect ballRect = new SDL.SDL_Rect { x = _ball.XPosition, y = _ball.YPosition, w = Ball.WIDTH, h = Ball.HEIGHT };
			SDL.SDL_RenderCopy(_sdlRenderer, _ballTexture, IntPtr.Zero, ref ballRect);
		}

		private void gameLoopFinished()
	   {
		}
	}
}
