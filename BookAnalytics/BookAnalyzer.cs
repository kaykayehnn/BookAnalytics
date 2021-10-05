using System.Collections.Generic;
using System.Linq;

namespace _01_BookStatistics
{
    public class BookAnalyzer
    {
        public BookAnalyzer(BookParser bookParser)
        {
            this.BookParser = bookParser;
        }

        protected BookParser BookParser { get; }

        public virtual BookAnalysis Analyze(Book book)
        {
            var words = this.BookParser.Parse(book.Text);

            var wordCount = this.CountWords(words);
            var shortestWord = this.GetShortestWord(words);
            var longestWord = this.GetLongestWord(words);
            var averageWordLength = this.GetAverageWordLength(words);
            var mostCommonWords = this.GetFiveMostCommonWords(words);
            var leastCommonWords = this.GetFiveLeastCommonWords(words);

            return new BookAnalysis(
                wordCount,
                shortestWord,
                longestWord,
                averageWordLength,
                mostCommonWords,
                leastCommonWords
               );
        }

        protected int CountWords(string[] words)
        {
            return words.Length;
        }

        protected string GetShortestWord(string[] words)
        {
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

        protected string GetLongestWord(string[] words)
        {
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

        protected double GetAverageWordLength(string[] words)
        {
            int totalLetters = words.Sum(w => w.Length);

            double averageLength = (double)totalLetters / words.Length;
            
            return averageLength;
        }

        protected string[] GetFiveMostCommonWords(string[] words)
        {
            var wordOccurences = this.GetWordOccurrences(words);

            var mostCommon = wordOccurences
                .OrderByDescending(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .Take(5)
                .ToArray();

            return mostCommon;
        }

        protected string[] GetFiveLeastCommonWords(string[] words)
        {
            var wordOccurences = this.GetWordOccurrences(words);

            var mostCommon = wordOccurences
                .OrderBy(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .Take(5)
                .ToArray();

            return mostCommon;
        }

        private Dictionary<string, int> GetWordOccurrences(string[] words)
        {
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

        public struct BookAnalysis
        {
            public BookAnalysis(int wordCount,
                string shortestWord,
                string longestWord,
                double averageWordLength,
                string[] mostCommonWords,
                string[] leastCommonWords)
            {
                this.WordCount = wordCount;
                this.ShortestWord = shortestWord;
                this.LongestWord = longestWord;
                this.AverageWordLength = averageWordLength;
                this.MostCommonWords = mostCommonWords;
                this.LeastCommonWords = leastCommonWords;
            }

            public int WordCount { get; }
            public string ShortestWord { get; }
            public string LongestWord { get; }
            public double AverageWordLength { get; }
            public string[] MostCommonWords { get; }
            public string[] LeastCommonWords { get; }
        }
    }
}
