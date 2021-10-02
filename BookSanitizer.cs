using System.Text.RegularExpressions;

namespace _01_BookStatistics
{
    /// <summary>
    /// This class is used to remove any formatting, notes and images from book text.
    /// </summary>
    public static class BookSanitizer
    {
        public static string Sanitize(string bookText)
        {
            // Strip editor's notes.
            var notesRegex = new Regex(@"\[[\s\S]+?\]|\*\d+");
            var bookTextWithoutNotes = notesRegex.Replace(bookText, string.Empty);

            // Strip images.
            var imagesRegex = new Regex(@"{img:.+}");
            var withoutImages = imagesRegex.Replace(bookTextWithoutNotes, string.Empty);

            // Strip formatting of italicized words.
            var italicRegex = new Regex(@"_(.+?)_");
            var withoutItalics = italicRegex.Replace(withoutImages, "$1");

            return withoutItalics;
        }
    }
}
