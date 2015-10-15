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
    public abstract class BaseTemplateRequestWithPayload : BaseTemplateRequest
    {
        #region Properties

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        #endregion

        #region Conditional Serialization

        public bool ShouldSerializeLocale() => String.IsNullOrEmpty(this.TemplateId) && !String.IsNullOrEmpty(this.Locale);

        #endregion

        #region Base Class Overrides
        
        public override string GetUriPath() => String.IsNullOrEmpty(this.TemplateId) ? BaseUriPath : $"{BaseUriPath}/{this.TemplateId}/locales";

        protected internal override IEnumerable<string> GetMissingRequiredProperties()
        {
            if (String.IsNullOrEmpty(this.Name))
            {
                yield return nameof(this.Name);
            }

            if (String.IsNullOrEmpty(this.Subject))
            {
                yield return nameof(this.Subject);
            }

            if (String.IsNullOrEmpty(this.Html) && String.IsNullOrEmpty(this.Text))
            {
                yield return "HtmlOrText";
            }
            
            foreach (var property in base.GetMissingRequiredProperties())
            {
                yield return property;
            }
        }

        #endregion
    }
}
