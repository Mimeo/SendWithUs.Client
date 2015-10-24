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
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseTemplateRequest : BaseRequest
    {
        protected internal static readonly string BaseUriPath = "/api/v1/templates";
        
        protected internal abstract bool IsTemplateIdRequired();

        #region ITemplateRequest Members

        public virtual string TemplateId { get; set; }

        public virtual string Locale { get; set; }

        #endregion

        #region Base Class Overrides

        public override string GetUriPath()
        {
            if (String.IsNullOrEmpty(this.TemplateId))
            {
                return BaseUriPath;
            }

            if (String.IsNullOrEmpty(this.Locale))
            {
                return $"{BaseUriPath}/{this.TemplateId}";
            }

            return $"{BaseUriPath}/{this.TemplateId}/locales/{this.Locale}";
        }

        protected internal override IEnumerable<string> GetMissingRequiredProperties()
        {
            if (this.IsTemplateIdRequired() && String.IsNullOrEmpty(this.TemplateId))
            {
                yield return nameof(this.TemplateId);
            }

            foreach (var property in base.GetMissingRequiredProperties())
            {
                yield return property;
            }
        }

        #endregion
    }
}
