using System;
using System.Threading;

namespace _01_BookStatistics
{
    class ThreadedStringProcessor
    {
        private const int DefaultThreads = 4;
        private int threadCount;
        public ThreadedStringProcessor(int threads)
        {
            this.threadCount = threads;
        }

        public ThreadedStringProcessor() : this(DefaultThreads)
        {}

        // Split string into n substrings, then process them in parallel and merge the results.
        public T Process<T>(
            string text,
            Func<string, int, int> splitFunc,
            Func<string, T> processFunc,
            Func<T[], T> joinFunc)
        {
            // To divide string in 4 substrings, we need to make 3 "cuts".
            int splits = this.threadCount - 1;
            int[] splitIndexes = new int[splits];

            // TODO: explain lastUsedIndex
            int lastUsedIndex = 0;
            for (int i = 0; i < splits; i++)
            {
                int approximateEndIndex = Math.Max(lastUsedIndex, text.Length / this.threadCount * (i + 1));
                int endIndex = splitFunc(text, approximateEndIndex);
                splitIndexes[i] = endIndex;
                lastUsedIndex = endIndex;
            }

            Thread[] threads = new Thread[this.threadCount];
            T[] results = new T[threads.Length];

            for (int i = 0; i < threads.Length; i++)
            {
                // Prevent closure-related problems
                int iCopy = i;
                
                threads[i] = new Thread(() =>
                {
                    int startIx = iCopy == 0 ? 0 : splitIndexes[iCopy - 1];
                    int endIx = iCopy == splits ? text.Length : splitIndexes[iCopy];
                    string substr = text.Substring(startIx, endIx - startIx);// +1?????

                    results[iCopy] = processFunc(substr);
                });

                threads[i].Start();
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            T finalResult = joinFunc(results);
            return finalResult;
        }
    }
}
