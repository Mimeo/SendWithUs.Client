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
    using System.Collections.Generic;

    [TestClass]
    public class BaseRequestTests
    {
        [TestMethod]
        public void Validate_HasMissingRequiredProperties_Throws()
        {
            // Arrange
            var missingRequiredProperties = new List<string> { "argyle" };
            var mockRequest = new Mock<BaseRequest>() { CallBase = true };

            mockRequest.Setup(r => r.GetMissingRequiredProperties())
                .Returns(missingRequiredProperties);

            // Act
            var exception = TestHelper.CaptureException(() => mockRequest.Object.Validate());

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ValidationException));
        }

        [TestMethod]
        public void Validate_NoMissingRequiredProperties_ReturnsSelf()
        {
            // Arrange
            var missingRequiredProperties = new List<string>();
            var mockRequest = new Mock<BaseRequest>() { CallBase = true };

            mockRequest.Setup(r => r.GetMissingRequiredProperties())
                .Returns(missingRequiredProperties);

            // Act
            var result = mockRequest.Object.Validate();

            // Assert
            Assert.AreSame(mockRequest.Object, result);
        }

        [TestMethod]
        public void IsValid_ValidateThrows_ReturnsFalse()
        {
            // Arrange
            var exception = new ValidationException(null);
            var mockRequest = new Mock<BaseRequest>() { CallBase = true };

            mockRequest.Setup(r => r.Validate())
                .Throws(exception);

            // Act
            var result = mockRequest.Object.IsValid;

            // Assert
            mockRequest.Verify(r => r.Validate(), Times.Once);
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void IsValid_ValidateDoesNotThrow_ReturnsTrue()
        {
            // Arrange
            var mockRequest = new Mock<BaseRequest>() { CallBase = true };

            mockRequest.Setup(r => r.Validate())
                .Returns(mockRequest.Object);

            // Act
            var result = mockRequest.Object.IsValid;

            // Assert
            mockRequest.Verify(r => r.Validate(), Times.Once);
            Assert.IsTrue(result);
        }
    }
}
