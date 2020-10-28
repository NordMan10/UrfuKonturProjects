using System.Linq;
using System.Collections.Generic;

namespace Recognizer
{
	public static class ThresholdFilterTask
	{
        public static List<double> GetSortedList(double [,] original, int lengthX, int lengthY)
        {
            var newList = new List<double>();
            for (var x = 0; x < lengthX; x++)
            {
                for (var y = 0; y < lengthY; y++)
                {
                    newList.Add(original[x, y]);
                }
            }
            newList.OrderByDescending(i => i);
            return newList;
        }

        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            var lengthX = original.GetLength(0);
            var lengthY = original.GetLength(1);
            var transformPixels = (int)(lengthX * lengthY * whitePixelsFraction);
            var list = GetSortedList(original, lengthX, lengthY);
            var threshold = list[transformPixels - 1];
            if (transformPixels == 0) return original;
            else
            {
                for (var x = 0; x < lengthX; x++)
                    for (var y = 0; y < lengthY; y++)
                    {
                        if (original[x, y] >= threshold) original[x, y] = 1;
                        else original[x, y] = 0;
                    }
                return original;
            }
            
		}
	}
}