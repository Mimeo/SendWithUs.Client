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
    using System.Reflection;
    using Newtonsoft.Json;

    public class RenderRequestConverter : BaseConverter
    {
        internal static class PropertyNames
        {
            public const string TemplateId = "template";
            public const string TemplateVersionId = "version_id";
            public const string Data = "template_data";
            public const string Locale = "locale";
        }

        public override bool CanRead => false;

        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType) => typeof(IRenderRequest).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        protected internal override void WriteJson(JsonWriter writer, object value, SerializerProxy serializer)
        {
            var request = EnsureArgument.Of<IRenderRequest>(value, nameof(value));

            writer.WriteStartObject();

            this.WriteProperty(writer, serializer, PropertyNames.TemplateId, request.TemplateId, false);
            this.WriteProperty(writer, serializer, PropertyNames.TemplateVersionId, request.TemplateVersionId, true);
            this.WriteProperty(writer, serializer, PropertyNames.Data, request.Data, true);
            this.WriteProperty(writer, serializer, PropertyNames.Locale, request.Locale, true);

            writer.WriteEndObject();
        }
    }
}
