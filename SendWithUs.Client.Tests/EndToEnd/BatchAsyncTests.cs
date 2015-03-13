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

namespace SendWithUs.Client.Tests.EndToEnd
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SendWithUs.Client;

    [TestClass]
    public class BatchAsyncTests
    {
        [TestMethod]
        public void BatchAsync_OneSendRequest_Succeeds()
        {
            var subject = "BatchAsync " + TestHelper.GetUniqueId();
            var testData = new TestData("EndToEnd/Data/SendRequest.xml");
            var request = new SendRequest(testData.TemplateId, testData.RecipientAddress, testData.Data.Upsert("subject", subject));
            var client = new SendWithUsClient(testData.ApiKey);

            var batchResponse = client.BatchAsync(new List<IRequest> { request }).Result;

            Assert.AreEqual(HttpStatusCode.OK, batchResponse.StatusCode);
            Assert.AreEqual(1, batchResponse.Items.Count());

            var sendResponse = batchResponse.Items.First() as ISendResponse;
            Assert.AreEqual(HttpStatusCode.OK, sendResponse.StatusCode);
            Assert.AreEqual("OK", sendResponse.Status, true);
            Assert.AreEqual(true, sendResponse.Success);
        }

        [TestMethod]
        public void BatchAsync_SendMultipleDifferentTypes_Succeeds()
        {
            // Item 1
            var subject = "BatchAsync " + TestHelper.GetUniqueId();
            var testData = new TestData("EndToEnd/Data/DripCampaignActivateRequest.xml");
            var request1 = new DripCampaignActivateRequest(testData.CampaignId, testData.RecipientAddress, testData.Data.Upsert("subject", subject));

            // Item 2
            subject = "BatchAsync " + TestHelper.GetUniqueId();
            testData = new TestData("EndToEnd/Data/SendRequest.xml");
            var request2 = new SendRequest(testData.TemplateId, testData.RecipientAddress, testData.Data.Upsert("subject", subject));
            
            var client = new SendWithUsClient(testData.ApiKey);

            var batchResponse = client.BatchAsync(new List<IRequest> { request1, request2 }).Result;

            Assert.AreEqual(HttpStatusCode.OK, batchResponse.StatusCode);
            Assert.AreEqual(2, batchResponse.Items.Count());

            var dripCampaignActivateResponse = batchResponse.Items.First() as IDripCampaignActivateResponse;
            Assert.AreEqual(HttpStatusCode.OK, dripCampaignActivateResponse.StatusCode);
            Assert.AreEqual("OK", dripCampaignActivateResponse.Status, true);
            Assert.AreEqual(true, dripCampaignActivateResponse.Success);
            batchResponse.Items.RemoveAt(0);

            var sendResponse = batchResponse.Items.First() as ISendResponse;
            Assert.AreEqual(HttpStatusCode.OK, sendResponse.StatusCode);
            Assert.AreEqual("OK", sendResponse.Status, true);
            Assert.AreEqual(true, sendResponse.Success);
        }
    }
}
