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
    using System;
    using System.Linq;
    using Xunit;

    public class SendRequestTests
    {
        [Fact]
        public void IsValid_Getter_CallsValidate()
        {
            // Arrange
            var request = new Mock<SendRequest>() { CallBase = true };

            // Act
            var isValid = request.Object.IsValid;

            // Assert
            request.Verify(r => r.Validate(), Times.Once);
        }

        [Fact]
        public void GetResponseType_Always_ReturnsSendResponseType()
        {
            // Arrange
            var request = new SendRequest();

            // Act
            var responseType = request.GetResponseType();

            // Assert
            Assert.Equal(typeof(SendResponse), responseType);
        }

        [Fact]
        public void Validate_Always_ReturnsSelf()
        {
            // Arrange
            var templateId = TestHelper.GetUniqueId();
            var recipientAddress = TestHelper.GetUniqueId();
            var request = new SendRequest { TemplateId = templateId, RecipientAddress = recipientAddress };

            // Act
            var self = request.Validate();

            // Assert
            Assert.Same(request, self);
        }

        [Fact]
        public void Validate_Normally_ReturnsThis()
        {
            // Arrange
            var templateId = TestHelper.GetUniqueId();
            var recipientAddress = TestHelper.GetUniqueId();
            var request = new Mock<SendRequest>() { CallBase = true };

            request.SetupGet(r => r.TemplateId).Returns(templateId);
            request.SetupGet(r => r.RecipientAddress).Returns(recipientAddress);

            // Act
            var self = request.Object.Validate();

            // Assert
            Assert.Same(request.Object, self);
        }

        [Fact]
        public void Validate_EmptyTemplateId_Throws()
        {
            // Arrange
            var templateId = String.Empty;
            var recipientAddress = TestHelper.GetUniqueId();
            var request = new Mock<SendRequest>() { CallBase = true };

            request.SetupGet(r => r.TemplateId).Returns(templateId);
            request.SetupGet(r => r.RecipientAddress).Returns(recipientAddress);

            // Act
            var exception = TestHelper.CaptureException(() => request.Object.Validate());

            // Assert
            Assert.IsType(typeof(ValidationException), exception);
            var invalid = exception as ValidationException;
            Assert.Equal(1, invalid.MissingRequiredProperties.Count());
            Assert.Equal("TemplateId", invalid.MissingRequiredProperties.First());
        }

        [Fact]
        public void Validate_EmptyRecipientAddress_Throws()
        {
            // Arrange
            var templateId = TestHelper.GetUniqueId();
            var recipientAddress = String.Empty;
            var request = new Mock<SendRequest>() { CallBase = true };

            request.SetupGet(r => r.TemplateId).Returns(templateId);
            request.SetupGet(r => r.RecipientAddress).Returns(recipientAddress);

            // Act
            var exception = TestHelper.CaptureException(() => request.Object.Validate());

            // Assert
            Assert.IsType(typeof(ValidationException), exception);
            var invalid = exception as ValidationException;
            Assert.Equal(1, invalid.MissingRequiredProperties.Count());
            Assert.Equal("RecipientAddress", invalid.MissingRequiredProperties.First());
        }
    }
}
