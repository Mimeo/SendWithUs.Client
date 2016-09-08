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

namespace SendWithUs.Client.Tests.EndToEnd
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class TestData
    {
        public string ApiKey
        {
            get { return this.GetString("ApiKey"); }
        }

        public string TemplateId
        {
            get { return this.GetString("TemplateId"); }
        }

        public string CampaignId
        {
            get { return this.GetString("CampaignId"); }
        }

        public string SenderAddress
        {
            get { return this.GetString("SenderAddress"); }
        }

        public string SenderName
        {
            get { return this.GetString("SenderName"); }
        }

        public string SenderReplyTo
        {
            get { return this.GetString("SenderReplyTo"); }
        }

        public string RecipientAddress
        {
            get { return this.GetString("RecipientAddress"); }
        }

        public string RecipientName
        {
            get { return this.GetString("RecipientName"); }
        }

        public IEnumerable<string> CopyTo
        {
            get { return null; }
        }

        public IEnumerable<string> BlindCopyTo
        {
            get { return null; }
        }

        public IDictionary<string, object> Data
        {
            get 
            {
                var rawData = this.DataSource.Root.Element("Data").Descendants("Item");
                return rawData.ToDictionary<XElement, string, object>(e => e.Attribute("name").Value, e => e.Attribute("value").Value);
            }
        }

        public IEnumerable<string> Tags
        {
            get { return null; }
        }

        public string AttachmentFileName
        {
            get { return this.GetString("AttachmentFileName"); }
        }

        public string ProviderId
        {
            get { return this.GetString("ProviderId"); }
        }

        public string TemplateVersion
        {
            get { return this.GetString("TemplateVersion"); }
        }

        protected XDocument DataSource { get; set; }

        protected string GetString(string name)
        {
            return this.DataSource.Root.Element(name).Value;
        }

        public TestData(string fileName)
        {
            this.DataSource = XDocument.Load(fileName);
        }
    }
}
