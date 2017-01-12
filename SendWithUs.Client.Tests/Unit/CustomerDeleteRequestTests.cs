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
    public class CustomerDeleteRequestTests
    {
        [TestMethod]
        public void IsValid_Getter_CallsValidate()
        {
            // Arrange
            var request = new Mock<CustomerDeleteRequest>() { CallBase = true };

            // Act
            var isValid = request.Object.IsValid;

            // Assert
            request.Verify(r => r.Validate(), Times.Once);
        }

        [TestMethod]
        public void GetResponseType_Always_ReturnsCustomerDeleteResponseType()
        {
            // Arrange
            var request = new CustomerDeleteRequest();

            // Act
            var responseType = request.GetResponseType();

            // Assert
            Assert.AreEqual(typeof(CustomerDeleteResponse), responseType);
        }

        [TestMethod]
        public void Validate_Always_ReturnsSelf()
        {
            // Arrange
            var email = TestHelper.GetUniqueId();
            var request = new CustomerDeleteRequest(email);

            // Act
            var self = request.Validate();

            // Assert
            Assert.AreSame(request, self);
        }

        [TestMethod]
        public void Validate_Normally_ReturnsThis()
        {
            // Arrange
            var email = TestHelper.GetUniqueId();
            var request = new Mock<CustomerDeleteRequest>() { CallBase = true };

            request.SetupGet(r => r.Email).Returns(email);

            // Act
            var self = request.Object.Validate();

            // Assert
            Assert.AreSame(request.Object, self);
        }

        [TestMethod]
        public void ValidateBool_EmptyEmail_Throws()
        {
            // Arrange
            var email = String.Empty;
            var request = new Mock<CustomerDeleteRequest>() { CallBase = true };

            request.SetupGet(r => r.Email).Returns(email);

            // Act
            var exception = TestHelper.CaptureException(() => request.Object.Validate());

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ValidationException));
            var invalid = exception as ValidationException;
            Assert.AreEqual(ValidationFailureMode.MissingCustomerAddress, invalid.FailureMode);
        }

    }
}
