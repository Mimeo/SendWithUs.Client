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
        internal static class PropertyNames
        {
            public const string StatusCode = "status_code";
            public const string Body = "body";
        }

        public virtual IEnumerable<IResponse> Items { get; set; }

        protected internal virtual JArray RawItems { get; set; }

        #region Base class overrides

        protected internal override void Populate(JArray json)
        {
            this.RawItems = json;
        }

        #endregion

        public BatchResponse Inflate(IEnumerable<Type> responseSequence, IResponseFactory responseFactory)
        {
            if (this.RawItems == null)
            {
                throw new InvalidOperationException("Cannot inflate; the value of RawItems is null.");
            }

            // Force enumeration of the Items value so we can discard the RawItems.
            this.Items = this.RawItems.Zip(responseSequence, (jt, rt) => this.BuildResponse(jt as JObject, rt, responseFactory)).ToList();
            this.RawItems = null;
            return this;
        }

        protected internal virtual IResponse BuildResponse(JObject wrapper, Type responseType, IResponseFactory responseFactory)
        {
            EnsureArgument.NotNull(wrapper, nameof(wrapper));
            EnsureArgument.NotNull(responseType, nameof(responseType));
            EnsureArgument.NotNull(responseFactory, nameof(responseFactory));

            var statusCode = (HttpStatusCode)wrapper.Value<int>(PropertyNames.StatusCode);
            var json = wrapper.GetValue(PropertyNames.Body) as JObject;

            return responseFactory.CreateResponse(responseType, statusCode, json);
        }
    }
}
