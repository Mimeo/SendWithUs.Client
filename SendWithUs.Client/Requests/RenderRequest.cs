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
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the data necessary to make API requests to render templates.
    /// </summary>
    [JsonConverter(typeof(RenderRequestConverter))]
    public class RenderRequest : IRenderRequest
    {
        #region Constructors

        public RenderRequest()
        { }

        public RenderRequest(string templateId) : this(templateId, null)
        { }

        public RenderRequest(string templateId, object data)
        {
            EnsureArgument.NotNullOrEmpty(templateId, "templateId");

            this.TemplateId = templateId;
            this.Data = data;
        }

        #endregion

        #region IRenderRequest Members

        public string TemplateId { get; set; }

        public string TemplateVersionId { get; set; }

        public object Data { get; set; }

        public string Locale { get; set; }

        #endregion

        #region IRequest Members

        public string GetHttpMethod()
        {
            return "POST";
        }

        public string GetUriPath()
        {
            return "/api/v1/render";
        }

        public Type GetResponseType()
        {
            return typeof(RenderResponse);
        }

        public IRequest Validate()
        {
            // Template ID is required.
            if (String.IsNullOrEmpty(this.TemplateId))
            {
                throw new ValidationException(ValidationFailureMode.MissingTemplateId);
            }

            return this;
        }

        public virtual bool IsValid
        {
            get
            {
                try
                {
                    this.Validate();
                }
                catch (ValidationException)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion
    }
}
