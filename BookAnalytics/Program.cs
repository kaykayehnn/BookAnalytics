using System;
using System.Diagnostics;
using System.Threading;
using static _01_BookStatistics.BookAnalyzer;

namespace _01_BookStatistics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ensure cyrillic characters are output correctly.
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Book[] books = {
                new Book("Война и мир", Properties.Resources.Lev_Tolstoj_Vojna_i_mir_24967),
                new Book("Клетниците, том 1", Properties.Resources.Victor_Hugo_Kletnitsite_Tom_pyrvi_6101_b),
                new Book("Клетниците, том 2", Properties.Resources.Victor_Hugo_Kletnitsite_Tom_vtori_6102_b),
                new Book("Amazon reviews of magazine subscriptions public dataset", Properties.Resources.Amazon_magazine_subscription_reviews)
            };

            AnalyzeBooksSequential(books);

            AnalyzeBooksSemiParallel(books);

            AnalyzeBooksInParallel(books);
        }

        static BookAnalysis[] AnalyzeBooksSequential(Book[] books)
        {
            Console.WriteLine($"Starting sequential analysis for {books.Length} books...");

            var analyses = new BookAnalysis[books.Length];

            var sw = new Stopwatch();
            var totalSw = new Stopwatch();
            totalSw.Start();

            for (int i = 0; i < books.Length; i++)
            {
                sw.Restart();
                var book = books[i];
                Console.WriteLine($"Starting analysis for {book.Title}...");
                var bookAnalyzer = new BookAnalyzer(new ThreadedBookParser());

                analyses[i] = bookAnalyzer.Analyze(book);
                sw.Stop();
                Console.WriteLine($"Completed analysis for {book.Title} in {sw.ElapsedMilliseconds / 1000f}s");
            }

            totalSw.Stop();
            Console.WriteLine($"Completed analysis of {books.Length} books in {totalSw.ElapsedMilliseconds / 1000f}s");
            Console.WriteLine();

            return analyses;
        }

        static BookAnalysis[] AnalyzeBooksSemiParallel(Book[] books)
        {
            Console.WriteLine($"Starting sequential analysis with parallel tasks for {books.Length} books...");

            var analyses = new BookAnalysis[books.Length];

            var sw = new Stopwatch();
            var totalSw = new Stopwatch();
            totalSw.Start();

            for (int i = 0; i < books.Length; i++)
            {
                sw.Restart();
                var book = books[i];
                Console.WriteLine($"Starting analysis for {book.Title}...");
                // TODO: add other book parsers
                var bookAnalyzer = new ThreadedBookAnalyzer(new ThreadedBookParser());

                analyses[i] = bookAnalyzer.Analyze(book);
                sw.Stop();
                Console.WriteLine($"Completed analysis for {book.Title} in {sw.ElapsedMilliseconds / 1000f}s");
            }

            totalSw.Stop();
            Console.WriteLine($"Completed analysis of {books.Length} books in {totalSw.ElapsedMilliseconds / 1000f}s");
            Console.WriteLine();

            return analyses;
        }

        static BookAnalysis[] AnalyzeBooksInParallel(Book[] books)
        {
            Console.WriteLine($"Starting parallel analysis for {books.Length} books...");

            Thread[] threads = new Thread[books.Length];
            BookAnalysis[] analyses = new BookAnalysis[books.Length];

            var totalSw = new Stopwatch();
            totalSw.Start();

            for (int i = 0; i < books.Length; i++)
            {
                // Copy i here to prevent closure-related problems.
                int ix = i;
                threads[i] = new Thread(() =>
                {
                    var sw = new Stopwatch();
                    
                    var book = books[ix];
                    Console.WriteLine($"Starting analysis for {book.Title}...");
                    
                    sw.Start();
                    
                    var bookAnalyzer = new ThreadedBookAnalyzer(new ThreadedBookParser());
                    analyses[ix] = bookAnalyzer.Analyze(books[ix]);

                    sw.Stop();
                    Console.WriteLine($"Completed analysis for {book.Title} in {sw.ElapsedMilliseconds / 1000f}s");
                });

                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            totalSw.Stop();
            Console.WriteLine($"Completed analysis of {books.Length} books in {totalSw.ElapsedMilliseconds / 1000f}s");
            Console.WriteLine();

            return analyses;
        }

        static void PrintAnalysis(BookAnalysis analysis)
        {
            Console.WriteLine($@"Words in book: {analysis.WordCount}
Shortest word: {analysis.ShortestWord}
Longest word: {analysis.LongestWord}
Average word length: {analysis.AverageWordLength:F1}
Most common words: {string.Join(", ", analysis.MostCommonWords)}
Least common words: {string.Join(", ", analysis.LeastCommonWords)}");
        }
    }
}
