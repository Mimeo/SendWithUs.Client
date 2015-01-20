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
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class BatchRequestWrapperConverterTests : ComponentTestsBase
    {
        [TestMethod]
        public void WriteJson_ValidBatchRequestWrapper_Succeeds()
        {
            var templateId = TestHelper.GetUniqueId();
            var recipientAddress = TestHelper.GetUniqueId();
            var request = new SendRequest(templateId, recipientAddress);
            var wrapper = new BatchRequestWrapper(request);
            var writer = JsonObjectWriter.Create();
            var serializer = JsonSerializer.Create();
            var converter = new BatchRequestWrapperConverter();

            converter.WriteJson(writer, wrapper, serializer);
            var jsonObject = writer.Get<JObject>();

            Assert.IsNotNull(jsonObject);
            var pathProperty = jsonObject.Property("path");
            Assert.IsNotNull(pathProperty);
            Assert.IsTrue(pathProperty.HasValues);
            Assert.AreEqual(request.GetUriPath(), (string)pathProperty.Value);
            var methodProperty = jsonObject.Property("method");
            Assert.IsNotNull(methodProperty);
            Assert.IsTrue(methodProperty.HasValues);
            Assert.AreEqual(request.GetHttpMethod(), (string)methodProperty.Value);
            var bodyProperty = jsonObject.Property("body");
            Assert.IsNotNull(bodyProperty);
            Assert.IsInstanceOfType(bodyProperty.Value, typeof(JObject));
            this.ValidateSendRequest(bodyProperty.Value as JObject, templateId, recipientAddress, false);
        }
    }
}
