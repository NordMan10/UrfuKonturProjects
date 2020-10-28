using System; 
using System.Drawing; 

namespace Fractals 
{ 
    internal static class DragonFractalTask 
    {
        public static double degree45 = Math.PI * 45 / 180; 
        public static double degree135 = Math.PI * 135 / 180;
        public static double x = 1.0;
        public static double y = 0.0;
		
		static void FirstMethod(Pixels pixels) {  
			var x1 = (x * Math.Cos(degree45) - y * Math.Sin(degree45)) / Math.Sqrt(2); 
			var y1 = (x * Math.Sin(degree45) + y * Math.Cos(degree45)) / Math.Sqrt(2); 
			x = x1;
			y = y1;
            pixels.SetPixel(x, y);
		}

        static void SecondMethod(Pixels pixels) {
			var x1 = (x * Math.Cos(degree135) - y * Math.Sin(degree135)) / Math.Sqrt(2) + 1; 
            var y1 = (x * Math.Sin(degree135) + y * Math.Cos(degree135)) / Math.Sqrt(2); 
            x = x1;
            y = y1;
            pixels.SetPixel(x, y);
		}
		
        public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed) 
        { 
            var random = new Random(seed);
            for (var i = 0; i < iterationsCount; i++)
            { 
                var nextNumber = random.Next(0, 2);
                if (nextNumber == 0)
                {
                    FirstMethod(pixels);
                }
                if (nextNumber == 1)
                {
                    SecondMethod(pixels);
                }
            }
        }
    }
}