using System;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            var arrayForLabelX = new string[30];
            for (var i = 0; i < arrayForLabelX.Length; i++) arrayForLabelX[i] = (i + 2).ToString();
            var arrayForLabelY = new string[12];
            for (var i = 0; i < arrayForLabelY.Length; i++) arrayForLabelY[i] = (i + 1).ToString();
            var mapDataArray = new double[30,12];
            foreach (var name in names) {
                if (name.BirthDate.Day != 1)
                    mapDataArray[name.BirthDate.Day - 2, name.BirthDate.Month - 1]++;
            }
            return new HeatmapData(
                "Пример карты интенсивностей",
                mapDataArray, 
                arrayForLabelX, 
                arrayForLabelY);
        }
    }
}