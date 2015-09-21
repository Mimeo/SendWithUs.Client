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
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the data necessary to make API requests to send email messages.
    /// </summary>
    [JsonConverter(typeof(SendRequestConverter))]
    public class SendRequest : BaseRequest, ISendRequest
    {
        #region ISendRequest members

        public virtual string TemplateId { get; set; }

        public virtual string SenderAddress { get; set; }

        public virtual string SenderName { get; set; }

        public virtual string SenderReplyTo { get; set; }

        public virtual string RecipientAddress { get; set; }

        public virtual string RecipientName { get; set; }

        public virtual IEnumerable<string> CopyTo { get; set; }

        public virtual IEnumerable<string> BlindCopyTo { get; set; }

        public virtual object Data { get; set; }

        public virtual IEnumerable<string> Tags { get; set; }

        public virtual IDictionary<string, string> Headers { get; set; }

        public virtual IAttachment InlineAttachment { get; set; }

        public virtual IEnumerable<IAttachment> FileAttachments { get; set; }

        public virtual string ProviderId { get; set; }

        public virtual string TemplateVersionId { get; set; }

        public virtual string Locale { get; set; }

        #endregion

        #region Base Class Overrides

        public override string GetUriPath() => "/api/v1/send";

        public override string GetHttpMethod() => "POST";

        public override Type GetResponseType() => typeof(SendResponse);

        protected override IEnumerable<string> GetMissingRequiredProperties()
        {
            if (String.IsNullOrEmpty(this.TemplateId))
            {
                yield return nameof(this.TemplateId);
            }
            
            if (String.IsNullOrEmpty(this.RecipientAddress))
            {
                yield return nameof(this.RecipientAddress);
            }

            foreach (var property in base.GetMissingRequiredProperties())
            {
                yield return property;
            }
        }

        #endregion
    }
}
