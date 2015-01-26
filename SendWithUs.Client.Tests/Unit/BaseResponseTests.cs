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
    using System.Linq;
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class BaseResponseTests
    {
        [TestMethod]
        public void Initialize_Always_SetsStatusCode()
        {
            // Arrange
            var statusCode = HttpStatusCode.OK;
            var response = new Mock<BaseResponse<JToken>>() { CallBase = true };

            // Act
            response.Object.Initialize(statusCode, null);

            // Assert
            response.VerifySet(r => r.StatusCode = statusCode, Times.Once);
            Assert.AreEqual(response.Object.StatusCode, statusCode);
        }

        [TestMethod]
        public void Initialize_SuccessStatusCode_CallsPopulate()
        {
            // Arrange
            var statusCode = HttpStatusCode.OK;
            var jtoken = new JObject();
            var response = new Mock<BaseResponse<JToken>>() { CallBase = true };

            // Act
            response.Object.Initialize(statusCode, jtoken);

            // Assert
            response.Verify(r => r.IsSuccessStatusCode(), Times.Once);
            response.Verify(r => r.Populate(jtoken), Times.Once);
            response.Verify(r => r.SetErrorMessage(It.IsAny<JValue>()), Times.Never);
        }


        [TestMethod]
        public void Initialize_NonSuccessStatusCode_CallsSetErrorMessage()
        {
            // Arrange
            var statusCode = HttpStatusCode.BadRequest;
            var jtoken = new JObject();
            var response = new Mock<BaseResponse<JToken>>() { CallBase = true };

            // Act
            response.Object.Initialize(statusCode, jtoken);

            // Assert
            response.Verify(r => r.IsSuccessStatusCode(), Times.Once);
            response.Verify(r => r.Populate(jtoken), Times.Never);
            response.Verify(r => r.SetErrorMessage(It.IsAny<JValue>()), Times.Once);
        }

        [TestMethod]
        public void IsSuccessStatusCode_StatusCodeInRange200To299_ReturnsTrue()
        {
            // Arrange
            var jtoken = new JObject();
            var response = new Mock<BaseResponse<JToken>>() { CallBase = true };

            // Act
            var result = TestHelper.Generate<bool>(200, 299, (i) =>
            {
                response.Object.StatusCode = (HttpStatusCode)i;
                return response.Object.IsSuccessStatusCode();
            });

            // Assert
            Assert.IsTrue(result.All(v => v == true));
        }

        [TestMethod]
        public void IsSuccessStatusCode_StatusCodeLessThan200_ReturnsFalse()
        {
            // Arrange
            var jtoken = new JObject();
            var response = new Mock<BaseResponse<JToken>>() { CallBase = true };

            // Act
            var result = TestHelper.Generate<bool>(100, 199, (i) =>
            {
                response.Object.StatusCode = (HttpStatusCode)i;
                return response.Object.IsSuccessStatusCode();
            }).ToList();

            // Assert
            Assert.IsTrue(result.All(v => v == false));
        }

        [TestMethod]
        public void IsSuccessStatusCode_StatusCodeGreaterThan299_ReturnsFalse()
        {
            // Arrange
            var jtoken = new JObject();
            var response = new Mock<BaseResponse<JToken>>() { CallBase = true };

            // Act
            var result = TestHelper.Generate<bool>(300, 599, (i) =>
            {
                response.Object.StatusCode = (HttpStatusCode)i;
                return response.Object.IsSuccessStatusCode();
            });

            // Assert
            Assert.IsTrue(result.All(v => v == false));
        }

        [TestMethod]
        public void SetErrorMessage_NullValue_UsesStatusCode()
        {
            // Arrange
            var jtoken = new JObject();
            var response = new Mock<BaseResponse<JToken>>() { CallBase = true };
            var statusCode = HttpStatusCode.OK;
            var statusString = statusCode.ToString();
            var value = null as JValue;

            response.SetupGet(r => r.StatusCode).Returns(statusCode);

            // Act
            response.Object.SetErrorMessage(value);

            // Assert
            response.VerifySet(r => r.ErrorMessage = statusString, Times.Once);
        }


        [TestMethod]
        public void SetErrorMessage_NonNullValue_UsesValue()
        {
            // Arrange
            var jtoken = new JObject();
            var response = new Mock<BaseResponse<JToken>>() { CallBase = true };
            var valueString = TestHelper.GetUniqueId();
            var value = new JValue(valueString);

            // Act
            response.Object.SetErrorMessage(value);

            // Assert
            response.VerifySet(r => r.ErrorMessage = valueString, Times.Once);
        }
    }
}
