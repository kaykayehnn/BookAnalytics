using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_BookStatistics
{
    public class BookAnalyzer
    {
        private string bookText;
        private string[] words;

        public BookAnalyzer(string bookText)
        {
            this.bookText = bookText;
        }

        public int CountWords()
        {
            var words = this.TokenizeBook();

            return words.Length;
        }

        public string GetShortestWord()
        {
            var words = this.TokenizeBook();

            int minIndex = 0;
            for (int i = 0; i < words.Length; i++)
            {
                if(words[i].Length < words[minIndex].Length)
                {
                    minIndex = i;
                }
            }

            return words[minIndex];
        }

        public string GetLongestWord()
        {
            var words = this.TokenizeBook();

            int maxIndex = 0;
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > words[maxIndex].Length)
                {
                    maxIndex = i;
                }
            }

            return words[maxIndex];
        }

        public double GetAverageWordLength()
        {
            var words = this.TokenizeBook();

            int totalLetters = words.Sum(w => w.Length);

            double averageLength = (double)totalLetters / words.Length;
            
            return averageLength;
        }

        public string[] GetFiveMostCommonWords()
        {
            var wordOccurences = this.GetWordOccurrences();

            var mostCommon = wordOccurences
                .OrderByDescending(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .Take(5)
                .ToArray();

            return mostCommon;
        }

        public string[] GetFiveLeastCommonWords()
        {
            var wordOccurences = this.GetWordOccurrences();

            var mostCommon = wordOccurences
                .OrderBy(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .Take(5)
                .ToArray();

            return mostCommon;
        }

        private Dictionary<string, int> GetWordOccurrences()
        {
            var words = this.TokenizeBook();

            var wordOccurences = new Dictionary<string, int>();
            foreach (var word in words)
            {
                // Convert to uppercase to make dictionary case-insensitive.
                // Further reading https://stackoverflow.com/questions/234591/upper-vs-lower-case
                var upper = word.ToUpperInvariant();
                if (!wordOccurences.ContainsKey(upper))
                {
                    wordOccurences[upper] = 0;
                }

                wordOccurences[upper]++;
            }

            return wordOccurences;
        }

        private string[] TokenizeBook()
        {
            if (this.words != null) return this.words;

            var sanitizedText = BookSanitizer.Sanitize(this.bookText);

            var words = WordMatcher.ExtractWords(sanitizedText);
            this.words = words;

            return words;
        }
    }
}
