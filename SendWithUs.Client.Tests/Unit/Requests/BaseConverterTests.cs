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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;
    using System;

    [TestClass]
    public class BaseConverterTests
    {
        public class ExampleSubject { }

        public class ExampleSubjectSubtype : ExampleSubject { }

        public class SomeOtherType { }

        [TestMethod]
        public void CanRead_Getter_ReturnsFalse()
        {
            // Arrange
            var converter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            // Act 
            var canRead = converter.Object.CanRead;

            // Assert
            Assert.IsFalse(canRead);
        }

        [TestMethod]
        public void CanWrite_Getter_ReturnsTrue()
        {
            // Arrange
            var converter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            // Act 
            var canRead = converter.Object.CanWrite;

            // Assert
            Assert.IsTrue(canRead);
        }

        [TestMethod]
        public void CanConvert_MatchingSubjectType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(ExampleSubject);
            var converter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            // Act
            var canConvert = converter.Object.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_SubjectSubtype_ReturnsTrue()
        {
            // Arrange
            var type = typeof(ExampleSubjectSubtype);
            var converter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            // Act
            var canConvert = converter.Object.CanConvert(type);

            // Assert
            Assert.IsTrue(canConvert);
        }

        [TestMethod]
        public void CanConvert_NonSubjectType_ReturnsFalse()
        {
            // Arrange
            var type = typeof(SomeOtherType);
            var converter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            // Act
            var canConvert = converter.Object.CanConvert(type);

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
            var converter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            // Act
            var exception = TestHelper.CaptureException(() => converter.Object.ReadJson(reader, request.GetType(), request, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(NotSupportedException));
        }
        
        [TestMethod]
        public void WriteJson_NullWriter_Throws()
        {
            // Arrange
            var writer = null as JsonWriter;
            var serializer = new Mock<JsonSerializer>().Object;
            var value = new object();
            var mockConverter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            // Should not reach this call to our WriteJson overload.
            mockConverter.Setup(c => c.WriteJson(It.IsAny<JsonWriter>(), It.IsAny<ExampleSubject>(), It.IsAny<SerializerProxy>()))
                .Throws<AssertFailedException>();

            // Act
            var exception = TestHelper.CaptureException(() => mockConverter.Object.WriteJson(writer, value, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void WriteJson_NullSerializer_Throws()
        {
            // Arrange
            var writer = new Mock<JsonWriter>().Object;
            var serializer = null as JsonSerializer;
            var value = new object();
            var mockConverter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            // Should not reach this call to our WriteJson overload.
            mockConverter.Setup(c => c.WriteJson(It.IsAny<JsonWriter>(), It.IsAny<ExampleSubject>(), It.IsAny<SerializerProxy>()))
                .Throws<AssertFailedException>();

            // Act
            var exception = TestHelper.CaptureException(() => mockConverter.Object.WriteJson(writer, value, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void WriteJson_NonSubjectType_Throws()
        {
            // Arrange
            var writer = new Mock<JsonWriter>().Object;
            var serializer = new Mock<JsonSerializer>().Object;
            var value = new Mock<SomeOtherType>().Object;
            var mockConverter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            // Should not reach this call to our WriteJson overload.
            mockConverter.Setup(c => c.WriteJson(It.IsAny<JsonWriter>(), It.IsAny<ExampleSubject>(), It.IsAny<SerializerProxy>()))
                .Throws<AssertFailedException>();

            // Act
            var exception = TestHelper.CaptureException(() => mockConverter.Object.WriteJson(writer, value, serializer));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));
            Assert.AreEqual(nameof(value), ((ArgumentException)exception).ParamName);
        }

        [TestMethod]
        public void WriteJson_Normally_CallsWriteJsonOverload()
        {
            // Arrange
            var writer = new Mock<JsonWriter>().Object;
            var serializer = new Mock<JsonSerializer>().Object;
            var subject = new Mock<ExampleSubject>().Object;
            var mockConverter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };
            
            mockConverter.Setup(c => c.WriteJson(It.IsAny<JsonWriter>(), It.IsAny<ExampleSubject>(), It.IsAny<SerializerProxy>()));

            // Act
            mockConverter.Object.WriteJson(writer, subject, serializer);

            // Assert
            mockConverter.Verify(c => c.WriteJson(writer, subject, It.IsAny<SerializerProxy>()), Times.Once);
        }

        [TestMethod]
        public void WriteProperty_NullValueAndIsOptional_DoesNothing()
        {
            // Arrange
            var propName = "platypus";
            var propValue = null as Object;
            var isOptional = true;
            var mockWriter = new Mock<JsonWriter>();
            var mockSerializer = new Mock<SerializerProxy>(null);
            var mockConverter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            mockWriter.Setup(w => w.WritePropertyName(It.IsAny<string>()));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<JsonWriter>(), It.IsAny<string>()));

            // Act
            mockConverter.Object.WriteProperty(mockWriter.Object, mockSerializer.Object, propName, propValue, isOptional);

            // Assert
            mockWriter.Verify(w => w.WritePropertyName(It.IsAny<string>()), Times.Never);
            mockSerializer.Verify(s => s.Serialize(It.IsAny<JsonWriter>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void WriteProperty_NonNullValue_SerializesNameAndValue()
        {
            // Arrange
            var propName = "lebowski";
            var propValue = new object();
            var isOptional = true;
            var mockWriter = new Mock<JsonWriter>();
            var mockSerializer = new Mock<SerializerProxy>(null);
            var mockConverter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            mockWriter.Setup(w => w.WritePropertyName(It.IsAny<string>()));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<JsonWriter>(), It.IsAny<string>()));

            // Act
            mockConverter.Object.WriteProperty(mockWriter.Object, mockSerializer.Object, propName, propValue, isOptional);

            // Assert
            mockWriter.Verify(w => w.WritePropertyName(propName), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.IsAny<JsonWriter>(), propValue), Times.Once);
        }

        [TestMethod]
        public void WriteProperty_EmptyStringValueAndIsOptional_DoesNothing()
        {
            // Arrange
            var propName = "platypus";
            var propValue = String.Empty;
            var isOptional = true;
            var mockWriter = new Mock<JsonWriter>();
            var mockSerializer = new Mock<SerializerProxy>(null);
            var mockConverter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            mockWriter.Setup(w => w.WritePropertyName(It.IsAny<string>()));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<JsonWriter>(), It.IsAny<string>()));

            // Act
            mockConverter.Object.WriteProperty(mockWriter.Object, mockSerializer.Object, propName, propValue, isOptional);

            // Assert
            mockWriter.Verify(w => w.WritePropertyName(It.IsAny<string>()), Times.Never);
            mockSerializer.Verify(s => s.Serialize(It.IsAny<JsonWriter>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void WriteProperty_NonEmptyStringValue_SerializesNameAndValue()
        {
            // Arrange
            var propName = "lebowski";
            var propValue = "dude";
            var isOptional = true;
            var mockWriter = new Mock<JsonWriter>();
            var mockSerializer = new Mock<SerializerProxy>(null);
            var mockConverter = new Mock<BaseConverter<ExampleSubject>>() { CallBase = true };

            mockWriter.Setup(w => w.WritePropertyName(It.IsAny<string>()));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<JsonWriter>(), It.IsAny<string>()));

            // Act
            mockConverter.Object.WriteProperty(mockWriter.Object, mockSerializer.Object, propName, propValue, isOptional);

            // Assert
            mockWriter.Verify(w => w.WritePropertyName(propName), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.IsAny<JsonWriter>(), propValue), Times.Once);
        }
    }
}
