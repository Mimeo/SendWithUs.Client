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
    using Newtonsoft.Json.Linq;

    public class SendResponse : BaseObjectResponse, ISendResponse
    {
        internal static class PropertyNames
        {
            public const string Success = "success";
            public const string Status = "status";
            public const string ReceiptId = "receipt_id";
            public const string Details = "email";
            public const string TemplateName = "name";
            public const string TemplateVersionId = "version_name";
        }

        #region ISendResponse Members

        public virtual bool Success { get; set; }

        public virtual string Status { get; set; }

        public virtual string ReceiptId { get; set; }

        public virtual string TemplateName { get; set; }

        public virtual string TemplateVersionId { get; set; }

        #endregion

        #region Base class overrides

        protected internal override void Populate(JObject json)
        {
            if (json == null)
            {
                return;
            }

            this.Success = json.Value<bool>(PropertyNames.Success);
            this.Status = json.Value<string>(PropertyNames.Status);
            this.ReceiptId = json.Value<string>(PropertyNames.ReceiptId);

            var details = this.GetPropertyValue(json, PropertyNames.Details);

            if (details != null)
            {
                this.TemplateName = details.Value<string>(PropertyNames.TemplateName);
                this.TemplateVersionId = details.Value<string>(PropertyNames.TemplateVersionId);
            }
        }

        #endregion
    }
}
