using System;
using System.Linq;

namespace _01_BookStatistics
{
    class ThreadedBookParser : BookParser
    {
        private ThreadedStringProcessor stringProcessor;
        public ThreadedBookParser() : base()
        {
            this.stringProcessor = new ThreadedStringProcessor();
        }
        public ThreadedBookParser(int threadCount) : base()
        {
            this.stringProcessor = new ThreadedStringProcessor(threadCount);
        }

        public override string[] Parse(string text)
        {
            return stringProcessor.Process(
                text,
                GetNextNewLine,
                base.Parse,
                MergeResults);
        }

        private int GetNextNewLine(string text, int startIndex)
        {
            return text.IndexOf('\n', startIndex) + 1; // Include new line in chunk
        }

        private string[] MergeResults(string[][] wordArrays)
        {
            int totalWords = wordArrays.Sum(arr => arr.Length);
            string[] allWords = new string[totalWords];

            int currentIndex = 0;
            for (int i = 0; i < wordArrays.Length; i++)
            {
                var currentWords = wordArrays[i];
                Array.Copy(currentWords, 0, allWords, currentIndex, currentWords.Length);
                
                currentIndex += currentWords.Length;
            }

            return allWords;
        }
    }
}
