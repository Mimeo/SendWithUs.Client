// Copyright © 2015 Mimeo, Inc. All rights reserved.

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
    using System.Threading.Tasks;

    /// <summary>
    /// Describes the interface of objects used to access to the SendWithUs REST API.
    /// </summary>
    public interface ISendWithUsClient
    {
        /// <summary>
        /// Sends an email message per the given request object.
        /// </summary>
        /// <param name="request">A request object describing the email to be sent.</param>
        /// <returns>A response object.</returns>
        Task<ISendResponse> SendAsync(ISendRequest request);

        /// <summary>
        /// Sends a request to the API
        /// </summary>
        /// <param name="request">A request object describing what you want to do</param>
        /// <returns>A response object.</returns>
        Task<IResponse> SingleAsync(IRequest request);

        /// <summary>
        /// Submits a batch request comprising the given set of request objects.
        /// </summary>
        /// <param name="requests">A set of request objects to be batched.</param>
        /// <returns>A response object.</returns>
        Task<IBatchResponse> BatchAsync(IEnumerable<IRequest> requests);
    }
}
