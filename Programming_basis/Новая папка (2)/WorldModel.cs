using System;
using System.Windows.Forms;

namespace GravityBalls
{
	public class WorldModel
	{
		public double BallX;
		public double BallY;
		public double BallRadius;
		public double WorldWidth;
		public double WorldHeight;
        public double SpeedX = 500; 
        public double SpeedY = 700;
        public double k = 0.99;
        public double cursorX;
        public double cursorY;




        public void SimulateTimeframe(double dt)
        {
            SpeedX *= k;
            SpeedY = SpeedY * k + 13; 
            BallY = Math.Min(BallY + SpeedY * dt, WorldHeight - BallRadius);
            BallX = Math.Min(BallX + SpeedX * dt, WorldWidth - BallRadius);
            if ((WorldWidth - BallX) == BallRadius) SpeedX *= -1;
            if ((WorldHeight - BallY) == BallRadius) SpeedY *= -1;
            if (BallY <= BallRadius + 1) SpeedY *= -1;
            if (BallX <= BallRadius) SpeedX *= -1;
        }
	}
}