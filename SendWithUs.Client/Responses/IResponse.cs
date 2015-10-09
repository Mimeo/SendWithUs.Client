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
    using Newtonsoft.Json.Linq;
    using System;
    using System.Net;

    public interface IResponse
    {
        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets a value indicating whether the status code represents success.
        /// </summary>
        bool IsSuccessStatusCode { get; }

        /// <summary>
        /// Gets the error message when the status code represents failure; null otherwise.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Gets the primary interface implemented by the response.
        /// </summary>
        Type InterfaceType { get; }

        /// <summary>
        /// Initializes a freshly created response.
        /// </summary>
        /// <param name="responseFactory">A response factory that may be used to create contained objects.</param>
        /// <param name="statusCode">An HTTP status code returned by the REST API.</param>
        /// <param name="json">A JSON payload returned by the REST API.</param>
        /// <returns></returns>
        IResponse Initialize(IResponseFactory responseFactory, HttpStatusCode statusCode, JToken json);
    }
}
