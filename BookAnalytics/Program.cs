using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace _01_BookStatistics
{
    class Program
    {
        static void AnalyzeBook(string bookText)
        {
            BookAnalyzer bookAnalyzer = new BookAnalyzer(bookText);

            Console.WriteLine("Analyzing book...");
            var wordCount = bookAnalyzer.CountWords();
            var shortestWord = bookAnalyzer.GetShortestWord();
            var longestWord = bookAnalyzer.GetLongestWord();
            var averageWordLength = bookAnalyzer.GetAverageWordLength();
            var mostCommonWords = bookAnalyzer.GetFiveMostCommonWords();
            var leastCommonWords = bookAnalyzer.GetFiveLeastCommonWords();

            Console.WriteLine($@"Words in book: {wordCount}
Shortest word: {shortestWord}
Longest word: {longestWord}
Average word length: {averageWordLength:F1}
Most common words: {string.Join(", ", mostCommonWords)}
Least common words: {string.Join(", ", leastCommonWords)}");
        }

        static void AnalyzeBookParallel(string bookText)
        {
            BookAnalyzer bookAnalyzer = new BookAnalyzer(bookText);

            Console.WriteLine("Analyzing book...");

            int wordCount = 0;
            string shortestWord = string.Empty;
            string longestWord = string.Empty;
            double averageWordLength = 0;
            string[] mostCommonWords = { };
            string[] leastCommonWords = { };

            var wordCountThread = new Thread(() => wordCount = bookAnalyzer.CountWords());
            var shortestWordThread = new Thread(() => shortestWord = bookAnalyzer.GetShortestWord());
            var longestWordThread = new Thread(() => longestWord = bookAnalyzer.GetLongestWord());
            var averageWordLengthThread = new Thread(() => averageWordLength = bookAnalyzer.GetAverageWordLength());
            var mostCommonWordsThread = new Thread(() => mostCommonWords = bookAnalyzer.GetFiveMostCommonWords());
            var leastCommonWordsThread = new Thread(() => leastCommonWords = bookAnalyzer.GetFiveLeastCommonWords());

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

            Console.WriteLine($@"Words in book: {wordCount}
Shortest word: {shortestWord}
Longest word: {longestWord}
Average word length: {averageWordLength:F1}
Most common words: {string.Join(", ", mostCommonWords)}
Least common words: {string.Join(", ", leastCommonWords)}");
        }

        static void Main(string[] args)
        {
            // Ensure cyrillic characters are output correctly.
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var book = _01_BookStatistics.Properties.Resources.Lev_Tolstoj_Vojna_i_mir_24967;

            Stopwatch sw = new Stopwatch();
            
            sw.Start();
            AnalyzeBookParallel(book);
            sw.Stop();

            Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds / 1000.0}s");
        }
    }
}
