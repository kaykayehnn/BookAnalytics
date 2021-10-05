using System.Threading;

namespace _01_BookStatistics
{
    public class ThreadedBookAnalyzer : BookAnalyzer
    {
        public ThreadedBookAnalyzer(BookParser bookParser):base(bookParser)
        { }

        public override BookAnalysis Analyze(Book book)
        {
            var words = this.BookParser.Parse(book.Text);

            int wordCount = 0;
            string shortestWord = string.Empty;
            string longestWord = string.Empty;
            double averageWordLength = 0;
            string[] mostCommonWords = { };
            string[] leastCommonWords = { };

            var wordCountThread = new Thread(() => wordCount = this.CountWords(words));
            var shortestWordThread = new Thread(() => shortestWord = this.GetShortestWord(words));
            var longestWordThread = new Thread(() => longestWord = this.GetLongestWord(words));
            var averageWordLengthThread = new Thread(() => averageWordLength = this.GetAverageWordLength(words));
            var mostCommonWordsThread = new Thread(() => mostCommonWords = this.GetFiveMostCommonWords(words));
            var leastCommonWordsThread = new Thread(() => leastCommonWords = this.GetFiveLeastCommonWords(words));

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
