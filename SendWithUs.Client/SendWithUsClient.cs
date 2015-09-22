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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides access to the SendWithUs REST API.
    /// </summary>
    /// <remarks>
    /// Currently, the client only covers a small portion of the API surface--namely, sending email
    /// rendering templates, and batch operations. 
    /// </remarks>
    public class SendWithUsClient : ISendWithUsClient
    {
        #region State

        /// <summary>
        /// The base URI of the SendWithUs service.
        /// </summary>
        /// <remarks>MUST NOT have a trailing slash.</remarks>
        private const string BaseUri = "https://api.sendwithus.com";

        /// <summary>
        /// Gets or sets the HTTP client.
        /// </summary>
        protected HttpClient HttpClient { get; set; }

        /// <summary>
        /// Gets or sets the formatter for JSON payloads.
        /// </summary>
        protected JsonMediaTypeFormatter ContentFormatter { get; set; }

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
        public SendWithUsClient(string apiKey) : this(apiKey, new HttpClient(), new JsonMediaTypeFormatter(), new ResponseFactory())
        { }

        /// <summary>
        /// Initializes a new instance of the SendWithUsClient class.
        /// </summary>
        /// <param name="apiKey">The API key to use when authenticating with SendWithUs.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="contentFormatter">The formatter for JSON payloads.</param>
        /// <param name="responseFactory">The factory used to construct response objects.</param>
        public SendWithUsClient(string apiKey, HttpClient httpClient, JsonMediaTypeFormatter contentFormatter, IResponseFactory responseFactory)
        {
            this.Initialize(apiKey, httpClient, contentFormatter, responseFactory);
        }

        /// <summary>
        /// Prepares the current instance for use.
        /// </summary>
        /// <param name="apiKey">The API key to use when authenticating with SendWithUs.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="contentFormatter">The formatter for JSON payloads.</param>
        /// <param name="responseFactory">The factory used to construct response objects.</param>
        /// <returns>The current instance.</returns>
        protected virtual SendWithUsClient Initialize(string apiKey, HttpClient httpClient, JsonMediaTypeFormatter contentFormatter, IResponseFactory responseFactory)
        {
            EnsureArgument.NotNullOrEmpty(apiKey, nameof(apiKey));
            EnsureArgument.NotNull(httpClient, nameof(httpClient));
            EnsureArgument.NotNull(contentFormatter, nameof(contentFormatter));
            EnsureArgument.NotNull(responseFactory, nameof(responseFactory));

            this.HttpClient = httpClient;
            this.ContentFormatter = contentFormatter;
            this.ResponseFactory = responseFactory;
            
            // FIXME
            this.ContentFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            this.HttpClient.DefaultRequestHeaders.Authorization = this.BuildAuthenticationHeader(apiKey);

            return this;
        }

        #endregion

        #region ISendWithUsClient implementation

        public async Task<ITemplateResponse> ExecuteAsync(IGetTemplateRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<TemplateResponse>(request.Validate()).ConfigureAwait(false);
        }

        public async Task<ITemplateVersionResponse> ExecuteAsync(IGetTemplateVersionRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<TemplateVersionResponse>(request.Validate()).ConfigureAwait(false);
        }

        public async Task<ICollectionResponse<ITemplateCollectionItem>> ExecuteAsync(IGetTemplateCollectionRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<CollectionResponse<ITemplateCollectionItem>, TemplateInfo>(request.Validate()).ConfigureAwait(false);
        }

        public async Task<ICollectionResponse<ITemplateVersionCollectionItem>> ExecuteAsync(IGetTemplateVersionCollectionRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<CollectionResponse<ITemplateVersionCollectionItem>, TemplateVersionInfo>(request.Validate()).ConfigureAwait(false);
        }

        public async Task<ITemplateResponse> ExecuteAsync(ICreateTemplateRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<TemplateResponse>(request.Validate()).ConfigureAwait(false);
        }

        public async Task<ITemplateVersionResponse> ExecuteAsync(ICreateTemplateVersionRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<TemplateVersionResponse>(request.Validate()).ConfigureAwait(false);
        }

        public async Task<ITemplateVersionResponse> ExecuteAsync(IUpdateTemplateVersionRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<TemplateVersionResponse>(request.Validate()).ConfigureAwait(false);
        }

        public async Task<VoidResponse> ExecuteAsync(IDeleteTemplateRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<VoidResponse>(request.Validate()).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Executes a request to send an email message.
        /// </summary>
        /// <param name="request">The request to send an email message.</param>
        /// <returns>A response object.</returns>
        public async Task<ISendResponse> ExecuteAsync(ISendRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<SendResponse>(request.Validate()).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes a request to render a template.
        /// </summary>
        /// <param name="request">The request to render a template.</param>
        /// <returns>A response object.</returns>
        public async Task<IRenderResponse> ExecuteAsync(IRenderRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<RenderResponse>(request.Validate()).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Executes a batch request comprising the given set of request objects.
        /// </summary>
        /// <param name="requests">A set of request objects to be batched.</param>
        /// <returns>A batch response object.</returns>
        public virtual Task<IBatchResponse> ExecuteAsync(params IRequest[] requests) => 
            this.ExecuteAsync((IEnumerable<IRequest>)requests);

        /// <summary>
        /// Executes a batch request comprising the given set of request objects.
        /// </summary>
        /// <param name="requests">A set of request objects to be batched.</param>
        /// <returns>A batch response object.</returns>
        /// <exception cref="System.ArgumentException">The requests argument was null, empty, or contained null item(s).</exception>
        public virtual async Task<IBatchResponse> ExecuteAsync(IEnumerable<IRequest> requests)
        {
            EnsureArgument.NotNullOrEmpty(requests, nameof(requests), false);
            var batchRequest = new BatchRequest(requests.Select(r => r.Validate()));
            var batchResponse = await this.ExecuteAsync<BatchResponse>(batchRequest).ConfigureAwait(false);
            return batchResponse.Inflate(requests.Select(r => r.GetResponseType()), this.ResponseFactory);
        }

        #region Deprecated API

        /// <summary>
        /// Sends an email message per the given request object.
        /// </summary>
        /// <param name="request">A request object describing the email to be sent.</param>
        /// <returns>A response object.</returns>
        /// <exception cref="System.ArgumentNullException">The request argument was null.</exception>
        [Obsolete("This method is deprecated and will be removed in the future. Use ExecuteAsync instead.")]
        public virtual async Task<ISendResponse> SendAsync(ISendRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<SendResponse>(request.Validate()).ConfigureAwait(false);
        }

        /// <summary>
        /// Renders a template per the given request object.
        /// </summary>
        /// <param name="request">A request object describing the template to be rendered.</param>
        /// <returns>A response object.</returns>
        /// <exception cref="System.ArgumentNullException">The request argument was null.</exception>
        [Obsolete("This method is deprecated and will be removed in the future. Use ExecuteAsync instead.")]
        public virtual async Task<IRenderResponse> RenderAsync(IRenderRequest request)
        {
            EnsureArgument.NotNull(request, nameof(request));
            return await this.ExecuteAsync<RenderResponse>(request.Validate()).ConfigureAwait(false);
        }

        /// <summary>
        /// Submits a batch request comprising the given set of request objects.
        /// </summary>
        /// <param name="requests">A set of request objects to be batched.</param>
        /// <returns>A response object.</returns>
        /// <exception cref="System.ArgumentException">The requests argument was null, empty, or contained null item(s).</exception>
        [Obsolete("This method is deprecated and will be removed in the future. Use ExecuteAsync instead.")]
        public virtual Task<IBatchResponse> BatchAsync(IEnumerable<IRequest> requests) => this.ExecuteAsync(requests);

        #endregion

        #endregion

        #region Helpers

        /// <summary>
        /// Builds an HTTP header that specifies Basic authentication using the given API key.
        /// </summary>
        /// <param name="apiKey">A valid SendWithUs API key.</param>
        /// <returns>An HTTP Basic authentication header.</returns>
        protected AuthenticationHeaderValue BuildAuthenticationHeader(string apiKey)
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":"));
            return new AuthenticationHeaderValue("Basic", token);
        }

        /// <summary>
        /// Builds a URI string from the given arguments.
        /// </summary>
        /// <param name="template">A URI template with positional parameters.</param>
        /// <param name="args">Arguments for the template.</param>
        /// <returns>The expanded template.</returns>
        /// <remarks>The template MUST begin with a slash.</remarks>
        protected string BuildRequestUri(string template, params object[] args)
            => SendWithUsClient.BaseUri + (args.Length > 0 ? String.Format(template, args) : template);

        protected bool NeedRequestContent(HttpMethod method)
            => (method == HttpMethod.Post || method == HttpMethod.Put);

        /// <summary>
        /// Calls the SendWithUs API using the appropriate HTTP method and URI for the given request.
        /// </summary>
        /// <param name="request">A request object.</param>
        /// <returns>An HTTP response object.</returns>
        protected Task<HttpResponseMessage> GetHttpResponseAsync(IRequest request)
        {
            var method = new HttpMethod(request.GetHttpMethod());
            var uri = this.BuildRequestUri(request.GetUriPath());
            var httpRequest = new HttpRequestMessage(method, uri);
            
            if (this.NeedRequestContent(method))
            {
                httpRequest.Content = new ObjectContent(request.GetType(), request, this.ContentFormatter);
            }

            return this.HttpClient.SendAsync(httpRequest);
        }

        /// <summary>
        /// Reads the payload (i.e., message body) of a response from the SendWithUs API.
        /// </summary>
        /// <param name="httpResponse">An HTTP response object.</param>
        /// <returns>The payload represented as a JToken object.</returns>
        protected async Task<JToken> ReadHttpResponsePayloadAsync(HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Content.ReadAsAsync<JToken>();
            }
            else
            {
                return new JValue(await httpResponse.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        /// Executes the given request and creates the appropriate response.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response to be created.</typeparam>
        /// <param name="request">A request object.</param>
        /// <returns>A response object of the specified type.</returns>
        protected async Task<TResponse> ExecuteAsync<TResponse>(IRequest request)
            where TResponse : class, IResponse
        { 
            var httpResponse = await this.GetHttpResponseAsync(request);
            var payload = await this.ReadHttpResponsePayloadAsync(httpResponse);
            return this.ResponseFactory.Create<TResponse>(httpResponse.StatusCode, payload);
        }

        /// <summary>
        /// Executes the given request and creates the appropriate collection response.
        /// </summary>
        /// <typeparam name="TCollectionResponse">The type of the collection response to be created.</typeparam>
        /// <typeparam name="TCollectionItem">The concrete type of items in the collection response.</typeparam>
        /// <param name="request">A request object.</param>
        /// <returns>A response object of the specified type.</returns>
        protected async Task<TCollectionResponse> ExecuteAsync<TCollectionResponse, TCollectionItem>(IRequest request)
            where TCollectionResponse : class, ICollectionResponse
            where TCollectionItem : class, ICollectionItem
        {
            var httpResponse = await this.GetHttpResponseAsync(request);
            var payload = await this.ReadHttpResponsePayloadAsync(httpResponse);
            return this.ResponseFactory.Create<TCollectionResponse, TCollectionItem>(httpResponse.StatusCode, payload);
        }

        #endregion
    }
}
