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
    using SendWithUs.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class SendWithUsClientTests
    {
        #region RenderAsync

        [Fact]
        public void RenderAsync_NullRequest_Throws()
        {
            // Arrange
            var apiKey = TestHelper.GetUniqueId();
            var client = new SendWithUsClient(apiKey);
            var request = null as IRenderRequest;

            // Act
            var exception = TestHelper.CaptureException(() => client.RenderAsync(request));

            // Assert
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        #endregion

        #region SendAsync

        [Fact]
        public void SendAsync_NullRequest_Throws()
        {
            // Arrange
            var apiKey = TestHelper.GetUniqueId();
            var client = new SendWithUsClient(apiKey);
            var request = null as ISendRequest;

            // Act
            var exception = TestHelper.CaptureException(() => client.SendAsync(request));

            // Assert
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        #endregion

        #region BatchAsync

        [Fact]
        public void BatchAsync_NullRequests_Throws()
        {
            // Arrange
            var apiKey = TestHelper.GetUniqueId();
            var client = new SendWithUsClient(apiKey);
            var requests = null as IEnumerable<IRequest>;

            // Act
            var exception = TestHelper.CaptureException(() => client.BatchAsync(requests));

            // Assert
            Assert.IsType(typeof(ArgumentException), exception);
        }

        [Fact]
        public void BatchAsync_EmptyRequests_Throws()
        {
            // Arrange
            var apiKey = TestHelper.GetUniqueId();
            var client = new SendWithUsClient(apiKey);
            var requests = Enumerable.Empty<IRequest>();

            // Act
            var exception = TestHelper.CaptureException(() => client.BatchAsync(requests));

            // Assert
            Assert.IsType(typeof(ArgumentException), exception);
        }

        #endregion
    }
}
