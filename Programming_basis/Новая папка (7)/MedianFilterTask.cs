using System.Drawing;
using System.Collections.Generic;

namespace Recognizer
{
    internal static class MedianFilterTask
    {
        public static double GetMedian(double[,] original, int lengthX, int lengthY, int x, int y)
        {
            var list = new List<double>();
            for (var i = -1; i < 2; i++)
                for (var j = -1; j < 2; j++)
                    if (!(i + x < 0 || i + x > lengthX - 1 || j + y < 0 || j + y > lengthY - 1))
                        list.Add(original[i + x, j + y]);
            list.Sort();
            if (list.Count % 2 != 0) return list[list.Count / 2];
            else return (list[(list.Count / 2) - 1] + list[list.Count / 2]) / 2;
        }

        public static double[,] MedianFilter(double[,] original)
        {
            var lengthX = original.GetLength(0);
            var lengthY = original.GetLength(1);
            var transformedArray = new double[lengthX, lengthY];
            for (var x = 0; x < lengthX; x++)
                for (var y = 0; y < lengthY; y++)
                {
                    transformedArray[x, y] = GetMedian(original, lengthX, lengthY, x, y);
                }
            return transformedArray;
        }
    }
}