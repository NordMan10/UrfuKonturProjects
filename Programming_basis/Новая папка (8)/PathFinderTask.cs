using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoutePlanning
{
	public static class PathFinderTask
	{ 
        static double minPathLength = double.MaxValue;
        static int position = 1;
        static List<int> tempListBestOrder = new List<int>() { 0 };
        static List<int> outListBestOrder = new List<int>();

        public static void CloseMethod()
        {
            position -= 1;
            tempListBestOrder.RemoveAt(tempListBestOrder.Count - 1);
        }

        public static void MakeTrivialAction(Point[] checkpoints)
        {
            if (PointExtensions.GetPathLength(checkpoints, tempListBestOrder.ToArray()) < minPathLength)
            {
                outListBestOrder.Clear();
                foreach (var e in tempListBestOrder) outListBestOrder.Add(e);
                minPathLength = PointExtensions.GetPathLength(checkpoints, tempListBestOrder.ToArray());
            }
        }

		public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
		{
            if (tempListBestOrder.Count == checkpoints.Length)
            {
                MakeTrivialAction(checkpoints);
                CloseMethod();
                return outListBestOrder.ToArray();//проблема здесь
            }

            if (PointExtensions.GetPathLength(checkpoints, tempListBestOrder.ToArray()) <= minPathLength)
            {
                for (int i = 1; i < checkpoints.Length; i++)     
                {
                    var index = Array.IndexOf(tempListBestOrder.ToArray(), i, 0, position);
                    if (index == -1)
                    {
                        tempListBestOrder.Add(i);
                        position += 1;
                        FindBestCheckpointsOrder(checkpoints);
                    }
                }
            }

            if (position == 1)
            {
                minPathLength = double.MaxValue;
                return outListBestOrder.ToArray();
            }

            CloseMethod();
            return new int[0];
        }
	}
}