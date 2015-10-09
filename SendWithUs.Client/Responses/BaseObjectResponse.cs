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
    using Newtonsoft.Json.Linq;

    public abstract class BaseObjectResponse : BaseResponse<JObject>
    {
        /// <summary>
        /// Gets the value of the named property on the specified object.
        /// </summary>
        /// <param name="json">The object from which to get the property value.</param>
        /// <returns>The property value.</returns>
        /// <remarks>This method exists to aid unit testing of Populate(), because JObject.GetValue()
        /// is not virtual and therefore cannot be mocked.</remarks>
        protected internal virtual JToken GetPropertyValue(JObject json, string propertyName)
        {
            //var details = json.Property(propertyName);
            //return (details != null && details.HasValues) ? details.Value : null;
            return json.GetValue(propertyName);
        }

    }
}
