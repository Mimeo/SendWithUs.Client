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

namespace SendWithUs.Client.Tests.EndToEnd
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SendWithUs.Client;

    [TestClass]
    public class DripCampaignDeactivateAsyncTests
    {
        [TestMethod]
        public void DripCampaignDeactivateAsync_MinimalRequest_Succeeds()
        {
            // Arrange
            var testData = new TestData("EndToEnd/Data/DripCampaignDeactivateRequest.xml");
            var request = new DripCampaignDeactivateRequest(testData.CampaignId, testData.RecipientAddress);
            var client = new SendWithUsClient(testData.ApiKey);

            // Act
            var response = client.SingleAsync<DripCampaignDeactivateResponse>(request).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(typeof(DripCampaignDeactivateResponse), response.GetType());
            var typedResponse = (DripCampaignDeactivateResponse)response;
            Assert.AreEqual("OK", typedResponse.Status, true);
            Assert.AreEqual(true, typedResponse.Success);
        }
    }
}
