namespace _01_BookStatistics
{
    public class BookParser
    {
        public BookParser()
        { }

        public virtual string[] Parse(string bookText)
        {
            string sanitized = BookSanitizer.Sanitize(bookText);
            string[] words = WordMatcher.ExtractWords(sanitized);

            return words;
        }
    }
}
