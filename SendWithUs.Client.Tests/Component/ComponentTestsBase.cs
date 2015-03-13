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

    public abstract class ComponentTestsBase
    {
        protected void ValidateSendRequest(JObject jsonObject, string expectedEmailId, string expectedRecipientAddress, bool allowOtherProperties)
        {
            var emailIdFound = false;
            var recipientAddressFound = false;

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "email_id":
                        Assert.AreEqual(expectedEmailId, pair.Value);
                        emailIdFound = true;
                        break;

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

            Assert.IsTrue(emailIdFound);
            Assert.IsTrue(recipientAddressFound);
        }

        protected void ValidateDripCampaignActivateRequest(JObject jsonObject, string expectedRecipientAddress, bool allowOtherProperties)
        {
            var recipientAddressFound = false;

            foreach(var pair in jsonObject)
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
