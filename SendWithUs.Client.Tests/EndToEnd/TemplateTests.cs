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
    using SendWithUs.Client;
    using System.Net;
    using Xunit;

    public class TemplateTests
    {
        [Fact]
        public void GetTemplateCollectionRequest_AllTemplates_Succeeds()
        {
            // Arrange
            var apiKey = TestData.GetApiKey();
            var request = GetTemplateCollectionRequest.AllTemplates;
            var client = new SendWithUsClient(apiKey);

            // Act
            var response = client.ExecuteAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void GetTemplateRequest_TemplateById_Succeeds()
        {
            // Arrange
            var apiKey = TestData.GetApiKey();
            dynamic testData = TestData.Load("Template");
            var request = new GetTemplateRequest { TemplateId = testData.ExistingTemplateId };
            var client = new SendWithUsClient(apiKey);

            // Act
            var response = client.ExecuteAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void CreateTemplateRequest_NewTemplate_Succeeds()
        {
            // Arrange
            var apiKey = TestData.GetApiKey();
            var name = "testSendWithUsClient";
            var subject = "purple monkey dishwasher";
            var request = new CreateTemplateRequest { Name = name, Subject = subject, Text = "foobar" };
            var client = new SendWithUsClient(apiKey);

            // Act
            var response = client.ExecuteAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void CreateTemplateRequest_AddLocale_Succeeds()
        {
            // Arrange
            var apiKey = TestData.GetApiKey();
            dynamic testData = TestData.Load("Template");
            var name = "a-new-locale";
            var subject = "whatever dude";
            var request = new CreateTemplateRequest
            {
                TemplateId = testData.ExistingTemplateId,
                Locale = testData.NewLocale,
                Name = name,
                Subject = subject,
                Text = testData.Text
            };
            var client = new SendWithUsClient(apiKey);

            // Act
            var response = client.ExecuteAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
