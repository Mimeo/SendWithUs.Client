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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class BatchResponse : BaseResponse, IBatchResponse
    {
        public IEnumerable<IResponse> Items { get; set; }

        protected IResponseFactory ResponseFactory { get; set; }

        protected IEnumerable<Type> ResponseTypes { get; set; }

        public BatchResponse(ResponseFactory responseFactory, IEnumerable<Type> responseTypes)
        {
            this.ResponseFactory = responseFactory;
            this.ResponseTypes = responseTypes;
        }

        #region Base class overrides

        protected override void Populate(dynamic responseData)
        {
            if (responseData == null)
            {
                return;
            }

            IEnumerable<dynamic> responses = responseData as IEnumerable<dynamic>;

            if (responses == null)
            {
                throw new ArgumentException("Expected data to be of type IEnumerable<dynamic>.", "responseData");
            }

            this.Items = responses.Zip(this.ResponseTypes, (d, rt) => (IResponse)this.BuildResponse(d, rt));
        }
        
        protected IResponse BuildResponse(dynamic batched, Type responseType)
        {
            if (batched == null)
            {
                throw new ArgumentNullException("batched");
            }

            if (responseType == null)
            {
                throw new ArgumentNullException("responseType");
            }

            var statusCode = (HttpStatusCode)batched.status_code;
            var body = batched.body;

            return this.ResponseFactory.Create(responseType, statusCode, body);
        }

        #endregion
    }
}
