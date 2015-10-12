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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    [JsonConverter(typeof(BatchRequestConverter))]
    internal class BatchRequest : IEnumerable<IRequest>, IRequest
    {
        protected IEnumerable<IRequest> Items { get; set; }

        public BatchRequest(IEnumerable<IRequest> items)
        {
            this.Items = items ?? Enumerable.Empty<IRequest>();
        }

        #region IEnumerable<IRequest> Members

        public virtual IEnumerator<IRequest> GetEnumerator() => this.Items.GetEnumerator();

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region IRequest Members

        public virtual string GetHttpMethod() => "POST";

        public virtual string GetUriPath() => "/api/v1/batch";

        public virtual Type GetResponseType()
        {
            // Throw to ensure that a batch response is never nested inside a batch response.
            throw new NotSupportedException();
        }

        public IRequest Validate()
        {
            // Throw to ensure that a batch request is never nested inside a batch request.
            throw new NotSupportedException();
        }

        public bool IsValid
        {
            get { throw new NotSupportedException(); }
        }
        
        #endregion
    }
}
