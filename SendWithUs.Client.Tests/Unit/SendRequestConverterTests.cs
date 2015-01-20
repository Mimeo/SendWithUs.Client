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
        public class NonSendRequest { }

        public interface ISendRequestSubtype : ISendRequest { }

        [TestMethod]
        public void CanRead_Getter_ReturnsFalse()
        {
            // Arrange
            var converter = new SendRequestConverter();

            // Act 
            var canRead = converter.CanRead;

            // Assert
            Assert.IsFalse(canRead);
        }

        [TestMethod]
        public void CanWrite_Getter_ReturnsTrue()
        {
            // Arrange
            var converter = new SendRequestConverter();

            // Act 
            var canWrite = converter.CanWrite;

            // Assert
            Assert.IsTrue(canWrite);
        }

        [TestMethod]
        public void CanConvert_ISendRequestType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(ISendRequest);
            var converter = new SendRequestConverter();

            // Act
            var canConvert = converter.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_ISendRequestSubtype_ReturnsTrue()
        {
            // Arrange
            var type = typeof(ISendRequestSubtype);
            var converter = new SendRequestConverter();

            // Act
            var canConvert = converter.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_NonISendRequestType_ReturnsFalse()
        {
            // Arrange
            var type = typeof(NonSendRequest);
            var converter = new SendRequestConverter();

            // Act
            var canConvert = converter.CanConvert(type);

            // Assert
            Assert.IsFalse(canConvert);
        }

        [TestMethod]
        public void ReadJson_Always_Throws()
        {
            // Arrange
            var reader = new Mock<JsonReader>().Object;
            var serializer = new Mock<JsonSerializer>().Object;
            var request = new Mock<ISendRequest>().Object;
            var converter = new SendRequestConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.ReadJson(reader, request.GetType(), request, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(NotImplementedException));
        }

        [TestMethod]
        public void WriteJson_NullWriter_Throws()
        {
            // Arrange
            var serializer = new Mock<SerializerProxy>(null).Object;
            var request = new Mock<ISendRequest>().Object;
            var converter = new SendRequestConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.WriteJson(null, request, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void WriteJson_NullSerializer_Throws()
        {
            // Arrange
            var writer = new Mock<JsonWriter>().Object;
            var serializer = null as SerializerProxy;
            var request = new Mock<ISendRequest>().Object;
            var converter = new SendRequestConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.WriteJson(writer, request, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void WriteJson_NonISendRequestValue_Throws()
        {
            // Arrange
            var writer = new Mock<JsonWriter>().Object;
            var serializer = new Mock<SerializerProxy>(null).Object;
            var value = new NonSendRequest();
            var converter = new SendRequestConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.WriteJson(writer, value, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));
        }

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

            request.SetupGet(r => r.TemplateId).Returns(templateId);
            request.SetupGet(r => r.ProviderId).Returns(providerId);
            request.SetupGet(r => r.TemplateVersionId).Returns(templateVersionId);
            
            // Act
            converter.Object.WriteJson(writer.Object, request.Object, serializer.Object);

            // Assert
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.TemplateId, templateId, false), Times.Once);
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.ProviderId, providerId, true), Times.Once);
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.TemplateVersionId, templateVersionId, true), Times.Once);
            converter.Verify(c => c.WriteEmailData(writer.Object, serializer.Object, request.Object), Times.Once);
            converter.Verify(c => c.WritePrimaryRecipient(writer.Object, serializer.Object, request.Object), Times.Once);
            converter.Verify(c => c.WriteCcRecipients(writer.Object, serializer.Object, request.Object), Times.Once);
            converter.Verify(c => c.WriteBccRecipients(writer.Object, serializer.Object, request.Object), Times.Once);
            converter.Verify(c => c.WriteTags(writer.Object, serializer.Object, request.Object), Times.Once);
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
        public void WriteEmailData_NullData_DoesNotWrite()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<ISendRequest>();
            var converter = new SendRequestConverter();

            request.SetupGet(r => r.Data).Returns(null);

            // Act
            converter.WriteEmailData(writer.Object, serializer.Object, request.Object);

            // Assert
            writer.Verify(w => w.WritePropertyName(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void WriteEmailData_Normally_WritesPropertyName()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<ISendRequest>();
            var converter = new SendRequestConverter();
            var data = true; //TestHelper.GetRandomData();

            request.SetupGet(r => r.Data).Returns(data);

            // Act
            converter.WriteEmailData(writer.Object, serializer.Object, request.Object);

            // Assert
            writer.Verify(w => w.WritePropertyName(Names.Data), Times.Once);
        }

        [TestMethod]
        public void WriteEmailData_Normally_SerializesData()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<ISendRequest>();
            var converter = new SendRequestConverter();
            var data = true; //TestHelper.GetRandomData();

            request.SetupGet(r => r.Data).Returns(data);

            // Act
            converter.WriteEmailData(writer.Object, serializer.Object, request.Object);

            // Assert
            serializer.Verify(s => s.Serialize(writer.Object, data), Times.Once);
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
