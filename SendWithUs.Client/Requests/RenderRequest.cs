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
    using System;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the data necessary to make API requests to render templates.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class RenderRequest : BaseRequest, IRenderRequest
    {
        #region IRenderRequest Members

        [JsonProperty("template")]
        public string TemplateId { get; set; }

        [JsonProperty("version_id")]
        public string TemplateVersionId { get; set; }

        [JsonProperty("template_data")]
        public object Data { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("strict")]
        public bool Strict { get; set; }

        #endregion

        #region Base Class Overrides

        public override string GetHttpMethod() => "POST";

        public override string GetUriPath() => "/api/v1/render";

        public override Type GetResponseType() => typeof(RenderResponse);

        protected internal override IEnumerable<string> GetMissingRequiredProperties()
        {
            if (String.IsNullOrEmpty(this.TemplateId))
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
