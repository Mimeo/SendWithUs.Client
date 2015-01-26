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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;
    using Names = BatchRequestWrapperConverter.PropertyNames;

    [TestClass]
    public class BatchRequestWrapperConverterTests
    {
        internal class BatchRequestWrapperSubtype : BatchRequestWrapper
        { 
            internal BatchRequestWrapperSubtype() : base(null)
            { }
        }

        public class NonBatchRequestWrapper
        { }

        [TestMethod]
        public void CanRead_Getter_ReturnsFalse()
        {
            // Arrange
            var converter = new BatchRequestWrapperConverter();

            // Act 
            var canRead = converter.CanRead;

            // Assert
            Assert.IsFalse(canRead);
        }

        [TestMethod]
        public void CanWrite_Getter_ReturnsTrue()
        {
            // Arrange
            var converter = new BatchRequestWrapperConverter();

            // Act 
            var canRead = converter.CanWrite;

            // Assert
            Assert.IsTrue(canRead);
        }

        [TestMethod]
        public void CanConvert_BatchRequestWrapperType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(BatchRequestWrapper);
            var converter = new BatchRequestWrapperConverter();

            // Act
            var canConvert = converter.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_BatchRequestWrapperSubtype_ReturnsTrue()
        {
            // Arrange
            var type = typeof(BatchRequestWrapperSubtype);
            var converter = new BatchRequestWrapperConverter();

            // Act
            var canConvert = converter.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_NonBatchRequestWrapperType_ReturnsFalse()
        {
            // Arrange
            var type = typeof(NonBatchRequestWrapper);
            var converter = new BatchRequestWrapperConverter();

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
            var inner = new Mock<IRequest>();
            var wrapper = new BatchRequestWrapper(inner.Object);
            var converter = new BatchRequestWrapperConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.ReadJson(reader, wrapper.GetType(), wrapper, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(NotImplementedException));
        }

        [TestMethod]
        public void WriteJson_NullWriter_Throws()
        {
            // Arrange
            var serializer = new Mock<JsonSerializer>().Object;
            var inner = new Mock<IRequest>();
            var wrapper = new BatchRequestWrapper(inner.Object);
            var converter = new BatchRequestWrapperConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.WriteJson(null, wrapper, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void WriteJson_NullSerializer_Throws()
        {
            // Arrange
            var writer = new Mock<JsonWriter>().Object;
            var inner = new Mock<IRequest>();
            var wrapper = new BatchRequestWrapper(inner.Object);
            var converter = new BatchRequestWrapperConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.WriteJson(writer, wrapper, null));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void WriteJson_NonBatchRequestWrapperValue_Throws()
        {
            // Arrange
            var writer = new Mock<JsonWriter>().Object;
            var serializer = new Mock<JsonSerializer>().Object;
            var value = new NonBatchRequestWrapper();
            var converter = new BatchRequestWrapperConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.WriteJson(writer, value, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));
        }

        [TestMethod]
        public void WriteJson_Normally_WritesJsonObject()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<JsonSerializer>();
            var inner = new Mock<IRequest>();
            var wrapper = new Mock<BatchRequestWrapper>(inner.Object);
            var converter = new BatchRequestWrapperConverter();

            inner.SetupAllProperties();

            // Act
            converter.WriteJson(writer.Object, wrapper.Object, serializer.Object);

            // Assert
            writer.Verify(w => w.WriteStartObject(), Times.Once);
            writer.Verify(w => w.WriteEndObject(), Times.Once);
        }

        [TestMethod]
        public void WriteJson_Normally_SerializesWrapperPath()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<JsonSerializer>();
            var inner = new Mock<IRequest>();
            var wrapper = new Mock<BatchRequestWrapper>(inner.Object);
            var path = TestHelper.GetUniqueId();
            var converter = new BatchRequestWrapperConverter();

            inner.SetupAllProperties();
            wrapper.SetupGet(w => w.Path).Returns(path);

            // Act
            converter.WriteJson(writer.Object, wrapper.Object, serializer.Object);

            // Assert
            writer.Verify(w => w.WritePropertyName(Names.Path), Times.Once);
            writer.Verify(s => s.WriteValue(wrapper.Object.Path), Times.Once);
        }

        [TestMethod]
        public void WriteJson_Normally_SerializesWrapperMethod()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<JsonSerializer>();
            var inner = new Mock<IRequest>();
            var wrapper = new Mock<BatchRequestWrapper>(inner.Object);
            var method = TestHelper.GetUniqueId();
            var converter = new BatchRequestWrapperConverter();

            inner.SetupAllProperties();
            wrapper.SetupGet(w => w.Method).Returns(method);

            // Act
            converter.WriteJson(writer.Object, wrapper.Object, serializer.Object);

            // Assert
            writer.Verify(w => w.WritePropertyName(Names.Method), Times.Once);
            writer.Verify(s => s.WriteValue(method), Times.Once);
        }

        [TestMethod]
        public void WriteJson_Normally_SerializesWrapperInnerRequest()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var inner = new Mock<IRequest>();
            var wrapper = new Mock<BatchRequestWrapper>(inner.Object);
            var converter = new BatchRequestWrapperConverter();

            wrapper.SetupGet(w => w.InnerRequest).Returns(inner.Object);

            // Act
            converter.WriteJson(writer.Object, wrapper.Object, serializer.Object);

            // Assert
            writer.Verify(w => w.WritePropertyName(Names.InnerRequest), Times.Once);
            serializer.Verify(s => s.Serialize(writer.Object, inner.Object), Times.Once);
        }
    }
}
