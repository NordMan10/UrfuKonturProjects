using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            var continues = new Dictionary<string, int>();//возможные продолжения и их количество
            var forBigram = new Dictionary<string, Dictionary<string, int>>();//начала биграмм, далее их продолжения и количество повторений 
                                                                              //этих продолжений для каждого начала
            foreach (var sentence in text)
                for (var i = 0; i < sentence.Count - 1; i++)

            //считаем количество продолжений для каждого слова(для биграмм)
            foreach (var sentence in text)
                for (var i = 0; i < sentence.Count - 1; i++)
                    forBigram[sentence[i]] = continues[sentence[i + 1]]++;

            //сортировка словаря
            var sortedDictionary = from e in forBigram
                                   orderby e.Value descending, e.Key ascending
                                   select e;

            return result;
        }
   }
}