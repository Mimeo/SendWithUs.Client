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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class BatchResponseTests
    {
        [TestMethod]
        public void Populate_NullJson_DoesNotSetItems()
        {
            // Arrange
            var responseFactory = (IResponseFactory)null;
            var responseSequence = (IEnumerable<Type>)null;
            var response = new Mock<BatchResponse>(responseFactory, responseSequence);
            var json = (JArray)null;

            // Act
            response.Object.Populate(json);

            // Assert
            response.VerifySet(r => r.Items = It.IsAny<IList<IResponse>>(), Times.Never);
        }

        [TestMethod]
        public void Populate_Normally_SetsItems()
        {
            // Arrange
            var responseFactory = (IResponseFactory)null;
            var responseSequence = Enumerable.Empty<Type>();
            var response = new Mock<BatchResponse>(responseFactory, responseSequence) { CallBase = true };
            var json = new JArray();

            response.SetupSet(r => r.Items = It.IsAny<IList<IResponse>>());

            // Act
            response.Object.Populate(json);

            // Assert
            response.VerifySet(r => r.Items = It.IsAny<IList<IResponse>>(), Times.Once);
        }

        [TestMethod]
        public void Populate_Normally_BuildsResponses()
        {
            // Arrange
            var count = TestHelper.GetRandomInteger(1, 10);
            var responseFactory = (IResponseFactory)null;
            var responseSequence = TestHelper.Generate(count, i => typeof(IResponse));
            var response = new Mock<BatchResponse>(responseFactory, responseSequence) { CallBase = true };
            var json = new JArray(TestHelper.Generate(count, i => new JObject()).ToArray());

            response.Setup(r => r.BuildResponse(It.IsAny<JObject>(), It.IsAny<Type>())).Returns(default(IResponse));
            response.SetupSet(r => r.Items = It.IsAny<IList<IResponse>>());

            // Act
            response.Object.Populate(json);

            // Assert
            response.Verify(r => r.BuildResponse(It.IsAny<JObject>(), It.IsAny<Type>()), Times.Exactly(count));
        }

        [TestMethod]
        public void BuildResponse_NullWrapper_Throws()
        {
            // Arrange
            var responseFactory = (IResponseFactory)null;
            var responseSequence = (IEnumerable<Type>)null;
            var response = new BatchResponse(responseFactory, responseSequence);
            var wrapper = (JObject)null;
            var responseType = typeof(IResponse);

            // Act
            var exception = TestHelper.CaptureException(() => response.BuildResponse(wrapper, responseType));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void BuildResponse_NullResponseType_Throws()
        {
            // Arrange
            var responseFactory = (IResponseFactory)null;
            var responseSequence = (IEnumerable<Type>)null;
            var response = new BatchResponse(responseFactory, responseSequence);
            var wrapper = new JObject();
            var responseType = (Type)null;

            // Act
            var exception = TestHelper.CaptureException(() => response.BuildResponse(wrapper, responseType));

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void BuildResponse_Normally_InvokesResponseFactory()
        {
            // Arrange
            var responseFactory = new Mock<IResponseFactory>();
            var responseSequence = (IEnumerable<Type>)null;
            var response = new BatchResponse(responseFactory.Object, responseSequence);
            var statusCode = HttpStatusCode.OK;
            var body = new JObject();
            var wrapper = JObject.FromObject(new { status_code = statusCode, body = body });
            var responseType = typeof(IResponse);

            responseFactory.Setup(f => f.Create(It.IsAny<Type>(), It.IsAny<HttpStatusCode>(), It.IsAny<JToken>()));

            // Act
            response.BuildResponse(wrapper, responseType);

            // Assert
            // Since there are no setups on the wrapper, we are also verifying that the statusCode and body
            // values were properly extracted from the wrapper.
            responseFactory.Verify(f => f.Create(responseType, statusCode, body));
        }
    }
}
