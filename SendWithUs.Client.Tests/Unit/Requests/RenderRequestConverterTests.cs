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
    using System.Linq.Expressions;
    using Names = RenderRequestConverter.PropertyNames;

    [TestClass]
    public class RenderRequestConverterTests
    {
        [TestMethod]
        public void WriteJson_Normally_WritesJsonObject()
        {
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<IRenderRequest>();
            var converter = new Mock<RenderRequestConverter> { CallBase = true };

            converter.Setup(c => c.WriteProperty(writer.Object, serializer.Object, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
            converter.Setup(c => c.WriteProperty(writer.Object, serializer.Object, It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()));

            // Act
            converter.Object.WriteJson(writer.Object, request.Object, serializer.Object);

            // Assert
            writer.Verify(w => w.WriteStartObject(), Times.Once);
            writer.Verify(w => w.WriteEndObject(), Times.Once);
        }

        [TestMethod]
        public void WriteJson_Normally_WritesTemplateId()
        {
            var propName = Names.TemplateId;
            var propValue = "Chumley";

            this.RunTest_WriteJsonCallsWriteProperty(r => r.TemplateId, propName, propValue, false);
        }

        [TestMethod]
        public void WriteJson_Normally_WritesTemplateVersionId()
        {
            var propName = Names.TemplateVersionId;
            var propValue = "Walrus";

            this.RunTest_WriteJsonCallsWriteProperty(r => r.TemplateVersionId, propName, propValue, true);
        }

        [TestMethod]
        public void WriteJson_Normally_WritesLocale()
        {
            var propName = Names.Locale;
            var propValue = "Megapolis Zoo";

            this.RunTest_WriteJsonCallsWriteProperty(r => r.Locale, propName, propValue, true);
        }

        [TestMethod]
        public void WriteJson_Normally_WritesData()
        {
            var propName = Names.Data;
            var propValue = new object();

            this.RunTest_WriteJsonCallsWriteProperty(r => r.Data, propName, propValue, true);
        }

        protected void RunTest_WriteJsonCallsWriteProperty<TProperty>(Expression<Func<IRenderRequest, TProperty>> getter, string propName, TProperty propValue, bool isOptional)
        { 
            // Arrange
            var writer = new Mock<JsonWriter>();
            var serializer = new Mock<SerializerProxy>(null);
            var request = new Mock<IRenderRequest>();
            var converter = new Mock<RenderRequestConverter> { CallBase = true };

            request.SetupGet(getter).Returns(propValue);
            converter.Setup(c => c.WriteProperty(writer.Object, serializer.Object, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
            converter.Setup(c => c.WriteProperty(writer.Object, serializer.Object, It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()));

            // Act
            converter.Object.WriteJson(writer.Object, request.Object, serializer.Object);

            // Assert
            if (typeof(TProperty) == typeof(string))
            {
                string stringValue = propValue as string;
                converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, propName, stringValue, isOptional), Times.Once);
            }
            else
            {
                converter.Verify(c => c.WriteProperty(writer.Object, serializer.Object, propName, propValue, isOptional), Times.Once);
            }
        }
    }
}
