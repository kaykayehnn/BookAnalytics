using System.Threading;

namespace _01_BookStatistics
{
    public class ThreadedBookAnalyzer : BookAnalyzer
    {
        public ThreadedBookAnalyzer(Book book):base(book)
        {
            // Pre-tokenize book to utilize parallelism
            this.TokenizeBook();
        }

        public override BookAnalysis Analyze()
        {
            int wordCount = 0;
            string shortestWord = string.Empty;
            string longestWord = string.Empty;
            double averageWordLength = 0;
            string[] mostCommonWords = { };
            string[] leastCommonWords = { };

            var wordCountThread = new Thread(() => wordCount = this.CountWords());
            var shortestWordThread = new Thread(() => shortestWord = this.GetShortestWord());
            var longestWordThread = new Thread(() => longestWord = this.GetLongestWord());
            var averageWordLengthThread = new Thread(() => averageWordLength = this.GetAverageWordLength());
            var mostCommonWordsThread = new Thread(() => mostCommonWords = this.GetFiveMostCommonWords());
            var leastCommonWordsThread = new Thread(() => leastCommonWords = this.GetFiveLeastCommonWords());

            Thread[] threads =
            {
                wordCountThread, shortestWordThread, longestWordThread,
                averageWordLengthThread, mostCommonWordsThread, leastCommonWordsThread
            };

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return new BookAnalysis(
                wordCount,
                shortestWord,
                longestWord,
                averageWordLength,
                mostCommonWords,
                leastCommonWords
               );
        }
    }
}
