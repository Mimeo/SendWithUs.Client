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
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Newtonsoft.Json;

    /// <summary>
    /// Converts a SendRequest object to JSON.
    /// </summary>
    public class SendRequestConverter : JsonConverter
    {
        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            //return objectType == typeof(ISendRequest);
            return typeof(ISendRequest).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var request = value as ISendRequest;

            if (request == null)
            {
                return;
            }

            request.Validate();

            writer.WriteStartObject();

            this.WriteProperty(writer, serializer, "email_id", request.TemplateId);
            this.WriteEmailData(writer, serializer, request);
            this.WritePrimaryRecipient(writer, serializer, request);
            this.WriteCcRecipients(writer, serializer, request);
            this.WriteBccRecipients(writer, serializer, request);
            this.WriteTags(writer, serializer, request);
            this.WriteInlineAttachment(writer, serializer, request);
            this.WriteFileAttachments(writer, serializer, request);
            this.WriteProperty(writer, serializer, "esp_account", request.ProviderId, true);
            this.WriteProperty(writer, serializer, "version_name", request.TemplateVersionId, true);

            writer.WriteEndObject();
        }

        protected void WritePrimaryRecipient(JsonWriter writer, JsonSerializer serializer, ISendRequest request)
        {
            writer.WritePropertyName("recipient");
            writer.WriteStartObject();
            this.WriteProperty(writer, serializer, "name", request.RecipientName, true);
            this.WriteProperty(writer, serializer, "address", request.RecipientAddress);
            writer.WriteEndObject();
        }

        protected void WriteEmailData(JsonWriter writer, JsonSerializer serializer, ISendRequest request)
        {
            if (request.Data == null || request.Data.Count == 0)
            {
                return;
            }

            writer.WritePropertyName("email_data");
            writer.WriteStartObject();

            foreach (var pair in request.Data)
            {
                this.WriteProperty(writer, serializer, pair.Key, pair.Value);
            }

            writer.WriteEndObject();
        }

        protected void WriteCcRecipients(JsonWriter writer, JsonSerializer serializer, ISendRequest request)
        {
            this.WriteRecipientsList(writer, serializer, "cc", request.CopyTo);
        }

        protected void WriteBccRecipients(JsonWriter writer, JsonSerializer serializer, ISendRequest request)
        {
            this.WriteRecipientsList(writer, serializer, "bcc", request.BlindCopyTo);
        }

        protected void WriteRecipientsList(JsonWriter writer, JsonSerializer serializer, string name, IEnumerable<string> recipients)
        {
            if (recipients == null)
            {
                return;
            }

            writer.WritePropertyName(name);
            writer.WriteStartArray();

            foreach (var address in recipients)
            {
                writer.WriteStartObject();
                this.WriteProperty(writer, serializer, "address", address);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        protected void WriteTags(JsonWriter writer, JsonSerializer serializer, ISendRequest request)
        {
            if (request.Tags == null)
            {
                return;
            }

            writer.WritePropertyName("tags");
            writer.WriteStartArray();

            foreach (var tag in request.Tags)
            {
                serializer.Serialize(writer, tag);
            }

            writer.WriteEndArray();
        }

        protected void WriteInlineAttachment(JsonWriter writer, JsonSerializer serializer, ISendRequest request)
        {
            if (request.InlineAttachment == null)
            {
                return;
            }

            writer.WritePropertyName("inline");
            this.WriteAttachmentObject(writer, serializer, request.InlineAttachment);
        }

        protected void WriteFileAttachments(JsonWriter writer, JsonSerializer serializer, ISendRequest request)
        {
            if (request.FileAttachments == null)
            {
                return;
            }

            writer.WritePropertyName("files");
            writer.WriteStartArray();

            foreach (var attachment in request.FileAttachments)
            {
                this.WriteAttachmentObject(writer, serializer, attachment);
            }

            writer.WriteEndArray();
        }

        protected void WriteAttachmentObject(JsonWriter writer, JsonSerializer serializer, IAttachment attachment)
        {
            writer.WriteStartObject();
            this.WriteProperty(writer, serializer, "id", attachment.Id);
            this.WriteProperty(writer, serializer, "data", attachment.Data);
            writer.WriteEndObject();
        }

        protected void WriteProperty(JsonWriter writer, JsonSerializer serializer, string name, string value, bool isOptional = false)
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
