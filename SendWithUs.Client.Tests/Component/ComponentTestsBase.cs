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
    using RenderPropertyNames = SendWithUs.Client.RenderRequestConverter.PropertyNames;
    using SendPropertyNames = SendWithUs.Client.SendRequestConverter.PropertyNames;

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
                    case SendPropertyNames.TemplateId:
                        Assert.AreEqual(request.TemplateId, pair.Value.Value<string>());
                        templateIdFound = true;
                        break;

                    case SendPropertyNames.Sender:
                        this.ValidateSenderObject(request, pair.Value);
                        break;

                    case SendPropertyNames.Recipient:
                        Assert.AreEqual(request.RecipientAddress, pair.Value[SendPropertyNames.Address].Value<string>());
                        recipientAddressFound = true;
                        break;

                    case SendPropertyNames.Data:
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
                Assert.AreEqual(request.SenderName, senderObject[SendPropertyNames.Name].Value<string>());
            }

            if (!String.IsNullOrEmpty(request.SenderAddress))
            {
                Assert.AreEqual(request.SenderAddress, senderObject[SendPropertyNames.Address].Value<string>());
            }

            if (!String.IsNullOrEmpty(request.SenderReplyTo))
            {
                Assert.AreEqual(request.SenderReplyTo, senderObject[SendPropertyNames.ReplyTo].Value<string>());
            }
        }

        protected void ValidateRenderRequest(JObject jsonObject, string expectedTemplateId, IDictionary<string, string> expectedData)
        {
            var templateIdFound = false;

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case RenderPropertyNames.TemplateId:
                        Assert.AreEqual(expectedTemplateId, pair.Value.Value<string>());
                        templateIdFound = true;
                        break;

                    case RenderPropertyNames.Data:
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

        protected void ValidateRequestData(JObject actualData, IDictionary<string, string> expectedData)
        {
            Assert.AreEqual(expectedData.Count, actualData.Count);

            foreach (var pair in actualData)
            {
                Assert.IsTrue(expectedData.ContainsKey(pair.Key));
                Assert.AreEqual(expectedData[pair.Key], pair.Value.Value<string>());
            }
        }
    }
}
