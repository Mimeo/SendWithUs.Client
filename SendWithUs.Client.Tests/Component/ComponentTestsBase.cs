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

namespace SendWithUs.Client.Tests.Component
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using RenderNames = SendWithUs.Client.RenderRequestConverter.PropertyNames;
    using SendNames = SendWithUs.Client.SendRequestConverter.PropertyNames;
    using CustomerUpdateNames = SendWithUs.Client.CustomerUpdateRequestConverter.PropertyNames;
    using System.Linq;
    public abstract class ComponentTestsBase
    {
        protected void ValidateSendRequest(SendRequest request, JObject jsonObject)
        {
            var templateIdFound = false;
            var recipientAddressFound = false;

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case SendNames.TemplateId:
                        Assert.AreEqual(request.TemplateId, pair.Value.Value<string>());
                        templateIdFound = true;
                        break;

                    case SendNames.Sender:
                        this.ValidateSenderObject(request, pair.Value);
                        break;

                    case SendNames.Recipient:
                        Assert.AreEqual(request.RecipientAddress, pair.Value[SendNames.Address].Value<string>());
                        recipientAddressFound = true;
                        break;

                    case SendNames.Data:
                        if (request.Data != null)
                        {
                            this.ValidateRequestData(pair.Value as JObject, request.Data as IDictionary<string, string>);
                        }
                        break;

                    default:
                        break;
                }
            }

            Assert.IsTrue(templateIdFound);
            Assert.IsTrue(recipientAddressFound);
        }

        protected void ValidateSenderObject(SendRequest request, JToken json)
        {
            var senderObject = json as JObject;
            Assert.IsNotNull(senderObject);

            if (!String.IsNullOrEmpty(request.SenderName))
            {
                Assert.AreEqual(request.SenderName, senderObject[SendNames.Name].Value<string>());
            }

            if (!String.IsNullOrEmpty(request.SenderAddress))
            {
                Assert.AreEqual(request.SenderAddress, senderObject[SendNames.Address].Value<string>());
            }

            if (!String.IsNullOrEmpty(request.SenderReplyTo))
            {
                Assert.AreEqual(request.SenderReplyTo, senderObject[SendNames.ReplyTo].Value<string>());
            }
        }

        protected void ValidateRenderRequest(JObject jsonObject, string expectedTemplateId, IDictionary<string, string> expectedData)
        {
            var templateIdFound = false;

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case RenderNames.TemplateId:
                        Assert.AreEqual(expectedTemplateId, pair.Value.Value<string>());
                        templateIdFound = true;
                        break;

                    case RenderNames.Data:
                        if (expectedData != null)
                        {
                            this.ValidateRequestData(pair.Value as JObject, expectedData);
                        }
                        break;

                    default:
                        break;
                }
            }

            Assert.IsTrue(templateIdFound);
        }

        protected void ValidateCustomerUpdateRequest(CustomerUpdateRequest request, JObject jsonObject)
        {
            var emailFound = false;

            foreach(var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case CustomerUpdateNames.Email:
                        Assert.AreEqual(request.Email, pair.Value.Value<string>());
                        emailFound = true;
                        break;
                    case CustomerUpdateNames.Locale:
                        Assert.AreEqual(request.Locale, pair.Value.Value<string>());
                        break;
                    case CustomerUpdateNames.Groups:
                        Assert.IsTrue(Enumerable.SequenceEqual(request.Groups, pair.Value.Values<string>()));
                        break;
                }
            }

            Assert.IsTrue(emailFound);
        }

        protected void ValidateRequestData(JObject actualData, IDictionary<string, string> expectedData)
        {
            Assert.AreEqual(expectedData.Count, actualData.Count);

            foreach (var pair in actualData)
            {
                Assert.IsTrue(expectedData.ContainsKey(pair.Key));
                Assert.AreEqual(expectedData[pair.Key], pair.Value.Value<string>());
            }
        }

        protected void ValidateDripCampaignActivateRequest(JObject jsonObject, string expectedRecipientAddress, bool allowOtherProperties)
        {
            var recipientAddressFound = false;

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "recipient":
                        Assert.AreEqual(expectedRecipientAddress, pair.Value["address"].Value<string>());
                        recipientAddressFound = true;
                        break;
                    default:
                        if (!allowOtherProperties)
                        {
                            Assert.Fail("Unexpected object property '{0}'", pair.Key);
                        }
                        break;
                }
            }

            Assert.IsTrue(recipientAddressFound);
        }

        protected void ValidateDripCampaignDeactivateRequest(JObject jsonObject, string expectedRecipientAddress, bool allowOtherProperties)
        {
            var recipientAddressFound = false;

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "recipient_address":
                        Assert.AreEqual(expectedRecipientAddress, pair.Value);
                        recipientAddressFound = true;
                        break;
                    default:
                        if (!allowOtherProperties)
                        {
                            Assert.Fail("Unexpected object property '{0}'", pair.Key);
                        }
                        break;
                }
            }

            Assert.IsTrue(recipientAddressFound);
        }

        protected void ValidateDripCampaignDeactivateAllRequest(JObject jsonObject, string expectedRecipientAddress, bool allowOtherProperties)
        {
            var recipientAddressFound = false;

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "recipient_address":
                        Assert.AreEqual(expectedRecipientAddress, pair.Value);
                        recipientAddressFound = true;
                        break;
                    default:
                        if (!allowOtherProperties)
                        {
                            Assert.Fail("Unexpected object property '{0}'", pair.Key);
                        }
                        break;
                }
            }

            Assert.IsTrue(recipientAddressFound);
        }
    }
}
