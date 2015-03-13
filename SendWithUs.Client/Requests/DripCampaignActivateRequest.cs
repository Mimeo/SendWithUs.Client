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
    /// Represents the data necessary to make API requests to activate a drip campaign.
    /// </summary>
    [JsonConverter(typeof(DripCampaignActivateRequestConverter))]
    public class DripCampaignActivateRequest : IDripCampaignActivateRequest
    {
        #region Constructors

        public DripCampaignActivateRequest()
        { }

        public DripCampaignActivateRequest(string campaignId, string recipientAddress) : this(campaignId, recipientAddress, null)
        { }

        public DripCampaignActivateRequest(string campaignId, string recipientAddress, object data)
        {
            EnsureArgument.NotNullOrEmpty(campaignId, "campaignId");
            EnsureArgument.NotNullOrEmpty(recipientAddress, "recipientAddress");

            this.CampaignId = campaignId;
            this.RecipientAddress = recipientAddress;
            this.Data = data;
        }

        #endregion

        #region IDripCampaignActivateRequest members

        public virtual string CampaignId { get; set; }

        public virtual string SenderAddress { get; set; }

        public virtual string SenderName { get; set; }

        public virtual string SenderReplyTo { get; set; }

        public virtual string RecipientAddress { get; set; }

        public virtual string RecipientName { get; set; }

        public virtual IEnumerable<string> CopyTo { get; set; }

        public virtual IEnumerable<string> BlindCopyTo { get; set; }

        public virtual object Data { get; set; }

        public virtual IEnumerable<string> Tags { get; set; }

        public virtual string ProviderId { get; set; }

        public virtual string Locale { get; set; }

        public virtual bool IsValid
        {
            get { return this.Validate(false); }
        }

        #endregion

        #region IRequest members

        public string GetUriPath()
        {
            return "/api/v1/" + string.Format("drip_campaigns/{0}/activate", this.CampaignId);
        }

        public string GetHttpMethod()
        {
            return "POST";
        }

        public Type GetResponseType()
        {
            return typeof(DripCampaignActivateResponse);
        }

        public IRequest Validate()
        {
            this.Validate(true);
            return this;
        }

        protected internal virtual bool Validate(bool throwOnFailure)
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
            if (String.IsNullOrEmpty(this.CampaignId))
            {
                return fail(ValidationFailureMode.MissingCampaignId);
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
    }

    /// <summary>
    /// Represents the data necessary to make API requests to activate a drip campaign.
    /// </summary>
    /// <typeparam name="TData">The type of the Data property.</typeparam>
    public class DripCampaignActivateRequest<TData> : DripCampaignActivateRequest
    {
        #region Constructors

        public DripCampaignActivateRequest()
            : base()
        { }

        public DripCampaignActivateRequest(string campaignId, string recipientAddress)
            : base(campaignId, recipientAddress)
        { }

        public DripCampaignActivateRequest(string campaignId, string recipientAddress, TData data)
            : base(campaignId, recipientAddress, data)
        { }

        #endregion

        public new TData Data
        {
            get { return (TData)base.Data; }
            set { base.Data = value; }
        }
    }
}
