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
    using Moq;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Names = BatchRequestConverter.PropertyNames;

    public class BatchRequestConverterTests
    {
        [Fact]
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
        
        [Fact]
        public void WriteJson_Normally_SerializesRequestItems()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var count = 7;
            var items = TestHelper.Generate(count, i => new Mock<IRequest>().Object);
            var request = new Mock<BatchRequest>(items);
            var converter = new Mock<BatchRequestConverter> { CallBase = true };

            request.Setup(r => r.GetEnumerator()).Returns(items.GetEnumerator());

            // Act
            converter.Object.WriteJson(writer.Object, request.Object, serializer.Object);

            // Assert
            converter.Verify(c => c.WriteWrapper(writer.Object, serializer.Object, It.IsAny<IRequest>()), Times.Exactly(count));
        }

        [Fact]
        public void WriteWrapper_Normally_WritesJsonObject()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var item = new Mock<IRequest>();
            var request = new Mock<BatchRequest>(new List<IRequest> { item.Object });
            var converter = new BatchRequestConverter();

            // Act
            converter.WriteWrapper(writer.Object, serializer.Object, request.Object);

            // Assert
            writer.Verify(w => w.WriteStartObject(), Times.Once);
            writer.Verify(w => w.WriteEndObject(), Times.Once);
        }

        [Fact]
        public void WriteWrapper_Normally_SerializesUriPath()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var item = new Mock<IRequest>();
            var request = new Mock<BatchRequest>(new List<IRequest> { item.Object });
            var path = "/werewolf/mummy/frankenstein";
            var converter = new Mock<BatchRequestConverter>() { CallBase = true };
            
            request.Setup(r => r.GetUriPath()).Returns(path);
            converter.Setup(c => c.WriteProperty(writer.Object, serializer.Object, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));

            // Act
            converter.Object.WriteWrapper(writer.Object, serializer.Object, request.Object);

            // Assert
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Path, path, false), Times.Once);
        }

        [Fact]
        public void WriteWrapper_Normally_SerializesHttpMethod()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var item = new Mock<IRequest>();
            var request = new Mock<BatchRequest>(new List<IRequest> { item.Object });
            var method = "haltandcatchfire";
            var converter = new Mock<BatchRequestConverter>() { CallBase = true };
            
            request.Setup(r => r.GetHttpMethod()).Returns(method);
            converter.Setup(c => c.WriteProperty(writer.Object, serializer.Object, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));

            // Act
            converter.Object.WriteWrapper(writer.Object, serializer.Object, request.Object);

            // Assert
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Method, method, false), Times.Once);
        }

        [Fact]
        public void WriteWrapper_Normally_SerializesBody()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var item = new Mock<IRequest>();
            var request = new Mock<BatchRequest>(new List<IRequest> { item.Object });
            var converter = new Mock<BatchRequestConverter>() { CallBase = true };
            
            converter.Setup(c => c.WriteProperty(writer.Object, serializer.Object, It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()));

            // Act
            converter.Object.WriteWrapper(writer.Object, serializer.Object, request.Object);

            // Assert
            converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, Names.Body, request.Object, false), Times.Once);
        }
    }
}
