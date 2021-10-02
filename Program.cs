using System;
using System.IO;
using System.Linq;

namespace _01_BookStatistics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ensure cyrillic characters are output correctly.
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var book = _01_BookStatistics.Properties.Resources.Lev_Tolstoj_Vojna_i_mir_24967;

            BookAnalyzer bookAnalyzer = new BookAnalyzer(book);

            bookAnalyzer.CountWords();

            File.WriteAllText("words.txt", string.Join(Environment.NewLine, bookAnalyzer.GetWords().Distinct().OrderBy(w => w)));
        }
    }
}
