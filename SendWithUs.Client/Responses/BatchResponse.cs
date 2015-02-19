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
    using Newtonsoft.Json.Linq;

    public class BatchResponse : BaseResponse<JArray>, IBatchResponse
    {
        public virtual IList<IResponse> Items { get; set; }

        protected IResponseFactory ResponseFactory { get; set; }

        protected IEnumerable<Type> ResponseSequence { get; set; }

        public BatchResponse(IResponseFactory responseFactory, IEnumerable<Type> responseSequence)
        {
            this.ResponseFactory = responseFactory;
            this.ResponseSequence = responseSequence;
        }

        #region Base class overrides

        protected internal override void Populate(JArray json)
        {
            if (json == null)
            {
                return;
            }

            this.Items = json.Zip(this.ResponseSequence, (jt, rt) => this.BuildResponse(jt as JObject, rt)).ToList();
        }

        protected internal virtual IResponse BuildResponse(JObject wrapper, Type responseType)
        {
            EnsureArgument.NotNull(wrapper, "wrapper");
            EnsureArgument.NotNull(responseType, "responseType");

            var statusCode = (HttpStatusCode)wrapper.Value<int>("status_code");
            var json = wrapper.GetValue("body") as JObject;

            return this.ResponseFactory.Create(responseType, statusCode, json);
        }

        #endregion
    }
}
