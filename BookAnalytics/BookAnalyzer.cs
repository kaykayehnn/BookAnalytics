using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_BookStatistics
{
    class BookAnalyzer
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

            throw new NotImplementedException();
        }

        public string GetLongestWord()
        {
            var words = this.TokenizeBook();

            throw new NotImplementedException();
        }

        public double GetAverageWordLength()
        {
            var words = this.TokenizeBook();

            throw new NotImplementedException();
        }

        public string[] GetMostCommonWords()
        {
            var words = this.TokenizeBook();

            throw new NotImplementedException();
        }

        public string[] GetLeastCommonWords()
        {
            var words = this.TokenizeBook();

            throw new NotImplementedException();
        }

        private string[] TokenizeBook()
        {
            if (this.words != null) return this.words;

            var sanitizedText = BookSanitizer.Sanitize(this.bookText);

            var words = WordMatcher.ExtractWords(sanitizedText);
            this.words = words;

            return words;
        }

        public string[] GetWords()
        {
            return this.TokenizeBook();
        }

    }
}
