﻿// Copyright © 2014 Mimeo, Inc.

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
    using System.Net;
    using Newtonsoft.Json.Linq;

    public abstract class BaseResponse<T> : IResponse where T : JToken
    {
        protected internal abstract void Populate(IResponseFactory responseFactory, T json);

        #region IResponse members

        public virtual HttpStatusCode StatusCode { get; set; }

        public virtual bool IsSuccessStatusCode
        {
            get
            {
                var codeValue = (int)this.StatusCode;
                return codeValue >= 200 && codeValue <= 299;
            }
        }

        public virtual string ErrorMessage { get; set; }

        public abstract Type InterfaceType { get; }

        public virtual IResponse Initialize(IResponseFactory responseFactory, HttpStatusCode statusCode, JToken json)
        {
            this.StatusCode = statusCode;

            if (this.IsSuccessStatusCode)
            {
                this.Populate(responseFactory, json as T);
            }
            else
            {
                this.SetErrorMessage(json as JValue);
            }

            return this;
        }

        #endregion

        #region Helpers

        protected internal virtual void SetErrorMessage(JValue json)
        {
            this.ErrorMessage = (json != null)
                ? json.Value as String
                : this.StatusCode.ToString();
        }

        #endregion
    }
}
