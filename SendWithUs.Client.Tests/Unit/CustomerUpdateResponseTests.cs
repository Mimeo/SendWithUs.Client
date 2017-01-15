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
    using System.Net;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;
    using Names = SendWithUs.Client.CustomerUpdateResponse.PropertyNames;

    [TestClass]
    public class CustomerUpdateResponseTests
    {
        [TestMethod]
        public void Populate_NullJson_DoesNotSetProperties()
        {
            // Arrange
            var response = new Mock<CustomerUpdateResponse>() { CallBase = true };
            var json = null as JObject;

            // Act
            response.Object.Populate(json);

            // Assert
            response.VerifySet(r => r.Success = It.IsAny<bool>(), Times.Never);
            response.VerifySet(r => r.Status = It.IsAny<string>(), Times.Never);
        }

        [TestMethod]
        public void Populate_ValidJson_SetsProperties()
        {
            // Arrange
            var response = new Mock<CustomerUpdateResponse>() { CallBase = true };
            var success = true;
            var status = TestHelper.GetUniqueId();
            var json = new Mock<JObject>();
            var details = new Mock<JObject>();

            json.Setup(j => j.Value<bool>(Names.Success)).Returns(success);
            json.Setup(j => j.Value<string>(Names.Status)).Returns(status);

            // Act
            response.Object.Populate(json.Object);

            // Assert
            response.VerifySet(r => r.Success = success, Times.Once);
            response.VerifySet(r => r.Status = status, Times.Once);
        }
    }
}
