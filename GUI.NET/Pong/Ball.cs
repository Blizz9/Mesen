namespace Pong
{
	internal class Ball
   {
		internal const int WIDTH = 8;
		internal const int HEIGHT = 8;

		private const int START_X_POSITION = 128;
		private const int START_Y_POSITION = 129;
		private const int CEILING_Y_POSITION = 72;
		private const int FLOOR_Y_POSITION = 216;

		internal int XPosition;
		internal int YPosition;
		internal BallAngle Angle;

		internal Ball()
	   {
			ResetLocation();
			Angle = BallAngle.DownLeft;
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

		internal void ResetLocation()
	   {
			XPosition = START_X_POSITION;
			YPosition = START_Y_POSITION;
		}

		internal void MoveBall()
	   {
			// bounce off ceiling
			if (YPosition <= CEILING_Y_POSITION)
			{
				if (Angle == BallAngle.UpLeft)
				{
					Angle = BallAngle.DownLeft;
				}
				else if (Angle == BallAngle.UpRight)
				{
					Angle = BallAngle.DownRight;
				}
			}

			// bounce off floor
			if (YPosition >= FLOOR_Y_POSITION)
		   {
				if (Angle == BallAngle.DownLeft)
			   {
					Angle = BallAngle.UpLeft;
			   }
				else if (Angle == BallAngle.DownRight)
			   {
					Angle = BallAngle.UpRight;
			   }
		   }

			// move ball one click
			switch (Angle)
			{
				case BallAngle.UpRight:
					XPosition++;
					YPosition--;
					break;

				case BallAngle.UpLeft:
					XPosition--;
					YPosition--;
					break;

				case BallAngle.DownLeft:
					XPosition--;
					YPosition++;
					break;

				case BallAngle.DownRight:
					XPosition++;
					YPosition++;
					break;
			}
		}
	}
}
