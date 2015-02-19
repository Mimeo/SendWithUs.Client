// Copyright © 2014 Mimeo, Inc.

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

namespace SendWithUs.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Provides access to the SendWithUs REST API.
    /// </summary>
    /// <remarks>
    /// Currently, the client only covers a small portion of the API surface--namely, sending email and 
    /// batch operations. 
    /// </remarks>
    public class SendWithUsClient : ISendWithUsClient
    {
        #region State

        /// <summary>
        /// The base URI of the SendWithUs service.
        /// </summary>
        /// <remarks>MUST have a trailing slash.</remarks>
        protected const string BaseUri = "https://api.sendwithus.com/api/v1/";

        /// <summary>
        /// Gets or sets the HTTP implementation.
        /// </summary>
        protected HttpClient Worker { get; set; }

        /// <summary>
        /// Gets or sets the factory used to construct response objects.
        /// </summary>
        protected IResponseFactory ResponseFactory { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SendWithUsClient class.
        /// </summary>
        /// <param name="apiKey">The API key to use when authenticating with SendWithUs.</param>
        public SendWithUsClient(string apiKey) : this(apiKey, new HttpClient())
        { }

        /// <summary>
        /// Initializes a new instance of the SendWithUsClient class.
        /// </summary>
        /// <param name="apiKey">The API key to use when authenticating with SendWithUs.</param>
        /// <param name="worker">The HTTP implementation.</param>
        public SendWithUsClient(string apiKey, HttpClient worker) : this(apiKey, worker, new ResponseFactory())
        { }

        /// <summary>
        /// Initializes a new instance of the SendWithUsClient class.
        /// </summary>
        /// <param name="apiKey">The API key to use when authenticating with SendWithUs.</param>
        /// <param name="worker">The HTTP implementation.</param>
        /// <param name="responseFactory">The factory used to construct response objects.</param>
        public SendWithUsClient(string apiKey, HttpClient worker, IResponseFactory responseFactory)
        {
            this.Initialize(apiKey, worker, responseFactory);
        }

        /// <summary>
        /// Prepares the current instance for use.
        /// </summary>
        /// <param name="apiKey">The API key to use when authenticating with SendWithUs.</param>
        /// <param name="worker">The HTTP implementation.</param>
        /// <param name="responseFactory">The factory used to construct response objects.</param>
        /// <returns>The current instance.</returns>
        protected virtual SendWithUsClient Initialize(string apiKey, HttpClient worker, IResponseFactory responseFactory)
        {
            EnsureArgument.NotNullOrEmpty(apiKey, "apiKey");
            EnsureArgument.NotNull(worker, "worker");
            EnsureArgument.NotNull(responseFactory, "responseFactory");

            this.Worker = worker;
            this.ResponseFactory = responseFactory;

            this.Worker.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Basic", this.BuildAuthenticationToken(apiKey));

            return this;
        }

        #endregion

        #region API methods

        /// <summary>
        /// Sends an email message per the given request object.
        /// </summary>
        /// <param name="request">A request object describing the email to be sent.</param>
        /// <returns>A response object.</returns>
        /// <exception cref="System.ArgumentNullException">The request argument was null.</exception>
        public virtual async Task<ISendResponse> SendAsync(ISendRequest request)
        {
            EnsureArgument.NotNull(request, "request");

            var httpResponse = await this.PostJsonAsync("send", request.Validate()).ConfigureAwait(false);
            var json = await this.ReadJsonAsync(httpResponse);
            return this.ResponseFactory.Create<SendResponse>(httpResponse.StatusCode, json);
        }

        /// <summary>
        /// Submits a batch request comprising the given set of request objects.
        /// </summary>
        /// <param name="requests">A set of request objects to be batched.</param>
        /// <returns>A response object.</returns>
        /// <exception cref="System.ArgumentException">The requests argument was null, empty, or contained null item(s).</exception>
        public virtual async Task<IBatchResponse> BatchAsync(IEnumerable<IRequest> requests)
        {
            EnsureArgument.NotNullOrEmpty(requests, "requests", false);

            var batchRequest = requests.Select(r => new BatchRequestWrapper(r.Validate()));
            var httpResponse = await this.PostJsonAsync("batch", batchRequest).ConfigureAwait(false);
            var json = await this.ReadJsonAsync(httpResponse);
            var responseSequence = requests.Select(r => r.GetResponseType());
            return this.ResponseFactory.Create(responseSequence, httpResponse.StatusCode, json);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Builds a token for use in HTTP basic authentication.
        /// </summary>
        /// <param name="apiKey">A valid SendWithUs API key.</param>
        /// <returns>A token for use in HTTP basic authentication.</returns>
        protected string BuildAuthenticationToken(string apiKey)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":"));
        }

        /// <summary>
        /// Builds a URI string from the given arguments.
        /// </summary>
        /// <param name="template">A URI template with positional parameters.</param>
        /// <param name="args">Arguments for the template.</param>
        /// <returns>The expanded template.</returns>
        /// <remarks>The template MUST NOT begin with a slash.</remarks>
        protected string BuildRequestUri(string template, params object[] args)
        {
            return SendWithUsClient.BaseUri + (args.Length > 0 ? String.Format(template, args) : template);
        }

        /// <summary>
        /// Posts to the specified URI path with the request serialized as JSON.
        /// </summary>
        /// <typeparam name="TRequest">The  type of the request.</typeparam>
        /// <param name="path">The path component of the request URI.</param>
        /// <param name="request">The request object.</param>
        /// <returns>A response message.</returns>
        protected Task<HttpResponseMessage> PostJsonAsync<TRequest>(string path, TRequest request)
        {
            return this.Worker.PostAsJsonAsync(this.BuildRequestUri(path), request);
        }

        /// <summary>
        /// Reads the content of the given response as a <see cref="Newtonsoft.Json.Linq.JToken"/>.
        /// </summary>
        /// <param name="response">The response object to read.</param>
        /// <returns>A JSON token.</returns>
        protected Task<JToken> ReadJsonAsync(HttpResponseMessage response)
        {
            return response.Content.ReadAsAsync<JToken>();
        }

        #endregion
    }
}
