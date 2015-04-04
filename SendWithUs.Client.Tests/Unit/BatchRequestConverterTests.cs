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
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;
    using Names = BatchRequestConverter.PropertyNames;

    [TestClass]
    public class BatchRequestConverterTests
    {
        internal class BatchRequestSubtype : BatchRequest
        {
            internal BatchRequestSubtype() : base(null)
            { }
        }

        public class NonBatchRequest
        { }

        [TestMethod]
        public void CanRead_Getter_ReturnsFalse()
        {
            // Arrange
            var converter = new BatchRequestConverter();

            // Act 
            var canRead = converter.CanRead;

            // Assert
            Assert.IsFalse(canRead);
        }

        [TestMethod]
        public void CanWrite_Getter_ReturnsTrue()
        {
            // Arrange
            var converter = new BatchRequestConverter();

            // Act 
            var canRead = converter.CanWrite;

            // Assert
            Assert.IsTrue(canRead);
        }

        [TestMethod]
        public void CanConvert_BatchRequestType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(BatchRequest);
            var converter = new BatchRequestConverter();

            // Act
            var canConvert = converter.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_BatchRequestWrapperSubtype_ReturnsTrue()
        {
            // Arrange
            var type = typeof(BatchRequestSubtype);
            var converter = new BatchRequestConverter();

            // Act
            var canConvert = converter.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_NonBatchRequestType_ReturnsFalse()
        {
            // Arrange
            var type = typeof(NonBatchRequest);
            var converter = new BatchRequestConverter();

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
            var request = new BatchRequest(null);
            var converter = new BatchRequestConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.ReadJson(reader, request.GetType(), request, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(NotImplementedException));
        }

        //[TestMethod]
        //public void WriteJson_NullWriter_Throws()
        //{
        //    // Arrange
        //    var serializer = new Mock<JsonSerializer>().Object;
        //    var inner = new Mock<IRequest>();
        //    var wrapper = new BatchRequestWrapper(inner.Object);
        //    var converter = new BatchRequestWrapperConverter();

        //    // Act
        //    var exception = TestHelper.CaptureException(() => converter.WriteJson(null, wrapper, serializer));

        //    // Assert
        //    Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        //}

        //[TestMethod]
        //public void WriteJson_NullSerializer_Throws()
        //{
        //    // Arrange
        //    var writer = new Mock<JsonWriter>().Object;
        //    var inner = new Mock<IRequest>();
        //    var wrapper = new BatchRequestWrapper(inner.Object);
        //    var converter = new BatchRequestWrapperConverter();

        //    // Act
        //    var exception = TestHelper.CaptureException(() => converter.WriteJson(writer, wrapper, null));

        //    // Assert
        //    Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        //}

        [TestMethod]
        public void WriteJson_NonBatchRequestValue_Throws()
        {
            // Arrange
            var writer = new Mock<JsonWriter>().Object;
            var serializer = new Mock<SerializerProxy>(null).Object;
            var value = new NonBatchRequest();
            var converter = new BatchRequestConverter();

            // Act
            var exception = TestHelper.CaptureException(() => converter.WriteJson(writer, value, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));
        }

        [TestMethod]
        public void WriteJson_Normally_WritesJsonArray()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<BatchRequest>(null);
            var enumerator = Enumerable.Empty<IRequest>().GetEnumerator();
            var converter = new BatchRequestConverter();

            request.Setup(r => r.GetEnumerator()).Returns(enumerator);

            // Act
            converter.WriteJson(writer.Object, request.Object, serializer.Object);

            // Assert
            writer.Verify(w => w.WriteStartArray(), Times.Once);
            writer.Verify(w => w.WriteEndArray(), Times.Once);
        }


        [TestMethod]
        public void WriteJson_Normally_SerializesRequestItems()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var count = TestHelper.GetRandomInteger(1, 10);
            var items = TestHelper.Generate(count, i => new Mock<IRequest>().Object);
            var request = new Mock<BatchRequest>(items);
            var converter = new Mock<BatchRequestConverter> { CallBase = true };

            request.Setup(r => r.GetEnumerator()).Returns(items.GetEnumerator());

            // Act
            converter.Object.WriteJson(writer.Object, request.Object, serializer.Object);

            // Assert
            converter.Verify(c => c.WriteWrapper(writer.Object, serializer.Object, It.IsAny<IRequest>()), Times.Exactly(count));
        }

        [TestMethod]
        public void WriteWrapper_Normally_SerializesUriPath()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var item = new Mock<IRequest>();
            var request = new Mock<BatchRequest>(new List<IRequest> { item.Object });
            var path = TestHelper.GetUniqueId();
            var converter = new BatchRequestConverter();

            item.SetupAllProperties();
            request.Setup(r => r.GetUriPath()).Returns(path);

            // Act
            converter.WriteWrapper(writer.Object, serializer.Object, request.Object);

            // Assert
            writer.Verify(w => w.WritePropertyName(Names.Path), Times.Once);
            writer.Verify(s => s.WriteValue(path), Times.Once);
        }

        [TestMethod]
        public void WriteWrapper_Normally_SerializesHttpMethod()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var item = new Mock<IRequest>();
            var request = new Mock<BatchRequest>(new List<IRequest> { item.Object });
            var method = TestHelper.GetUniqueId();
            var converter = new BatchRequestConverter();

            item.SetupAllProperties();
            request.Setup(r => r.GetHttpMethod()).Returns(method);

            // Act
            converter.WriteWrapper(writer.Object, serializer.Object, request.Object);

            // Assert
            writer.Verify(w => w.WritePropertyName(Names.Method), Times.Once);
            writer.Verify(w => w.WriteValue(method), Times.Once);
        }

        [TestMethod]
        public void WriteWrapper_Normally_SerializesBody()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var item = new Mock<IRequest>();
            var request = new Mock<BatchRequest>(new List<IRequest> { item.Object });
            var converter = new BatchRequestConverter();

            item.SetupAllProperties();

            // Act
            converter.WriteWrapper(writer.Object, serializer.Object, request.Object);

            // Assert
            writer.Verify(w => w.WritePropertyName(Names.Body), Times.Once);
            serializer.Verify(s => s.Serialize(writer.Object, request.Object), Times.Once);
        }
    }
}
