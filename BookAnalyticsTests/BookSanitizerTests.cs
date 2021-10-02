using _01_BookStatistics;
using NUnit.Framework;


namespace BookStatisticsTests
{
    public class BookSanitizerTests
    {
        [Test]
        public void StripsEditorNotes()
        {
            string book = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit.
[This is a note. It should be removed]";

            string expected = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit.
";
            string actual = BookSanitizer.Sanitize(book);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void StripsInlineEditorNotes()
        {
            string book = @"
Lorem*1 ipsum dolor sit amet, consectetur adipiscing elit.
";

            string expected = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit.
";

            string actual = BookSanitizer.Sanitize(book);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void StripsImages()
        {
            string book = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit.
{img:lorem.png}";

            string expected = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit.
";

            string actual = BookSanitizer.Sanitize(book);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DeformatsItalicizedWords()
        {
            string book = @"
Lorem _ipsum_ dolor sit amet, consectetur adipiscing elit.
";

            string expected = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit.
";

            string actual = BookSanitizer.Sanitize(book);
            Assert.AreEqual(expected, actual);
        }
    }
}