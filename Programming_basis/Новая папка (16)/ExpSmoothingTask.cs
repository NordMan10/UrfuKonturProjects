using System.Collections.Generic;

namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
            var flag = true;
            double expSmoothing = 0;

            foreach (var e in data)
            {
                if (flag)
                {
                    expSmoothing = e.OriginalY;
                    flag = false;
                }
                
                expSmoothing = alpha * e.OriginalY + (1 - alpha) * expSmoothing;
                e.ExpSmoothedY = expSmoothing;
                yield return e;
            }
		}
	}
}