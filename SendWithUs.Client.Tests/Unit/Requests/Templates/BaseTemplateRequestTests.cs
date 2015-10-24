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
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Unit", "Requests")]
    public class BaseTemplateRequestTests
    {
        protected static List<string> BaseUriPathSegments = BaseTemplateRequest.BaseUriPath.Split('/').ToList();

        [Fact]
        public void GetUriPath_EmptyTemplateId_ReturnsBaseUriPath()
        {
            // Arrange
            var templateId = String.Empty;
            var requestMock = new Mock<BaseTemplateRequest>() { CallBase = true };

            requestMock.SetupGet(r => r.TemplateId).Returns(templateId);

            // Act
            var result = requestMock.Object.GetUriPath();

            // Assert
            Assert.Equal(BaseTemplateRequest.BaseUriPath, result);
        }

        [Fact]
        public void GetUriPath_EmptyLocale_ReturnsPathEndingWithTemplateId()
        {
            // Arrange
            var templateId = "sold out polaroid hashtag";
            var locale = String.Empty;
            var requestMock = new Mock<BaseTemplateRequest>() { CallBase = true };

            requestMock.SetupGet(r => r.TemplateId).Returns(templateId);
            requestMock.SetupGet(r => r.Locale).Returns(locale);

            // Act
            var result = requestMock.Object.GetUriPath();

            // Assert
            var expected = BaseUriPathSegments.Copy().Append(templateId);
            var actual = result.Split('/').ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetUriPath_WithTemplateIdAndLocale_ReturnsFullPath()
        {
            // Arrange
            var templateId = "sartorial leggings cardigan quinoa";
            var locale = "flannel beard drinking vinegar";
            var requestMock = new Mock<BaseTemplateRequest>() { CallBase = true };

            requestMock.SetupGet(r => r.TemplateId).Returns(templateId);
            requestMock.SetupGet(r => r.Locale).Returns(locale);

            // Act
            var result = requestMock.Object.GetUriPath();

            // Assert
            var expected = BaseUriPathSegments.Copy().Append(templateId, "locales", locale);
            var actual = result.Split('/').ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMissingRequiredProperties_RequiredButEmptyTemplateId_ContainsTemplateId()
        {
            // Arrange
            var templateId = String.Empty;
            var requestMock = new Mock<BaseTemplateRequest>() { CallBase = true };

            requestMock.Setup(r => r.IsTemplateIdRequired()).Returns(true);
            requestMock.SetupGet(r => r.TemplateId).Returns(templateId);

            // Act
            var result = requestMock.Object.GetMissingRequiredProperties().ToList();

            // Assert
            Assert.Contains("TemplateId", result);
        }

        [Fact]
        public void GetMissingRequiredProperties_OptionalAndEmptyTemplateId_ReturnsEmptyCollection()
        {
            // Arrange
            var templateId = String.Empty;
            var requestMock = new Mock<BaseTemplateRequest>() { CallBase = true };

            requestMock.Setup(r => r.IsTemplateIdRequired()).Returns(false);
            requestMock.SetupGet(r => r.TemplateId).Returns(templateId);

            // Act
            var result = requestMock.Object.GetMissingRequiredProperties().ToList();

            // Assert
            Assert.Equal(0, result.Count);
        }
    }
}
