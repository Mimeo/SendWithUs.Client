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
    using SendWithUs.Client;
    using Xunit;

    public class BatchAsyncTests
    {
        [Fact]
        public void BatchAsync_TwoRequests_Succeeds()
        {
            // Arange
            var apiKey = TestData.GetApiKey();
            var subject = "BatchAsync " + TestHelper.GetUniqueId();
            dynamic testData = TestData.Load(nameof(SendRequest));
            var data = ((IDictionary<string, object>)testData.Data).Upsert("subject", subject);
            var sendRequest = new SendRequest { TemplateId = testData.TemplateId, RecipientAddress = testData.RecipientAddress, Data = data };
            var renderRequest = new RenderRequest { TemplateId = testData.TemplateId };
            var requests = new List<IRequest> { sendRequest, renderRequest };
            var client = new SendWithUsClient(apiKey);

            // Act
            var batchResponse = client.BatchAsync(requests).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, batchResponse.StatusCode);
            Assert.Equal(requests.Count, batchResponse.Items.Count());

            var sendResponse = batchResponse.Items.ElementAt(0) as ISendResponse;
            Assert.Equal(HttpStatusCode.OK, sendResponse.StatusCode);
            Assert.Equal("OK", sendResponse.Status, true);
            Assert.Equal(true, sendResponse.Success);

            var renderResponse = batchResponse.Items.ElementAt(1) as IRenderResponse;
            Assert.Equal(HttpStatusCode.OK, renderResponse.StatusCode);
            Assert.Equal("OK", renderResponse.Status, true);
            Assert.Equal(true, renderResponse.Success);
        }
    }
}
