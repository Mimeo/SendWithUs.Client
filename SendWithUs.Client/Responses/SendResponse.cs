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

    public class SendResponse : BaseResponse<JObject>, ISendResponse
    {
        public bool Success { get; set; }

        public string Status { get; set; }

        public string ReceiptId { get; set; }

        public string TemplateName { get; set; }

        public string TemplateVersionId { get; set; }

        #region Base class overrides 

        protected override void Populate(JObject json)
        {
            if (json == null)
            {
                return;
            }

            this.Success = json.Value<bool>("success");
            this.Status = json.Value<string>("status");
            this.ReceiptId = json.Value<string>("receipt_id");

            var details = json.Property("email");

            if (details != null && details.HasValues)
            {
                this.TemplateName = details.Value.Value<string>("name");
                this.TemplateVersionId = details.Value.Value<string>("version_name");
            }
        }

        #endregion
    }
}
