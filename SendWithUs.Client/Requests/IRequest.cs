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
    using System;

    /// <summary>
    /// Defines the common interface of objects used to make API requests.
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Gets the HTTP method for the request.
        /// </summary>
        /// <returns>The HTTP method for the request.</returns>
        string GetHttpMethod();

        /// <summary>
        /// Gets the URI path for the request.
        /// </summary>
        /// <returns>The URI path for the request.</returns>
        string GetUriPath();

        /// <summary>
        /// Gets the type of the corresponding response object for the request.
        /// </summary>
        /// <returns>The type of the corresponding response object for the request.</returns>
        Type GetResponseType();

        /// <summary>
        /// Validates the current object.
        /// </summary>
        /// <returns>The current object if valid; throws otherwise.</returns>
        IRequest Validate();

        /// <summary>
        /// Gets a value indicating whether the request object is valid (well-formed).
        /// </summary>
        bool IsValid { get; }
    }
}
