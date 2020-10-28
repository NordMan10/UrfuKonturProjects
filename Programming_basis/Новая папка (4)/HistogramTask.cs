using System;
using System.Linq;
using System.Collections.Generic;

namespace Names
{
    
    internal static class HistogramOfNames 
    {
        public  static HistogramData GetNameHistogram(NameData[] names, string[] arrayNames) 
        {
            Dictionary<string, double> openWith = new Dictionary<string, double>();
            for (var i = 0; i < 100; i++) openWith[arrayNames[i]] = 0;
            foreach (var name in names) openWith[name.Name]++;
            var values = new double[openWith.Count];
            var j = 0;
            foreach (var name in names) {
                values[j] = openWith[name.Name];
                j++;
            }
            List<string> keyList = new List<string>(openWith.Keys);
            var keys = keyList.ToArray();
            var str = "hjk";
            return new HistogramData(
                string.Format("Рождаемость людей по именам '{0}'", str), keys, values);
        }
    }
}