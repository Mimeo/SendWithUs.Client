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

    public class CreateTemplateResponse : BaseObjectResponse, ICreateTemplateResponse
    {
        internal static class PropertyNames
        {
            public const string TemplateId = "id";
            public const string Name = "name";
            public const string Locale = "locale";
        }

        #region ICreateTemplateResponse Members

        public string TemplateId { get; set; }

        public string Name { get; set; }

        public string Locale { get; set; }

        #endregion

        #region Base Class Overrides

        protected internal override void Populate(JObject json)
        {
            if (json == null)
            {
                return;
            }

            this.TemplateId = json.Value<string>(PropertyNames.TemplateId);
            this.Name = json.Value<string>(PropertyNames.Name);
            this.Locale = json.Value<string>(PropertyNames.Locale);
        }

        #endregion
    }
}
