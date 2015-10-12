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
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class TemplateInfo : ITemplateCollectionItem
    {
        internal static class PropertyNames
        {
            public const string Id = "id";
            public const string Name = "name";
            public const string Locale = "locale";
            public const string Created = "created";
            public const string Versions = "versions";
            public const string Tags = "tags";
        }

        #region ITemplateInfo Members

        public string Id { get; set; }

        public string Name { get; set; }

        public string Locale { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<ITemplateVersionInfo> Versions { get; set; }

        public IEnumerable<string> Tags { get; set; }

        #endregion

        #region ICollectionItem Members

        public ICollectionItem Initialize(IResponseFactory responseFactory, JToken json)
        {
            var jObject = json as JObject;

            if (jObject == null)
            {
                throw new ArgumentException("Expecting an instance of JObject.", nameof(json));
            }

            this.Id = jObject.Value<string>(PropertyNames.Id);
            this.Name = jObject.Value<string>(PropertyNames.Name);
            this.Locale = jObject.Value<string>(PropertyNames.Locale);
            // It would be cool if we could write json.Value<DateTime>("name") here.
            this.Created = DateTimeHelper.FromUnixTimeSeconds(jObject.Value<long>(PropertyNames.Created));

            var versions = jObject.GetValue(PropertyNames.Versions) as JArray;

            if (versions != null)
            {
                this.Versions = versions.Select(jt => this.BuildVersionInfo(responseFactory, jt)).ToArray();
            }
            else
            {
                this.Versions = Enumerable.Empty<ITemplateVersionInfo>();
            }

            var tags = jObject.GetValue(PropertyNames.Tags) as JArray;

            if (tags != null)
            {
                this.Tags = tags.Select(jt => jt.Value<string>()).ToArray();
            }
            else
            {
                this.Tags = Enumerable.Empty<string>();
            }

            return this;
        }

        #endregion

        #region Helpers

        protected ITemplateVersionInfo BuildVersionInfo(IResponseFactory responseFactory, JToken json) => 
            responseFactory.CreateItem(typeof(TemplateVersionInfo), json) as ITemplateVersionInfo;

        #endregion
    }
}
