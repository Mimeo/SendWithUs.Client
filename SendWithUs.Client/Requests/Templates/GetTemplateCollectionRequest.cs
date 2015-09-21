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

namespace SendWithUs.Client
{
    using System;

    public class GetTemplateCollectionRequest : BaseTemplateRequest, IGetTemplateCollectionRequest
    {
        public static readonly IGetTemplateCollectionRequest AllTemplates = new GetTemplateCollectionRequest();

        #region Constructors

        // Prevent external code from creating instances.
        protected GetTemplateCollectionRequest() { }

        #endregion

        #region Base Class Overrides

        public override string GetHttpMethod() => "GET";
        
        public override Type GetResponseType() => typeof(CollectionResponse<ITemplateCollectionItem>);

        // TemplateId is actually forbidden for this class, but since external code cannot create instances
        // and set the TemplateId property, the distinction between forbidden and not required is moot.
        protected override bool IsTemplateIdRequired() => false;

        #endregion
    }
}
