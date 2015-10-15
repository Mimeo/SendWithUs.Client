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

namespace SendWithUs.Client.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;
    using SendWithUs.Client;
    using Names = SendRequestConverter.PropertyNames;

    [TestClass]
    public class SendRequestConverterTests
    {
        [TestMethod]
        public void WriteJson_Normally_WritesObject()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<ISendRequest>();
            var converter = new SendRequestConverter();

            // Act
            converter.WriteJson(writer.Object, request.Object, serializer.Object);

            // Assert
            writer.Verify(w => w.WriteStartObject(), Times.AtLeastOnce);
            writer.Verify(w => w.WriteEndObject(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void WriteJson_Normally_CallsHelpers()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<ISendRequest>();
            var converter = new Mock<SendRequestConverter>() { CallBase = true };
            var templateId = TestHelper.GetUniqueId();
            var providerId = TestHelper.GetUniqueId();
            var templateVersionId = TestHelper.GetUniqueId();
            var language = TestHelper.GetUniqueId();
            var data = (object)null;
            var tags = (IEnumerable<string>)null;
            var headers = (IDictionary<string, string>)null;

            request.SetupGet(r => r.TemplateId).Returns(templateId);
            request.SetupGet(r => r.ProviderId).Returns(providerId);
            request.SetupGet(r => r.TemplateVersionId).Returns(templateVersionId);
            request.SetupGet(r => r.Locale).Returns(language);
            request.SetupGet(r => r.Data).Returns(data);
            request.SetupGet(r => r.Tags).Returns(tags);
            request.SetupGet(r => r.Headers).Returns(headers);
            
            // Act
            converter.Object.WriteJson(writer.Object, request.Object, serializer.Object);

            // Assert
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.TemplateId, templateId, false), Times.Once);
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.ProviderId, providerId, true), Times.Once);
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.TemplateVersionId, templateVersionId, true), Times.Once);
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Locale, language, true), Times.Once);
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Data, data, true), Times.Once);
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Tags, tags, true), Times.Once);
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Headers, headers, true), Times.Once);
            converter.Verify(c => c.WritePrimaryRecipient(writer.Object, serializer.Object, request.Object), Times.Once);
            converter.Verify(c => c.WriteCcRecipients(writer.Object, serializer.Object, request.Object), Times.Once);
            converter.Verify(c => c.WriteBccRecipients(writer.Object, serializer.Object, request.Object), Times.Once);
            converter.Verify(c => c.WriteInlineAttachment(writer.Object, serializer.Object, request.Object), Times.Once);
            converter.Verify(c => c.WriteFileAttachments(writer.Object, serializer.Object, request.Object), Times.Once);
        }

        [TestMethod]
        public void WritePrimaryRecipient_Normally_WritesObjectValuedProperty()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<ISendRequest>();
            var converter = new SendRequestConverter();

            // Act
            converter.WritePrimaryRecipient(writer.Object, serializer.Object, request.Object);

            // Assert
            writer.Verify(w => w.WritePropertyName(Names.Recipient));
            writer.Verify(w => w.WriteStartObject());
            writer.Verify(w => w.WriteEndObject());
        }

        [TestMethod]
        public void WritePrimaryRecipient_Normally_CallsHelpers()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<ISendRequest>();
            var converter = new Mock<SendRequestConverter>() { CallBase = true };
            var recipientName = TestHelper.GetUniqueId();
            var recipientAddress = TestHelper.GetUniqueId();

            request.SetupGet(r => r.RecipientName).Returns(recipientName);
            request.SetupGet(r => r.RecipientAddress).Returns(recipientAddress);

            // Act
            converter.Object.WritePrimaryRecipient(writer.Object, serializer.Object, request.Object);

            // Assert
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Name, recipientName, true));
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Address, recipientAddress, false));
        }

        [TestMethod]
        public void WriteCcRecipients_Always_CallsWriteRecipientsList()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<ISendRequest>();
            var converter = new Mock<SendRequestConverter>() { CallBase = true };
            var recipients = new List<string>();

            request.SetupGet(r => r.CopyTo).Returns(recipients);

            // Act
            converter.Object.WriteCcRecipients(writer.Object, serializer.Object, request.Object);

            // Assert
            converter.Verify(c => c.WriteRecipientsList(writer.Object, serializer.Object, Names.CopyTo, recipients));
        }

        [TestMethod]
        public void WriteBccRecipients_Always_CallsWriteRecipientsList()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<ISendRequest>();
            var converter = new Mock<SendRequestConverter>() { CallBase = true };
            var recipients = new List<string>();

            request.SetupGet(r => r.CopyTo).Returns(recipients);

            // Act
            converter.Object.WriteBccRecipients(writer.Object, serializer.Object, request.Object);

            // Assert
            converter.Verify(c => c.WriteRecipientsList(writer.Object, serializer.Object, Names.BlindCopyTo, recipients));
        }

        [TestMethod]
        public void WriteRecipientsList_NullRecipients_DoesNotWrite()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var converter = new SendRequestConverter();
            var name = TestHelper.GetUniqueId();

            // Act
            converter.WriteRecipientsList(writer.Object, serializer.Object, name, null);

            // Assert
            writer.Verify(w => w.WritePropertyName(name), Times.Never);
        }
    }
}
