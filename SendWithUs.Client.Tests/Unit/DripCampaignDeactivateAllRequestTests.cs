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

    [TestClass]
    public class DripCampaignDeactivateAllRequestTests
    {
        [TestMethod]
        public void IsValid_Getter_CallsValidate()
        {
            // Arrange
            var request = new Mock<DripCampaignDeactivateAllRequest>() { CallBase = true };

            // Act
            var isValid = request.Object.IsValid;

            // Assert
            request.Verify(r => r.Validate(false), Times.Once);
        }

        [TestMethod]
        public void GetResponseType_Always_ReturnsDripCampaignDeactivateAllResponseType()
        {
            // Arrange
            var request = new DripCampaignDeactivateAllRequest();

            // Act
            var responseType = request.GetResponseType();

            // Assert
            Assert.AreEqual(typeof(DripCampaignDeactivateAllResponse), responseType);
        }

        [TestMethod]
        public void Validate_Always_CallsValidateBool()
        {
            // Arrange
            var request = new Mock<DripCampaignDeactivateAllRequest>();

            // Act
            var self = request.Object.Validate();

            // Assert
            request.Verify(r => r.Validate(true), Times.Once);
        }

        [TestMethod]
        public void Validate_Always_ReturnsSelf()
        {
            // Arrange
            var recipientAddress = TestHelper.GetUniqueId();
            var request = new DripCampaignDeactivateAllRequest(recipientAddress);

            // Act
            var self = request.Validate();

            // Assert
            Assert.AreSame(request, self);
        }

        [TestMethod]
        public void ValidateBool_Normally_ReturnsTrue()
        {
            // Arrange
            var recipientAddress = TestHelper.GetUniqueId();
            var request = new Mock<DripCampaignDeactivateAllRequest>() { CallBase = true };

            request.SetupGet(r => r.RecipientAddress).Returns(recipientAddress);

            // Act
            var isValid = request.Object.Validate(false);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateBool_TrueFlagAndEmptyRecipientAddress_Throws()
        {
            // Arrange
            var recipientAddress = String.Empty;
            var request = new Mock<DripCampaignDeactivateAllRequest>() { CallBase = true };

            request.SetupGet(r => r.RecipientAddress).Returns(recipientAddress);

            // Act
            var exception = TestHelper.CaptureException(() => request.Object.Validate(true));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ValidationException));
            var invalid = exception as ValidationException;
            Assert.AreEqual(ValidationFailureMode.MissingRecipientAddress, invalid.FailureMode);
        }

        [TestMethod]
        public void ValidateBool_FalseFlagAndEmptyRecipientAddress_ReturnsFalse()
        {
            // Arrange
            var recipientAddress = String.Empty;
            var request = new Mock<DripCampaignDeactivateAllRequest>() { CallBase = true };

            request.SetupGet(r => r.RecipientAddress).Returns(recipientAddress);

            // Act
            var isValid = request.Object.Validate(false);

            // Assert
            Assert.AreEqual(false, isValid);
        }
    }
}
