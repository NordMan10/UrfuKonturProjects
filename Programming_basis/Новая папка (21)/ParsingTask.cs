using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class ParsingTask
    {
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            var result = new Dictionary<int, SlideRecord>();
            return lines
                .Skip(1)
                .Select(line => line.Split(';'))
                .Select(item =>
                {
                    if (item.Length < 3) return null;
                    var id = 0;
                    if (!int.TryParse(item[0], out id)) return null;
                    return Tuple.Create(id, new SlideRecord(id, DefineType(item[1]), item[2]));
                })
                .Where(element => element != null)
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        }

        public static SlideType DefineType(string type)
        {
            if (type == "theory") return SlideType.Theory;
            if (type == "quiz") return SlideType.Quiz;
            if (type == "exercise") return SlideType.Exercise;
            return default(SlideType);
        }

        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            return lines
                .Skip(1)
                .Select(line => line.Split(';'))
                .Select(item =>
                {
                    int firstItem;
                    int secondItem;
                    if (!int.TryParse(item[0], out firstItem))
                        throw new FormatException(string.Format("Wrong line [{0}]", item[0]));
                    if (!int.TryParse(item[1], out secondItem))
                        throw new FormatException(string.Format("Wrong line [{0}]", item[1]));
                    return new VisitRecord(
                        firstItem,
                        secondItem,
                        GetDateTime(item[2].Split('-'), item[3].Split(':')),
                        slides[int.Parse(item[1])].SlideType
                    );
                });
        }

        public static DateTime GetDateTime(string[] date, string[] time)
        {
            var intDate = date.Select(dateItem =>
            {
                var parsedDateItem = 0;
                if (int.TryParse(dateItem, out parsedDateItem)) return parsedDateItem;
                throw new FormatException();
            }).ToArray();
            
            var intTime = time.Select(timeItem =>
            {
                var parsedTimeItem = 0;
                if (int.TryParse(timeItem, out parsedTimeItem)) return parsedTimeItem;
                throw new FormatException();
            }).ToArray();
            if (intDate[0] < 0 || intDate[1] > 12 || intDate[2] > 31)
                throw new FormatException(string.Format("Wrong line [{0};{1};{2}-{3}-{4};{5}:{6:d2}:{7:d2}]",
                   1, 2, intDate[0], intDate[1], intDate[2], intTime[0], intTime[1], intTime[2]));
            if (intTime[0] > 24 || intTime[1] > 60 || intTime[2] > 60)
                throw new FormatException(string.Format("Wrong line [{0};{1};{2}-{3:d2}-{4:d2};{5:d2}:{6:d2}:{7:d2}]",
                   1, 2, intDate[0], intDate[1], intDate[2], intTime[0], intTime[1], intTime[2]));
            return new DateTime(intDate[0], intDate[1], intDate[2], intTime[0], intTime[1], intTime[2]);
        }
    }
}