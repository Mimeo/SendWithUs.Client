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

        public SendRequest()
        { }

        public SendRequest(string templateId, string recipientAddress) : this(templateId, recipientAddress, null)
        { }

        public SendRequest(string templateId, string recipientAddress, object data)
        {
            EnsureArgument.NotNullOrEmpty(templateId, "templateId");
            EnsureArgument.NotNullOrEmpty(recipientAddress, "recipientAddress");

            this.TemplateId = templateId;
            this.RecipientAddress = recipientAddress;
            this.Data = data;
        }

        #endregion

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

        public virtual IRequest Validate()
        {
            // Template ID is required.
            if (String.IsNullOrEmpty(this.TemplateId))
            {
                throw new ValidationException(ValidationFailureMode.MissingTemplateId);
            }

            // Recipient address is required.
            if (String.IsNullOrEmpty(this.RecipientAddress))
            {
                throw new ValidationException(ValidationFailureMode.MissingRecipientAddress);
            }

            // If sender name or reply-to are specified, then sender address is required.
            if ((!String.IsNullOrEmpty(this.SenderName) || !String.IsNullOrEmpty(this.SenderReplyTo))
                && String.IsNullOrEmpty(this.SenderAddress))
            {
                throw new ValidationException(ValidationFailureMode.MissingSenderAddress);
            }

            return this;
        }

        public virtual bool IsValid
        {
            get
            {
                try
                {
                    this.Validate();
                }
                catch (ValidationException)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents the data necessary to make API requests to send email messages.
    /// </summary>
    /// <typeparam name="TData">The type of the Data property.</typeparam>
    public class SendRequest<TData> : SendRequest
    {
        #region Constructors

        public SendRequest() : base()
        { }

        public SendRequest(string templateId, string recipientAddress) : base(templateId, recipientAddress)
        { }

        public SendRequest(string templateId, string recipientAddress, TData data) : base(templateId, recipientAddress, data)
        { }

        #endregion

        public new TData Data 
        {
            get { return (TData)base.Data; }
            set { base.Data = value; }
        }
    }
}
