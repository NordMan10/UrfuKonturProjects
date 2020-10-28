using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static string[] sentencesSeparator(string text) {
            return text.Split('.', '!', '?', ';', ':', '(', ')');
        }

        public static List<string> wordSeparator(string [] sentences) {
            var words = new List<string>();
            foreach (var sentence in sentences) {
                var word = new StringBuilder();
                foreach (var ch in sentence) {
                    if (char.IsLetter(ch) || ch == '\'') {
                        word.Append(char.ToLower(ch));
                    }
                    else if (word.Length > 0) {
                        words.Add(word.ToString());
                        word.Length = 0;
                    }
                }
            }
            return words;
        }

        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            var sentences = sentencesSeparator(text);
            for (var i = 0; i < sentences.Length; i++) {
                sentencesList.Add(wordSeparator(sentences));
            }
            return sentencesList;
        }
    }
}