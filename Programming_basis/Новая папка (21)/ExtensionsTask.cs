using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public static class ExtensionsTask
	{
		public static double Median(this IEnumerable<double> items)
        {
            var itemsList = new List<double>();
            itemsList = items.OrderBy(item => item).ToList();
            var itemsCount = itemsList.Count;
            if (itemsCount == 0) throw new InvalidOperationException();
            if (itemsCount % 2 != 0)
                return itemsList
                    .Skip(itemsCount / 2)
                    .Take(1)
                    .ToList()[0];
            return itemsList
                .Skip((itemsCount / 2) - 1)
                .Take(2)
                .Average();
        }

        public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
        {
            var prevItem = default(T);
            var isFirst = true;
            foreach (var item in items)
            {
                if (!isFirst)
                    yield return Tuple.Create(prevItem, item);
                isFirst = false;
                prevItem = item;
            }
		}
	}
}