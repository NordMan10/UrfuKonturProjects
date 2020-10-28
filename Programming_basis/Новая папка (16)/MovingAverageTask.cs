using System.Collections.Generic;

namespace yield
{
	public static class MovingAverageTask
	{
		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
            double sum = 0;
            var queue = new Queue<double>();
            foreach (var e in data)
            {
                sum += e.OriginalY;
                queue.Enqueue(e.OriginalY);
                if (queue.Count > windowWidth)
                {
                    sum -= queue.Dequeue();
                }
                e.AvgSmoothedY = sum / queue.Count;
                yield return e;
            }
		}
	}
}