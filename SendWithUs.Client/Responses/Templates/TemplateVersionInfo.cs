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
    using Newtonsoft.Json.Linq;

    public class TemplateVersionInfo : ITemplateVersionInfo, ICollectionItem
    {
        internal static class PropertyNames
        {
            public const string Id = "id";
            public const string Name = "name";
            public const string Locale = "locale";
            public const string Created = "created";
            public const string Modified = "modified";
            public const string Published = "published";
            public const string Subject = "subject";
            public const string Html = "html";
            public const string Text = "text";
        }

        #region ITemplateVersionInfo Members

        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public bool? Published { get; set; }

        public string Subject { get; set; }

        public string Html { get; set; }

        public string Text { get; set; }

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
            // It would be cool if we could write json.Value<DateTime>("name") here.
            this.Created = DateTimeHelper.FromUnixTimeSeconds(jObject.Value<long>(PropertyNames.Created));

            var modified = jObject.GetValue(PropertyNames.Modified);

            if (modified != null)
            {
                this.Modified = DateTimeHelper.FromUnixTimeSeconds(modified.Value<long>());
            }

            var published = jObject.GetValue(PropertyNames.Published);

            if (published != null)
            {
                this.Published = published.Value<bool>();
            }

            this.Subject = jObject.Value<string>(PropertyNames.Subject);
            this.Html = jObject.Value<string>(PropertyNames.Html);
            this.Text = jObject.Value<string>(PropertyNames.Text);

            return this;
        }

        #endregion
    }
}
