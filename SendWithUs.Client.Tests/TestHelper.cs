// Copyright © 2015 Mimeo, Inc.

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SendWithUs.Client.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class TestHelper
    {
        // NOTE: Use a fixed seed to aid in debugging.
        private static readonly Random RandomNumber = new Random();

        public static IEnumerable<T> Generate<T>(int count, Func<int, T> itemGenerator)
        {
            if (count < 0)
            {
                throw new ArgumentException("Count must be non-negative.");
            }

            return Enumerable.Range(0, count).Select(i => itemGenerator(i));
        }

        public static IEnumerable<T> Generate<T>(int min, int max, Func<int, T> itemGenerator)
        {
            if (max < min)
            {
                throw new ArgumentException("Max must be greater than or equal to min.");
            }

            return Enumerable.Range(min, max - min + 1).Select(i => itemGenerator(i));
        }

        public static Exception CaptureException(Action action)
        {
            try
            {
                action();
            }
            catch (Exception captured)
            {
                return captured;
            }

            return null;
        }

        public static Exception CaptureException(Func<Task> asyncAction)
        {
            try
            {
                asyncAction().Wait();
            }
            catch (AggregateException captured)
            {
                return captured.InnerException;
            }

            return null;
        }

        public static int GetRandomInteger(int min, int max)
        {
            return RandomNumber.Next(min, max);
        }

        public static string GetUniqueId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static IDictionary<string, string> GetRandomDictionary()
        {
            var itemCount = GetRandomInteger(1, 10);
            return Generate(itemCount, i => GetUniqueId()).ToDictionary(k => k, k => GetUniqueId());
        }
    }
}
