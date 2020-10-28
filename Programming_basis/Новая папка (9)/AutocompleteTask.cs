using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask 
    {
        /// <returns>
        /// Возвращает первую фразу словаря, начинающуюся с prefix.
        /// </returns>
        /// <remarks>
        /// Эта функция уже реализована, она заработает, 
        /// как только вы выполните задачу в файле LeftBorderTask
        /// </remarks>
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];
            
            return null;
        }

        /// <returns>
        /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
        /// элементов словаря, начинающихся с prefix.
        /// </returns>
        /// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var phrasesList = new List<string>();
            var firstItemIndex = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            var trueCount = Math.Min(count, phrases.Count - firstItemIndex);
            if (firstItemIndex == phrases.Count) return new string[0];
            for (var i = 0; i < trueCount; i++)
            {
                if (!phrases[firstItemIndex + i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) break;
                phrasesList.Add(phrases[firstItemIndex + i]);
            }
            return phrasesList.ToArray();
        }

        /// <returns>
        /// Возвращает количество фраз, начинающихся с заданного префикса
        /// </returns>
        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var leftBorderIndex = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            var rightBorderIndex = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);
            return rightBorderIndex - leftBorderIndex - 1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrases()
        {
            var checkList = new List<string>();
            var prefix = "df";
            IReadOnlyList<string> myReadOnlyList = checkList;
            var checkResult = AutocompleteTask.GetTopByPrefix(myReadOnlyList, prefix, 5);
            for (var i = 0; i < checkResult.Length; i++)
                checkList[i] = checkResult[i];
            CollectionAssert.IsEmpty(checkList);
        }

        // ...

        [Test]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
        {
            var checkList = new List<string>() { "ab", "abcd", "cde", "drtu", "jklk", "vhkj" };
            var prefix = "";
            IReadOnlyList<string> myReadOnlyList = checkList;
            var actualCount = AutocompleteTask.GetCountByPrefix(myReadOnlyList, prefix);
            var expectedCount = checkList.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        // ...
    }
}
