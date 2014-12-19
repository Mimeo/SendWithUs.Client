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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents the data necessary to make API requests to send email messages.
    /// </summary>
    [JsonConverter(typeof(SendRequestConverter))]
    public class SendRequest : ISendRequest
    {
        #region Constructors

        public SendRequest(string templateId, string recipientAddress) : this(templateId, recipientAddress, null)
        {}

        public SendRequest(string templateId, string recipientAddress, IDictionary<string, string> data)
        {
            this.TemplateId = templateId;
            this.RecipientAddress = recipientAddress;
            this.Data = data;
        }

        public SendRequest(ISendRequest source) : this(source.TemplateId, source.RecipientAddress, source.Data)
        {
            this.SenderAddress = source.SenderAddress;
            this.SenderName = source.SenderName;
            this.SenderReplyTo = source.SenderReplyTo;
            this.RecipientName = source.RecipientName;
            this.CopyTo = source.CopyTo;
            this.BlindCopyTo = source.BlindCopyTo;
            this.Tags = source.Tags;
            this.InlineAttachment = source.InlineAttachment;
            this.FileAttachments = source.FileAttachments;
            this.ProviderId = source.ProviderId;
            this.TemplateVersionId = source.TemplateVersionId;
        }

        #endregion

        #region ISendRequest members

        public string TemplateId { get; set; }

        public string SenderAddress { get; set; }

        public string SenderName { get; set; }

        public string SenderReplyTo { get; set; }

        public string RecipientAddress { get; set; }

        public string RecipientName { get; set; }

        public IEnumerable<string> CopyTo { get; set; }

        public IEnumerable<string> BlindCopyTo { get; set; }

        public IDictionary<string, string> Data { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IAttachment InlineAttachment { get; set; }

        public IEnumerable<IAttachment> FileAttachments { get; set; }

        public string ProviderId { get; set; }

        public string TemplateVersionId { get; set; }

        public bool IsValid
        {
            get { return this.Validate(false); }
        }

        #endregion

        #region IRequest members

        public string GetUriPath()
        {
            return "/api/v1/send";
        }

        public string GetHttpMethod()
        {
            return "POST";
        }

        public Type GetResponseType()
        {
            return typeof(SendResponse);
        }

        public IRequest Validate()
        {
            this.Validate(true);
            return this;
        }

        protected bool Validate(bool throwOnFailure)
        {
            Func<ValidationFailureMode, bool> fail = (f) =>
            {
                if (throwOnFailure)
                {
                    throw new ValidationException(f);
                }

                return false;
            };

            // Template ID is required.
            if (String.IsNullOrEmpty(this.TemplateId))
            {
                return fail(ValidationFailureMode.MissingTemplateId);
            }

            // Recipient address is required.
            if (String.IsNullOrEmpty(this.RecipientAddress))
            {
                return fail(ValidationFailureMode.MissingRecipientAddress);
            }

            // If sender name or reply-to are specified, then sender address is required.
            if ((!String.IsNullOrEmpty(this.SenderName) || !String.IsNullOrEmpty(this.SenderReplyTo))
                && String.IsNullOrEmpty(this.SenderAddress))
            {
                return fail(ValidationFailureMode.MissingSenderAddress);
            }

            return true;
        }

        #endregion

        #region JSON conversion

#if false
        protected JObject Convert()
        {
            this.Validate();

            var result = new JObject(new JProperty("email_id", this.TemplateId));
            var recipient = new JObject(new JProperty("address", this.RecipientAddress));

            if (!String.IsNullOrEmpty(this.RecipientName))
            {
                recipient.Add("name", new JValue(this.RecipientName));
            }

            result.Add("recipient", recipient);

            if (this.CopyTo != null)
            {
                result.Add("cc", new JArray(this.CopyTo.Select(a => new JObject(new JProperty("address", a)))));
            }

            if (this.BlindCopyTo != null)
            {
                result.Add("bcc", new JArray(this.BlindCopyTo.Select(a => new JObject(new JProperty("address", a)))));
            }

            if (!String.IsNullOrEmpty(this.SenderAddress))
            {
                var sender = new JObject();

                if (!String.IsNullOrEmpty(this.SenderName))
                {
                    sender.Add("name", this.SenderName);
                }

                if (!String.IsNullOrEmpty(this.SenderReplyTo))
                {
                    sender.Add("reply_to", this.SenderReplyTo);
                }

                result.Add("sender", sender);
            }

            if (this.Data != null)
            {
                result.Add("email_data", new JObject(this.Data.Select(kvp => new JProperty(kvp.Key, kvp.Value))));
            }

            if (this.Tags != null)
            {
                result.Add("tags", new JArray(this.Tags));
            }

            if (this.InlineAttachment != null)
            {
                result.Add("inline", this.Convert(this.InlineAttachment));
            }

            if (this.FileAttachments != null)
            {
                result.Add("files", new JArray(this.FileAttachments.Select(a => this.Convert(a))));
            }

            if (!String.IsNullOrEmpty(this.ProviderId))
            {
                result.Add("esp_account", this.ProviderId);
            }

            if (!String.IsNullOrEmpty(this.TemplateVersionId))
            {
                result.Add("version_name", this.TemplateVersionId);
            }

            return result;
        }

        protected JObject Convert(IAttachment attachment)
        {
            return new JObject(new JProperty("id", attachment.Id), new JProperty("data", attachment.Data));
        }
#endif

        #endregion
    }
}
