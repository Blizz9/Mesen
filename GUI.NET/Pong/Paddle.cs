namespace Pong
{
	internal class Paddle
   {
		internal int XPosition;
		internal int YPosition;

		internal Paddle(int xPosition)
	   {
			XPosition = xPosition;
			YPosition = 120;
	   }

		internal void MoveUp()
	   {
			if (YPosition > 72)
		   {
				YPosition--;
		   }
		}

		internal void MoveDown()
		{
			if (YPosition < 192)
			{
				YPosition++;
			}
		}
	}
}
