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
    using Names = CustomerDeleteRequestConverter.PropertyNames;

    [TestClass]
    public class CustomerDeleteRequestConverterTests
    {
        public class NonCustomerDeleteRequest { }

        public interface ICustomerDeleteRequestSubtype : ICustomerDeleteRequest { }

        [TestMethod]
        public void CanRead_Getter_ReturnsFalse()
        {
            // Arrange
            var converter = new CustomerDeleteRequestConverter();

            // Act 
            var canRead = converter.CanRead;

            // Assert
            Assert.IsFalse(canRead);
        }

        [TestMethod]
        public void CanWrite_Getter_ReturnsTrue()
        {
            // Arrange
            var converter = new CustomerDeleteRequestConverter();

            // Act 
            var canWrite = converter.CanWrite;

            // Assert
            Assert.IsTrue(canWrite);
        }

        [TestMethod]
        public void CanConvert_ICustomerDeleteRequestType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(ICustomerDeleteRequest);
            var converter = new CustomerDeleteRequestConverter();

            // Act
            var canConvert = converter.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_ICustomerDeleteRequestSubtype_ReturnsTrue()
        {
            // Arrange
            var type = typeof(ICustomerDeleteRequestSubtype);
            var converter = new CustomerDeleteRequestConverter();

            // Act
            var canConvert = converter.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_NonICustomerDeleteRequestType_ReturnsFalse()
        {
            // Arrange
            var type = typeof(NonCustomerDeleteRequest);
            var converter = new CustomerDeleteRequestConverter();

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
            var request = new Mock<ICustomerDeleteRequest>().Object;
            var converter = new CustomerDeleteRequestConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.ReadJson(reader, request.GetType(), request, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(NotSupportedException));
        }

        [TestMethod]
        public void WriteJson_NullWriter_Throws()
        {
            // Arrange
            var serializer = new Mock<SerializerProxy>(null).Object;
            var request = new Mock<ICustomerDeleteRequest>().Object;
            var converter = new CustomerDeleteRequestConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.WriteJson(null, request, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(NullReferenceException));
        }

        [TestMethod]
        public void WriteJson_NullSerializer_Throws()
        {
            // Arrange
            var writer = new Mock<JsonWriter>().Object;
            var serializer = null as SerializerProxy;
            var request = new Mock<ICustomerDeleteRequest>().Object;
            var converter = new CustomerDeleteRequestConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.WriteJson(writer, request, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(NullReferenceException));
        }

        [TestMethod]
        public void WriteJson_NonICustomerDeleteRequestValue_Throws()
        {
            // Arrange
            var writer = new Mock<JsonWriter>().Object;
            var serializer = new Mock<SerializerProxy>(null).Object;
            var value = new NonCustomerDeleteRequest();
            var converter = new CustomerDeleteRequestConverter();

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
            var request = new Mock<ICustomerDeleteRequest>();
            var converter = new CustomerDeleteRequestConverter();

            // Act
            converter.WriteJson(writer.Object, request.Object, serializer.Object);

            // Assert
            writer.Verify(w => w.WriteStartObject(), Times.AtLeastOnce);
            writer.Verify(w => w.WriteEndObject(), Times.AtLeastOnce);
        }
    }
}
