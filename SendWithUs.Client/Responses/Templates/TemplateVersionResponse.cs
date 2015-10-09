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
    using Newtonsoft.Json.Linq;

    public class TemplateVersionResponse : BaseObjectResponse, ITemplateVersionResponse
    {
        #region ITemplateVersionInfo Members

        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public bool? Published { get; set; }

        public string Subject { get; set; }

        public string Html { get; set; }

        public string Text { get; set; }

        #endregion

        #region Base Class Overrides

        public override Type InterfaceType => typeof(ITemplateVersionResponse);

        protected internal override void Populate(IResponseFactory responseFactory, JObject json)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
