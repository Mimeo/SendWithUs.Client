// Copyright © 2015 Mimeo, Inc. All rights reserved.

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
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Describes the interface of objects used to access to the SendWithUs REST API.
    /// </summary>
    public interface ISendWithUsClient
    {
        #region Templates

        /// <summary>
        /// Executes a request to retrieve a template.
        /// </summary>
        /// <param name="request">The request to retrieve a template.</param>
        /// <returns>A response object.</returns>
        Task<ITemplateResponse> ExecuteAsync(IGetTemplateRequest request);

        /// <summary>
        /// Executes a request to retrieve a template version.
        /// </summary>
        /// <param name="request">The request to retrieve a template version.</param>
        /// <returns>A response object.</returns>
        Task<ITemplateVersionResponse> ExecuteAsync(IGetTemplateVersionRequest request);

        /// <summary>
        /// Executes a request to retrieve a collection of templates.
        /// </summary>
        /// <param name="request">The request to retrieve a collection of templates.</param>
        /// <returns>A response object.</returns>
        Task<ICollectionResponse<ITemplateCollectionItem>> ExecuteAsync(IGetTemplateCollectionRequest request);

        /// <summary>
        /// Executes a request to retrieve a collection of template versions.
        /// </summary>
        /// <param name="request">The request to retrieve a collection of template versions.</param>
        /// <returns>A response object.</returns>
        Task<ICollectionResponse<ITemplateVersionCollectionItem>> ExecuteAsync(IGetTemplateVersionCollectionRequest request);

        /// <summary>
        /// Executes a request to create a template.
        /// </summary>
        /// <param name="request">The request to create a template.</param>
        /// <returns>A response object.</returns>
        Task<ITemplateResponse> ExecuteAsync(ICreateTemplateRequest request);

        /// <summary>
        /// Executes a request to create a version of an existing template.
        /// </summary>
        /// <param name="request">The request to create a template version.</param>
        /// <returns>A response object.</returns>
        Task<ITemplateVersionResponse> ExecuteAsync(ICreateTemplateVersionRequest request);

        /// <summary>
        /// Executes a request to update a version of an existing template.
        /// </summary>
        /// <param name="request">The request to update a template version.</param>
        /// <returns>A response object.</returns>
        Task<ITemplateVersionResponse> ExecuteAsync(IUpdateTemplateVersionRequest request);

        /// <summary>
        /// Executes a request to delete a template.
        /// </summary>
        /// <param name="request">The request to delete a template.</param>
        /// <returns>A response object.</returns>
        Task<VoidResponse> ExecuteAsync(IDeleteTemplateRequest request);

        #endregion

        #region Batching

        /// <summary>
        /// Executes a batch request comprising the given collection of request objects.
        /// </summary>
        /// <param name="requests">A collection of request objects to be batched.</param>
        /// <returns>A batch response object.</returns>
        Task<IBatchResponse> ExecuteAsync(params IRequest[] requests);

        /// <summary>
        /// Executes a batch request comprising the given collection of request objects.
        /// </summary>
        /// <param name="requests">A collection of request objects to be batched.</param>
        /// <returns>A batch response object.</returns>
        Task<IBatchResponse> ExecuteAsync(IEnumerable<IRequest> requests);

        #endregion

        #region Deprecated API

        /// <summary>
        /// Sends an email message per the given request object.
        /// </summary>
        /// <param name="request">A request object describing the email to be sent.</param>
        /// <returns>A response object.</returns>
        Task<ISendResponse> SendAsync(ISendRequest request);

        /// <summary>
        /// Renders a template per the given request object.
        /// </summary>
        /// <param name="request">A request object describing the template to be rendered.</param>
        /// <returns>A response object.</returns>
        Task<IRenderResponse> RenderAsync(IRenderRequest request);
        
        /// <summary>
        /// Submits a batch request comprising the given collection of request objects.
        /// </summary>
        /// <param name="requests">A collection of request objects to be batched.</param>
        /// <returns>A response object.</returns>
        Task<IBatchResponse> BatchAsync(IEnumerable<IRequest> requests);

        #endregion
    }
}
