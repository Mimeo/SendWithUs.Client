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
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Xunit;

    public class BatchResponseTests
    {
        [Fact]
        public void Populate_NullJson_SetsItemsToEmpty()
        {
            // Arrange
            var responseFactory = new Mock<IResponseFactory>();
            var response = new Mock<BatchResponse> { CallBase = true };
            var json = null as JArray;
            var empty = Enumerable.Empty<IResponse>();

            response.SetupSet(r => r.Items = It.IsAny<IEnumerable<IResponse>>());

            // Act
            response.Object.Populate(responseFactory.Object, json);

            // Assert
            response.VerifySet(r => r.Items = empty, Times.Once);
        }

        [Fact]
        public void Populate_Normally_SetsItemsToList()
        {
            // Arrange
            var responseFactory = new Mock<IResponseFactory>();
            var response = new Mock<BatchResponse> { CallBase = true };
            var itemCount = TestHelper.GetRandomInteger(1, 10);
            var json = new JArray(Enumerable.Repeat(new JObject(), itemCount).ToArray());
            var itemTypes = Enumerable.Repeat(typeof(IResponse), itemCount).ToList<Type>();
            var mockItems = itemTypes.Select(t => new Mock<IResponse>()).ToList();
            var call = 0;
            var actualItems = default(IEnumerable<IResponse>);
            
            response.SetupGet(r => r.ItemTypes).Returns(itemTypes);
            response.Setup(r => r.BuildResponse(It.IsAny<JObject>(), typeof(IResponse), responseFactory.Object))
                .Returns(() => mockItems[call++].Object);
            response.SetupSet(r => r.Items = It.IsAny<IEnumerable<IResponse>>())
                .Callback<IEnumerable<IResponse>>(v => actualItems = v);

            // Act
            response.Object.Populate(responseFactory.Object, json);

            // Assert
            //response.VerifySet(r => r.Items = It.IsAny<List<IResponse>>(), Times.Once);
            response.Verify(r => r.BuildResponse(It.IsAny<JObject>(), It.IsAny<Type>(), It.IsAny<IResponseFactory>()), Times.Exactly(itemCount));
            Assert.Equal(itemCount, actualItems.Count());
            for (var index = 0; index < itemCount; index += 1)
            {
                Assert.Same(mockItems[index].Object, actualItems.ElementAt(index));
            }
        }
        
        [Fact]
        public void BuildResponse_NullWrapper_Throws()
        {
            // Arrange
            var response = new BatchResponse();
            var wrapper = (JObject)null;
            var responseType = typeof(IResponse);
            var responseFactory = new Mock<IResponseFactory>().Object;

            // Act
            var exception = TestHelper.CaptureException(() => response.BuildResponse(wrapper, responseType, responseFactory));

            // Assert
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        [Fact]
        public void BuildResponse_NullResponseType_Throws()
        {
            // Arrange
            var response = new BatchResponse();
            var wrapper = new JObject();
            var responseType = (Type)null;
            var responseFactory = new Mock<IResponseFactory>().Object;

            // Act
            var exception = TestHelper.CaptureException(() => response.BuildResponse(wrapper, responseType, responseFactory));

            // Assert
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        [Fact]
        public void BuildResponse_NullResponseFactory_Throws()
        {
            // Arrange
            var response = new BatchResponse();
            var wrapper = new JObject();
            var responseType = typeof(IResponse);
            var responseFactory = (IResponseFactory)null;

            // Act
            var exception = TestHelper.CaptureException(() => response.BuildResponse(wrapper, responseType, responseFactory));

            // Assert
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        [Fact]
        public void BuildResponse_Normally_InvokesResponseFactory()
        {
            // Arrange
            var response = new BatchResponse();
            var statusCode = HttpStatusCode.OK;
            var body = new JObject();
            var wrapper = JObject.FromObject(new { status_code = statusCode, body = body });
            var responseType = typeof(IResponse);
            var responseFactory = new Mock<IResponseFactory>();

            responseFactory.Setup(f => f.CreateResponse(It.IsAny<Type>(), It.IsAny<HttpStatusCode>(), It.IsAny<JToken>()));

            // Act
            response.BuildResponse(wrapper, responseType, responseFactory.Object);

            // Assert
            // Since there are no setups on the wrapper, we are also verifying that the statusCode and body
            // values were properly extracted from the wrapper.
            responseFactory.Verify(f => f.CreateResponse(responseType, statusCode, body));
        }
    }
}
