namespace Pong
{
	internal class Paddle
   {
		internal const int WIDTH = 8;
		internal const int HEIGHT = 32;

		private const int START_Y_POSITION = 121;
		private const int CEILING_Y_POSITION = 72;
		private const int FLOOR_Y_POSITION = 192;

		internal int XPosition;
		internal int YPosition;

		internal Paddle(int startXPosition)
	   {
			XPosition = startXPosition;
			YPosition = START_Y_POSITION;
	   }

		internal int Top()
		{
			return (YPosition);
		}

		internal int Bottom()
		{
			return (YPosition + HEIGHT);
		}

		internal int Left()
		{
			return (XPosition);
		}

		internal int Right()
		{
			return (XPosition + WIDTH);
		}

		internal void MoveUp()
	   {
			if (YPosition > CEILING_Y_POSITION)
		   {
				YPosition--;
		   }
		}

		internal void MoveDown()
		{
			if (YPosition < FLOOR_Y_POSITION)
			{
				YPosition++;
			}
		}
	}
}
