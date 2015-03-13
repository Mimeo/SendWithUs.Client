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
    using Names = DripCampaignActivateRequestConverter.PropertyNames;

    [TestClass]
    public class DripCampaignActivateRequestConverterTests
    {
        public class NonDripCampaignActivateRequest { }

        public interface IDripCampaignActivateRequestSubtype : IDripCampaignActivateRequest { }

        [TestMethod]
        public void CanRead_Getter_ReturnsFalse()
        {
            // Arrange
            var converter = new DripCampaignActivateRequestConverter();

            // Act 
            var canRead = converter.CanRead;

            // Assert
            Assert.IsFalse(canRead);
        }

        [TestMethod]
        public void CanWrite_Getter_ReturnsTrue()
        {
            // Arrange
            var converter = new DripCampaignActivateRequestConverter();

            // Act 
            var canWrite = converter.CanWrite;

            // Assert
            Assert.IsTrue(canWrite);
        }

        [TestMethod]
        public void CanConvert_IDripCampaignActivateRequestType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(IDripCampaignActivateRequest);
            var converter = new DripCampaignActivateRequestConverter();

            // Act
            var canConvert = converter.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_IDripCampaignActivateRequestSubtype_ReturnsTrue()
        {
            // Arrange
            var type = typeof(IDripCampaignActivateRequestSubtype);
            var converter = new DripCampaignActivateRequestConverter();

            // Act
            var canConvert = converter.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_NonIDripCampaignActivateRequestType_ReturnsFalse()
        {
            // Arrange
            var type = typeof(NonDripCampaignActivateRequest);
            var converter = new DripCampaignActivateRequestConverter();

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
            var request = new Mock<IDripCampaignActivateRequest>().Object;
            var converter = new DripCampaignActivateRequestConverter();

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
            var request = new Mock<IDripCampaignActivateRequest>().Object;
            var converter = new DripCampaignActivateRequestConverter();

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
            var request = new Mock<IDripCampaignActivateRequest>().Object;
            var converter = new DripCampaignActivateRequestConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.WriteJson(writer, request, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void WriteJson_NonIDripCampaignActivateRequestValue_Throws()
        {
            // Arrange
            var writer = new Mock<JsonWriter>().Object;
            var serializer = new Mock<SerializerProxy>(null).Object;
            var value = new NonDripCampaignActivateRequest();
            var converter = new DripCampaignActivateRequestConverter();

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
            var request = new Mock<IDripCampaignActivateRequest>();
            var converter = new DripCampaignActivateRequestConverter();

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
            var request = new Mock<IDripCampaignActivateRequest>();
            var converter = new Mock<DripCampaignActivateRequestConverter>() { CallBase = true };
            var campaignId = TestHelper.GetUniqueId();
            var providerId = TestHelper.GetUniqueId();
            var language = TestHelper.GetUniqueId();
            var data = (object)null;
            var tags = (IEnumerable<string>)null;

            request.SetupGet(r => r.CampaignId).Returns(campaignId);
            request.SetupGet(r => r.ProviderId).Returns(providerId);
            request.SetupGet(r => r.Locale).Returns(language);
            request.SetupGet(r => r.Data).Returns(data);
            request.SetupGet(r => r.Tags).Returns(tags);

            // Act
            converter.Object.WriteJson(writer.Object, request.Object, serializer.Object);

            // Assert
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.ProviderId, providerId, true), Times.Once);
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Locale, language, true), Times.Once);
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Data, data, true), Times.Once);
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Tags, tags, true), Times.Once);
            converter.Verify(c => c.WritePrimaryRecipient(writer.Object, serializer.Object, request.Object), Times.Once);
            converter.Verify(c => c.WriteCcRecipients(writer.Object, serializer.Object, request.Object), Times.Once);
            converter.Verify(c => c.WriteBccRecipients(writer.Object, serializer.Object, request.Object), Times.Once);
        }

        [TestMethod]
        public void WritePrimaryRecipient_Normally_WritesObjectValuedProperty()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<IDripCampaignActivateRequest>();
            var converter = new DripCampaignActivateRequestConverter();

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
            var request = new Mock<IDripCampaignActivateRequest>();
            var converter = new Mock<DripCampaignActivateRequestConverter>() { CallBase = true };
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
            var request = new Mock<IDripCampaignActivateRequest>();
            var converter = new Mock<DripCampaignActivateRequestConverter>() { CallBase = true };
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
            var request = new Mock<IDripCampaignActivateRequest>();
            var converter = new Mock<DripCampaignActivateRequestConverter>() { CallBase = true };
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
            var converter = new DripCampaignActivateRequestConverter();
            var name = TestHelper.GetUniqueId();

            // Act
            converter.WriteRecipientsList(writer.Object, serializer.Object, name, null);

            // Assert
            writer.Verify(w => w.WritePropertyName(name), Times.Never);
        }
    }
}
