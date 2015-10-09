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
    using Newtonsoft.Json;

    public abstract class BaseConverter : JsonConverter
    {
        protected internal abstract void WriteJson(JsonWriter writer, object value, SerializerProxy serializer);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            EnsureArgument.NotNull(writer, "writer");
            EnsureArgument.NotNull(serializer, "serializer");

            // KLUGE: JsonSerializer is not mockable, so we resort to the bad practice of inserting
            // code (i.e., the proxy) merely to support unit testing.
            this.WriteJson(writer, value, new SerializerProxy(serializer));
        }

        protected internal virtual void WriteProperty(JsonWriter writer, SerializerProxy serializer, string name, object value, bool isOptional)
        {
            if (isOptional && value == null)
            {
                return;
            }

            writer.WritePropertyName(name);
            serializer.Serialize(writer, value);
        }

        protected internal virtual void WriteProperty(JsonWriter writer, SerializerProxy serializer, string name, string value, bool isOptional)
        {
            if (isOptional && String.IsNullOrEmpty(value))
            {
                return;
            }

            writer.WritePropertyName(name);
            serializer.Serialize(writer, value);
        }
    }
}
