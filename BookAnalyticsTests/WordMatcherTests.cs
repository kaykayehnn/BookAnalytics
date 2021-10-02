using _01_BookStatistics;
using NUnit.Framework;


namespace BookStatisticsTests
{
    public class WordMatcherTests
    {
        [Test]
        public void MatchesEnglishWords()
        {
            string text = "word";

            string[] expected = { "word" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesCyrillicWords()
        {
            string text = "боза";

            string[] expected = { "боза" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesAccentedWords()
        {
            string text = "Düsseldorf";

            string[] expected = { "Düsseldorf" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesChineseWords()
        {
            string text = "北京市";

            string[] expected = { "北京市" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesArabicWords()
        {
            string text = "إسرائيل";

            string[] expected = { "إسرائيل" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesComplexWords()
        {
            string text = "voyez-vous";

            string[] expected = { "voyez-vous" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesWordsWithApostrophes()
        {
            string text = "s’accuse";

            string[] expected = { "s’accuse" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesNumbers()
        {
            string text = "42";

            string[] expected = { "42" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesWordedNumbers()
        {
            string text = "42nd";

            string[] expected = { "42nd" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesWordedNumbersCyrillic()
        {
            string text = "42-ри";

            string[] expected = { "42-ри" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesNegativeNumbers()
        {
            string text = "-42";

            string[] expected = { "-42" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesOnlyWordCharacters()
        {
            string text = "42°";

            string[] expected = { "42" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Ignore("Too difficult to fix")]
        // This tests fails. It would be really difficult to develop a proper regex for this, but
        // it is left for future reference.
        public void MatchesNumberPeriods()
        {
            string text = "2011-2012";

            string[] expected = { "2011", "2012" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesWordsWithNumbers()
        {
            string text = "C4";

            string[] expected = { "C4" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesWordsInInvertedCommas()
        {
            string text = "„о“-то";

            string[] expected = { "„о“-то" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesSentencesCorrectly()
        {
            string text = "The 42 box is s’accuse.";

            string[] expected = { "The", "42", "box", "is", "s’accuse" };
            string[] actual = WordMatcher.ExtractWords(text);

            Assert.AreEqual(expected, actual);
        }
    }
}