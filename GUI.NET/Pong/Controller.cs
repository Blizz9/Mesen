namespace Pong
{
	internal class Controller
   {
		internal bool A;
		internal bool B;
		internal bool Start;
		internal bool Select;
		internal bool Up;
		internal bool Down;
		internal bool Left;
		internal bool Right;

		internal Controller()
	   {
			A = false;
			B = false;
			Start = false;
			Select = false;
			Up = false;
			Down = false;
			Left = false;
			Right = false;
		}

		internal void SetState(byte rawState)
	   {
			A = (rawState & 0b00000001) != 0;
			B = (rawState & 0b00000010) != 0;
			Start = (rawState & 0b00001000) != 0;
			Select = (rawState & 0b00000100) != 0;
			Up = (rawState & 0b00010000) != 0;
			Down = (rawState & 0b00100000) != 0;
			Left = (rawState & 0b01000000) != 0;
			Right = (rawState & 0b10000000) != 0;
		}
	}
}
