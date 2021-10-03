using System.Collections.Generic;
using System.Linq;

namespace _01_BookStatistics
{
    public class BookAnalyzer
    {
        private Book book;
        private string[] words;

        public BookAnalyzer(Book book)
        {
            this.book = book;
        }

        public virtual BookAnalysis Analyze()
        {
            var wordCount = this.CountWords();
            var shortestWord = this.GetShortestWord();
            var longestWord = this.GetLongestWord();
            var averageWordLength = this.GetAverageWordLength();
            var mostCommonWords = this.GetFiveMostCommonWords();
            var leastCommonWords = this.GetFiveLeastCommonWords();

            return new BookAnalysis(
                wordCount,
                shortestWord,
                longestWord,
                averageWordLength,
                mostCommonWords,
                leastCommonWords
               );
        }

        protected int CountWords()
        {
            var words = this.TokenizeBook();

            return words.Length;
        }

        protected string GetShortestWord()
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

        protected string GetLongestWord()
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

        protected double GetAverageWordLength()
        {
            var words = this.TokenizeBook();

            int totalLetters = words.Sum(w => w.Length);

            double averageLength = (double)totalLetters / words.Length;
            
            return averageLength;
        }

        protected string[] GetFiveMostCommonWords()
        {
            var wordOccurences = this.GetWordOccurrences();

            var mostCommon = wordOccurences
                .OrderByDescending(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .Take(5)
                .ToArray();

            return mostCommon;
        }

        protected string[] GetFiveLeastCommonWords()
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

        // TODO: think more about this
        protected string[] TokenizeBook()
        {
            if (this.words != null) return this.words;

            var sanitizedText = BookSanitizer.Sanitize(this.book.Text);

            var words = WordMatcher.ExtractWords(sanitizedText);
            this.words = words;

            return words;
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
