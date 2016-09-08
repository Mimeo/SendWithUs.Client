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

using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void BatchAsync_TwoRequests_Succeeds()
        {
            // Arange
            var subject = "BatchAsync " + TestHelper.GetUniqueId();
            var testData = new TestData("EndToEnd/Data/SendRequest.xml");
            var sendRequest = new SendRequest(testData.TemplateId, testData.RecipientAddress, testData.Data.Upsert("subject", subject));
            var renderRequest = new RenderRequest(testData.TemplateId);
            var requests = new List<IRequest> { sendRequest, renderRequest };
            var client = new SendWithUsClient(testData.ApiKey);

            // Act
            var batchResponse = client.BatchAsync(requests).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, batchResponse.StatusCode);
            Assert.AreEqual(requests.Count, batchResponse.Items.Count());

            var sendResponse = batchResponse.Items.ElementAt(0) as ISendResponse;
            Assert.AreEqual(HttpStatusCode.OK, sendResponse.StatusCode);
            Assert.AreEqual("OK", sendResponse.Status, true);
            Assert.AreEqual(true, sendResponse.Success);

            var renderResponse = batchResponse.Items.ElementAt(1) as IRenderResponse;
            Assert.AreEqual(HttpStatusCode.OK, renderResponse.StatusCode);
            Assert.AreEqual("OK", renderResponse.Status, true);
            Assert.AreEqual(true, renderResponse.Success);
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
            var batchItems = batchResponse.Items.ToList();

            Assert.AreEqual(HttpStatusCode.OK, batchResponse.StatusCode);
            Assert.AreEqual(2, batchItems.Count());

            var dripCampaignActivateResponse = batchItems.First() as IDripCampaignActivateResponse;
            Assert.AreEqual(HttpStatusCode.OK, dripCampaignActivateResponse.StatusCode);
            Assert.AreEqual("OK", dripCampaignActivateResponse.Status, true);
            Assert.AreEqual(true, dripCampaignActivateResponse.Success);
            batchItems.RemoveAt(0);

            var sendResponse = batchItems.First() as ISendResponse;
            Assert.AreEqual(HttpStatusCode.OK, sendResponse.StatusCode);
            Assert.AreEqual("OK", sendResponse.Status, true);
            Assert.AreEqual(true, sendResponse.Success);
        }

        [TestMethod]
        public void BatchAsync_SendMultipleCustomerWithData_Succeeds()
        {
            // Item 1
            var testData = new TestData("EndToEnd/Data/CustomerUpdateRequest.xml");
            var request1 = new CustomerUpdateRequest(testData.Email, testData.Data);

            // Item 2
            testData = new TestData("EndToEnd/Data/CustomerUpdateRequest2.xml");
            var request2 = new CustomerUpdateRequest(testData.Email, testData.Data);

            var client = new SendWithUsClient(testData.ApiKey);

            var batchResponse = client.BatchAsync(new List<IRequest> { request1, request2 }).Result;
            var batchItems = batchResponse.Items.ToList();

            Assert.AreEqual(HttpStatusCode.OK, batchResponse.StatusCode);
            Assert.AreEqual(2, batchItems.Count());

            var updateCustomerResponse1 = batchItems.First() as ICustomerUpdateResponse;
            Assert.AreEqual(HttpStatusCode.OK, updateCustomerResponse1.StatusCode);
            Assert.AreEqual("OK", updateCustomerResponse1.Status, true);
            Assert.AreEqual(true, updateCustomerResponse1.Success);
            batchItems.RemoveAt(0);

            var updateCustomerResponse2 = batchItems.First() as ICustomerUpdateResponse;
            Assert.AreEqual(HttpStatusCode.OK, updateCustomerResponse2.StatusCode);
            Assert.AreEqual("OK", updateCustomerResponse2.Status, true);
            Assert.AreEqual(true, updateCustomerResponse2.Success);
        }
    }
}
