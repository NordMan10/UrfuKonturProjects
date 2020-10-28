using System;
using System.Collections.Generic;
using System.Linq;

namespace yield
{
	public static class MovingMaxTask
	{
        public static void CheckMaxValue(LinkedList<double> potMax, double initValue)
        {
                potMax.Clear();
                potMax.AddLast(initValue);
        }

        public static void CheckFromEnd(LinkedList<double> potMax, double initValue)
        {
            for (var i = 0; i < potMax.Count; i++)
            {
                if (initValue > potMax.Last.Value)
                {
                    potMax.RemoveLast();
                    i--;
                }
                else
                {
                    potMax.AddLast(initValue);
                    break;
                }
            }
        }

        public static void SetPotentialMax(LinkedList<double> potMax, double initValue)
        {
            if (potMax.Count > 0)
            {
                if (initValue > potMax.First()) CheckMaxValue(potMax, initValue);
                else CheckFromEnd(potMax, initValue);
            }
            else potMax.AddLast(initValue);
        }

        public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
		{
            var window = new Queue<double>();
            var potMax = new LinkedList<double>();

            foreach (var point in data)
            {
                window.Enqueue(point.OriginalY);

                if (window.Count > windowWidth)
                {
                    if (window.Dequeue() == potMax.First())
                    {
                        potMax.RemoveFirst();
                    }
                }

                SetPotentialMax(potMax, point.OriginalY);

                point.MaxY = potMax.First.Value;
                yield return point;
            }
		}
	}
}