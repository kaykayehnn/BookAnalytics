using System.Linq;
using System.Text.RegularExpressions;

namespace _01_BookStatistics
{
    public static class WordMatcher
    {
        public static string[] ExtractWords(string text)
        {
            // This regex attempts to match any words and wordlike combinations.
            // Here is a detailed breakdown of what each group does and why:
            //
            //   [+-]?\d+[\p{L}’-]*|[\p{L}\d’-]+|[„“][\p{L}\d’-]+[„“]-[\p{L}’-]+
            //
            //   [+-]?\d+[\p{L}’-]*
            //   This group matches numbers and numberlike words.
            //   
            //   [+-]?      : Optional sign in front of the number
            //   \d+        : The digits constituting the number
            //   [\p{L}’-]* : Any letters following the number (e.g. 42nd, 42-nd)
            //
            //
            //   [\p{L}\d’-]+
            //   This group matches all common words.
            //   
            //   A word can be any combination of:
            //   \p{L} : Letter characters, including all languages and cultures.
            //           Source https://stackoverflow.com/a/48902765/6317168
            //   \d    : Digits (e.g. C4, H2O)
            //   ’     : Apostrophe for words which include it (e.g. t’aime in French)
            //   -     : Hyphen for complex words (e.g. au-revoir)
            //
            //
            //   [„“][\p{L}\d’-][„“]-[\p{L}’-]+
            //   This group matches any combination of words in inverted commas, directly preceding
            //   an article. This construct may only apply to Bulgarian phrases, for example
            //   "„о“-то".
            //
            //   [„“]       : Open inverted commas. Allow both opening and closing inverted commas
            //                to be more flexible.
            //   [\p{L}\d’-] : The word part between the inv commas. See previous group.
            //   [„“]       : Close inverted commas.
            //   -          : Hyphen for the article
            //   [\p{L}’-]+ : The article.
            //
            //
            string pattern = @"[+-]?\d+[\p{L}’-]*|[\p{L}\d’-]+|[„“][\p{L}\d’-]+[„“]-[\p{L}’-]+";
            var wordRegex = new Regex(pattern);

            var words = wordRegex.Matches(text)
                .Select(m => m.Value)
                .ToArray();

            return words;
        }
    }
}
