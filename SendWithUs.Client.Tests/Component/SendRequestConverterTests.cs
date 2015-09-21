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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SendWithUs.Client;

    [TestClass]
    public class SendRequestConverterTests : ComponentTestsBase
    {
        [TestMethod]
        public void WriteJson_MinimalSendRequest_Succeeds()
        {
            var templateId = TestHelper.GetUniqueId();
            var recipientAddress = TestHelper.GetUniqueId();
            var request = new SendRequest { TemplateId = templateId, RecipientAddress = recipientAddress };
            var writer = BufferedJsonStringWriter.Create();
            var serializer = JsonSerializer.Create();
            var converter = new SendRequestConverter();

            converter.WriteJson(writer, request, serializer);
            var jsonObject = writer.GetBufferAs<JObject>();

            Assert.IsNotNull(jsonObject);
            this.ValidateSendRequest(jsonObject, templateId, recipientAddress, null);
        }

        [TestMethod]
        public void WriteJson_WithData_IncludesAllData()
        {
            var templateId = TestHelper.GetUniqueId();
            var recipientAddress = TestHelper.GetUniqueId();
            var data = TestHelper.GetRandomDictionary();
            var request = new SendRequest { TemplateId = templateId, RecipientAddress = recipientAddress, Data = data };
            var writer = BufferedJsonStringWriter.Create();
            var serializer = new JsonSerializer();
            var converter = new SendRequestConverter();

            converter.WriteJson(writer, request, serializer);
            var jsonObject = writer.GetBufferAs<JObject>();

            Assert.IsNotNull(jsonObject);
            this.ValidateSendRequest(jsonObject, templateId, recipientAddress, data);
        }
    }
}
