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

namespace SendWithUs.Client
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public static class EnsureArgument
    {
        public static void NotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void NotNullOrEmpty(string value, string paramName)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Argument is null or empty.", paramName);
            }
        }

        public static void NotNullOrEmpty<TItem>(IEnumerable<TItem> value, string paramName, bool allowNullItems)
        {
            if (value == null || value.Count() == 0 || (!allowNullItems && value.Any(i => i == null)))
            {
                var message = allowNullItems ? "Argument is null or empty." : "Argument is null, empty, or contains a null item.";
                throw new ArgumentException(message, paramName);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Language", "CSE0003:Use expression-bodied members", Justification = "Complex expression")]
        public static T Of<T>(object value, string paramName) where T : class
        {
            return EnsureArgument.Of<T>(value, paramName, String.Format(CultureInfo.InvariantCulture, 
                "The value of '{0}' must be of type '{1}'.", paramName, typeof(T)));
        }

        public static T Of<T>(object value, string paramName, string errorMessage) where T : class
        {
            var typedValue = value as T;

            if (typedValue == null)
            {
                throw new ArgumentException(errorMessage, paramName);
            }

            return typedValue;
        }
    }
}
