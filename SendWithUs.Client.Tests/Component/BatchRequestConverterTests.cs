﻿// Copyright © 2015 Mimeo, Inc.

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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class BatchRequestConverterTests : ComponentTestsBase
    {
        [TestMethod]
        public void WriteJson_ValidBatchRequest_Succeeds()
        {
            var templateId = TestHelper.GetUniqueId();
            var recipientAddress = TestHelper.GetUniqueId();
            var sendRequest = new SendRequest(templateId, recipientAddress);
            var renderRequest = new RenderRequest(templateId);
            var requests = new List<IRequest> { sendRequest, renderRequest };
            var batchRequest = new BatchRequest(requests);
            var writer = BufferedJsonStringWriter.Create();
            var serializer = JsonSerializer.Create();
            var converter = new BatchRequestConverter();

            converter.WriteJson(writer, batchRequest, serializer);
            var jsonArray = writer.GetBufferAs<JArray>();

            Assert.IsNotNull(jsonArray);
            Assert.AreEqual(requests.Count, jsonArray.Count);

            var sendResponse = jsonArray[0] as JObject;
            var pathProperty = sendResponse.Property("path");
            var methodProperty = sendResponse.Property("method");
            var bodyProperty = sendResponse.Property("body");

            Assert.IsNotNull(pathProperty);
            Assert.IsTrue(pathProperty.HasValues);
            Assert.AreEqual(sendRequest.GetUriPath(), (string)pathProperty.Value);
            Assert.IsNotNull(methodProperty);
            Assert.IsTrue(methodProperty.HasValues);
            Assert.AreEqual(sendRequest.GetHttpMethod(), (string)methodProperty.Value);
            Assert.IsNotNull(bodyProperty);
            Assert.IsInstanceOfType(bodyProperty.Value, typeof(JObject));

            this.ValidateSendRequest(sendRequest, bodyProperty.Value as JObject);

            var renderResponse = jsonArray[1] as JObject;
            pathProperty = renderResponse.Property("path");
            methodProperty = renderResponse.Property("method");
            bodyProperty = renderResponse.Property("body");

            Assert.IsNotNull(pathProperty);
            Assert.IsTrue(pathProperty.HasValues);
            Assert.AreEqual(renderRequest.GetUriPath(), (string)pathProperty.Value);
            Assert.IsNotNull(methodProperty);
            Assert.IsTrue(methodProperty.HasValues);
            Assert.AreEqual(renderRequest.GetHttpMethod(), (string)methodProperty.Value);
            Assert.IsNotNull(bodyProperty);
            Assert.IsInstanceOfType(bodyProperty.Value, typeof(JObject));

            this.ValidateRenderRequest(bodyProperty.Value as JObject, templateId, null);
        }
    }
}
