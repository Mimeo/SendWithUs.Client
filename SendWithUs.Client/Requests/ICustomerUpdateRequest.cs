// Copyright © 2014 Mimeo, Inc.

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
    using System.Collections.Generic;

    /// <summary>
    /// Describes the interface of objects used to make API requests to add/update customers.
    /// </summary>
    /// <remarks>This interface roughly corresponds to the JSON object used with the /customers route
    /// in the REST API, with some properties flattened or renamed.</remarks>
    public interface ICustomerUpdateRequest : IRequest
    {
        /// <summary>
        /// Gets the email address of the customer.
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Gets the locale of the customer.
        /// </summary>
        string Locale { get; }

        /// <summary>
        /// Gets the data to use for creating the customer.
        /// </summary>
        /// <remarks>Corresponds to the "data" property in the SendWithUs API.</remarks>
        IEnumerable<KeyValuePair<string, object>> Data { get; }

        /// <summary>
        /// Gets the groups that this customer should belong to
        /// </summary>
        IEnumerable<string> Groups { get; }
    }
}
