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
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    public abstract class ComponentTestsBase
    {
        protected void ValidateSendRequest(JObject jsonObject, string expectedEmailId, string expectedRecipientAddress, IDictionary<string,string> expectedData)
        {
            var emailIdFound = false;
            var recipientAddressFound = false;

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "email_id":
                        Assert.AreEqual(expectedEmailId, pair.Value.Value<string>());
                        emailIdFound = true;
                        break;

                    case "recipient":
                        Assert.AreEqual(expectedRecipientAddress, pair.Value["address"].Value<string>());
                        recipientAddressFound = true;
                        break;

                    case "email_data":
                        if (expectedData != null)
                        {
                            this.ValidateRequestData(pair.Value as JObject, expectedData);
                        }
                        break;

                    default:
                        break;
                }
            }

            Assert.IsTrue(emailIdFound);
            Assert.IsTrue(recipientAddressFound);
        }

        protected void ValidateRenderRequest(JObject jsonObject, string expectedTemplateId, IDictionary<string, string> expectedData)
        {
            var templateIdFound = false;

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "template_id":
                        Assert.AreEqual(expectedTemplateId, pair.Value.Value<string>());
                        templateIdFound = true;
                        break;

                    case "template_data":
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
