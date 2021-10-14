using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ConsoleTables;
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
                new Book("Клетниците, том 2", Properties.Resources.Victor_Hugo_Kletnitsite_Tom_vtori_6102_b)
                // This eats a lot of ram.
                //new Book("Amazon reviews of magazine subscriptions public dataset", Properties.Resources.Amazon_magazine_subscription_reviews)
            };

            const int TaskRunners = 3;

            var timings = new long[TaskRunners][];
            var analyses = new BookAnalysis[TaskRunners][];

            var timingLabels = new[]
            {
                "Sequential",
                "Parallel",
                "Parallel v2"
            };


            analyses[0] = AnalyzeBooksSequential(books, out timings[0]);

            analyses[1] = AnalyzeBooksSemiParallel(books, out timings[1]);

            analyses[2] = AnalyzeBooksInParallel(books, out timings[2]);

            PrintTimings(books, timings, timingLabels);

            //PrintAnalyses(books, analyses[0]);
        }


        static BookAnalysis[] AnalyzeBooksSequential(Book[] books, out long[] timings)
        {
            Console.WriteLine($"Starting sequential analysis for {books.Length} books...");

            var analyses = new BookAnalysis[books.Length];

            var sw = new Stopwatch();
            var totalSw = new Stopwatch();

            timings = new long[books.Length + 1];

            totalSw.Start();
            for (int i = 0; i < books.Length; i++)
            {
                sw.Restart();
                var book = books[i];
                Console.WriteLine($"Starting analysis for {book.Title}...");
                var bookAnalyzer = new BookAnalyzer(new ThreadedBookParser());

                analyses[i] = bookAnalyzer.Analyze(book);
                sw.Stop();

                var elapsed = sw.ElapsedMilliseconds;
                timings[i] = elapsed;
                Console.WriteLine($"Completed analysis for {book.Title} in {elapsed / 1000f}s");
            }

            totalSw.Stop();
            var totalElapsed = totalSw.ElapsedMilliseconds;
            timings[timings.Length - 1] = totalElapsed;

            Console.WriteLine($"Completed analysis of {books.Length} books in {totalElapsed / 1000f}s");
            Console.WriteLine();

            return analyses;
        }

        static BookAnalysis[] AnalyzeBooksSemiParallel(Book[] books, out long[] timings)
        {
            Console.WriteLine($"Starting sequential analysis with parallel tasks for {books.Length} books...");

            var analyses = new BookAnalysis[books.Length];

            var sw = new Stopwatch();
            var totalSw = new Stopwatch();

            timings = new long[books.Length + 1];

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

                var elapsed = sw.ElapsedMilliseconds;
                timings[i] = elapsed;
                Console.WriteLine($"Completed analysis for {book.Title} in {elapsed / 1000f}s");
            }

            totalSw.Stop();
            var totalElapsed = totalSw.ElapsedMilliseconds;
            timings[timings.Length - 1] = totalElapsed;

            Console.WriteLine($"Completed analysis of {books.Length} books in {totalElapsed / 1000f}s");
            Console.WriteLine();

            return analyses;
        }

        static BookAnalysis[] AnalyzeBooksInParallel(Book[] books, out long[] timings)
        {
            Console.WriteLine($"Starting parallel analysis for {books.Length} books...");

            Thread[] threads = new Thread[books.Length];
            BookAnalysis[] analyses = new BookAnalysis[books.Length];

            // C# doesn't allow using out params in anonymous functions, so we
            // use this variable as a proxy.
            var localTimings = new long[books.Length + 1];
            timings = localTimings;

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
                    var elapsed = sw.ElapsedMilliseconds;
                    localTimings[ix] = elapsed;

                    Console.WriteLine($"Completed analysis for {book.Title} in {elapsed / 1000f}s");
                });

                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            totalSw.Stop();

            var total = totalSw.ElapsedMilliseconds;
            timings[timings.Length - 1] = total;

            Console.WriteLine($"Completed analysis of {books.Length} books in {total / 1000f}s");
            Console.WriteLine();

            return analyses;
        }

        static void PrintTimings(Book[] books, long[][] timings, string[] timingLabels)
        {
            var table = new ConsoleTable();

            table.Options.EnableCount = false;

            table.Columns.Add("Algorithm / Book");
            foreach (var book in books)
            {
                table.Columns.Add(book.Title);
            }

            table.Columns.Add("Total");

            for (int i = 0; i < timings.Length; i++)
            {
                var row = new object[table.Columns.Count];
                row[0] = timingLabels[i];

                for (int j = 0; j < timings[i].Length; j++)
                {
                    row[j + 1] = timings[i][j];
                }

                table.AddRow(row);
            }

            table.Write();
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

        private static void PrintAnalyses(Book[] books, BookAnalysis[] analyses)
        {
            for (int i = 0; i < books.Length; i++)
            {
                Console.WriteLine($"Analysis of {books[i].Title}:");
                PrintAnalysis(analyses[i]);
                Console.WriteLine();
            }
        }
    }
}
