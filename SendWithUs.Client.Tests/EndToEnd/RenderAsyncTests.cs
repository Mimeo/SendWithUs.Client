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
    using System.Net;
    using SendWithUs.Client;
    using Xunit;

    public class RenderAsyncTests
    {
        [Fact]
        public void RenderAsync_MinimalRequest_Succeeds()
        {
            // Arrange
            var apiKey = TestData.GetApiKey();
            dynamic testData = TestData.Load(nameof(RenderRequest));
            var request = new RenderRequest { TemplateId = testData.TemplateId };
            var client = new SendWithUsClient(apiKey);

            // Act
            var response = client.RenderAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("OK", response.Status, true);
            Assert.Equal(true, response.Success);
        }

        [Fact]
        public void RenderAsync_WithData_Succeeds()
        {
            // Arrange
            var apiKey = TestData.GetApiKey();
            dynamic testData = TestData.Load(nameof(RenderRequest));
            var subject = "RenderAsync_WithData " + TestHelper.GetUniqueId();
            var request = new RenderRequest { TemplateId = testData.TemplateId, Data = testData.Data };
            var client = new SendWithUsClient(apiKey);

            // Act 
            var response = client.RenderAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("OK", response.Status, true);
            Assert.Equal(true, response.Success);
        }
    }
}
