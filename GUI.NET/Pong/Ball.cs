namespace Pong
{
	internal class Ball
   {
		private const int START_X_POSITION = 128;
		private const int START_Y_POSITION = 128;
		private const int CEILING_Y_POSITION = 72;
		private const int FLOOR_Y_POSITION = 216;
		private const int GOAL_1_X_POSITION = 7;
		private const int GOAL_2_X_POSITION = 249;

		internal int XPosition;
		internal int YPosition;
		private BallAngle _angle;

		internal Ball()
	   {
			resetBallLocation();
			_angle = BallAngle.DownLeft;
	   }

		private void resetBallLocation()
	   {
			XPosition = START_X_POSITION;
			YPosition = START_Y_POSITION;
		}

		internal void MoveBall()
	   {
			// check if ball enters a goal
			if (XPosition <= GOAL_1_X_POSITION)
		   {
				resetBallLocation();
			}
			else if (XPosition >= GOAL_2_X_POSITION)
		   {
				resetBallLocation();
			}

			// bounce off ceiling
			if (YPosition <= CEILING_Y_POSITION)
			{
				if (_angle == BallAngle.UpLeft)
				{
					_angle = BallAngle.DownLeft;
				}
				else if (_angle == BallAngle.UpRight)
				{
					_angle = BallAngle.DownRight;
				}
			}

			// bounce off floor
			if (YPosition >= FLOOR_Y_POSITION)
		   {
				if (_angle == BallAngle.DownLeft)
			   {
					_angle = BallAngle.UpLeft;
			   }
				else if (_angle == BallAngle.DownRight)
			   {
					_angle = BallAngle.UpRight;
			   }
		   }

			// move ball one click
			switch (_angle)
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
