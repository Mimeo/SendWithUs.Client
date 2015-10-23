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
    using System.Net;
    using Xunit;

    public class ResponseFactoryTests
    {
        public class TestResponse : IResponse
        {
            public bool Initialized { get; set; }

            #region IResponse Members

            public HttpStatusCode StatusCode
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsSuccessStatusCode
            {
                get { throw new NotImplementedException(); }
            }

            public string ErrorMessage
            {
                get { throw new NotImplementedException(); }
            }

            public Type InterfaceType => typeof(IResponse);

            public IResponse Initialize(IResponseFactory responseFactory, HttpStatusCode statusCode, JToken json)
            {
                this.Initialized = true;
                return this;
            }

            #endregion
        }

        [Fact]
        public void GenericCreate_Always_CallsCreate()
        {
            // Arrange
            var statusCode = HttpStatusCode.OK;
            var json = (JToken)null;
            var factory = new Mock<ResponseFactory>();

            // Act
            var response = factory.Object.Create<TestResponse>(statusCode, json);

            // Assert
            factory.Verify(f => f.CreateResponse(typeof(TestResponse), statusCode, json), Times.Once);
        }

        [Fact]
        public void Create_NullResponseType_Throws()
        {
            // Arrange
            var responseType = (Type)null;
            var statusCode = HttpStatusCode.OK;
            var json = (JToken)null;
            var factory = new ResponseFactory();

            // Act
            var exception = TestHelper.CaptureException(() => factory.CreateResponse(responseType, statusCode, json));

            // Assert
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        [Fact]
        public void Create_NonIResponseType_Throws()
        {
            // Arrange
            var responseType = typeof(String);
            var statusCode = HttpStatusCode.OK;
            var json = (JToken)null;
            var factory = new ResponseFactory();

            // Act
            var exception = TestHelper.CaptureException(() => factory.CreateResponse(responseType, statusCode, json));

            // Assert
            Assert.IsType(typeof(InvalidOperationException), exception);
        }

        [Fact]
        public void Create_IResponseSubtype_ReturnsInstanceOfSubtype()
        {
            // Arrange
            var responseType = typeof(TestResponse);
            var statusCode = HttpStatusCode.OK;
            var json = (JToken)null;
            var factory = new ResponseFactory();

            // Act
            var response = factory.CreateResponse(responseType, statusCode, json);

            // Assert 
            Assert.IsType(responseType, response);
        }

        [Fact]
        public void Create_Normally_InitializesInstance()
        {
            // Arrange
            var responseType = typeof(TestResponse);
            var statusCode = HttpStatusCode.OK;
            var json = (JToken)null;
            var factory = new ResponseFactory();

            // Act
            var response = factory.CreateResponse(responseType, statusCode, json) as TestResponse;

            // Assert 
            // The Initialized property was set by side-effect.
            Assert.True(response.Initialized);
        }
    }
}
