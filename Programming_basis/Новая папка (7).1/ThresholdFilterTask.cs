using System.Linq;
using System.Collections.Generic;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double GetThreshold(double[,] original, int transformPixels)
        {
            var lengthX = original.GetLength(0);
            var lengthY = original.GetLength(1);
            if (transformPixels == 0) return 1000000000.0;
            if (transformPixels == lengthX * lengthY) return -10000.1;
            var newList = new List<double>();
            for (var x = 0; x < lengthX; x++)
            {
                for (var y = 0; y < lengthY; y++)
                {
                    newList.Add(original[x, y]);
                }
            }
            newList.Sort();
            return newList[lengthX * lengthY - transformPixels];
        }

        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            var lengthX = original.GetLength(0);
            var lengthY = original.GetLength(1);
            var transformPixels = (int)(lengthX * lengthY * whitePixelsFraction);
            var threshold = GetThreshold(original, transformPixels);
            for (var x = 0; x < lengthX; x++)
                for (var y = 0; y < lengthY; y++)
                {
                    if (original[x, y] >= threshold) original[x, y] = 1.0;
                    else original[x, y] = 0.0;
                }
            return original;
        }
    }
}