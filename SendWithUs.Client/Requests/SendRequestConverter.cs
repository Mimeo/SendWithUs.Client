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
    public class SendRequestConverter : BaseConverter
    {
        internal static class PropertyNames
        {
            public const string TemplateId = "template";
            public const string ProviderId = "esp_account";
            public const string TemplateVersionId = "version_name";
            public const string Locale = "locale";
            public const string Sender = "sender";
            public const string Recipient = "recipient";
            public const string Name = "name";
            public const string Address = "address";
            public const string ReplyTo = "reply_to";
            public const string Data = "template_data";
            public const string CopyTo = "cc";
            public const string BlindCopyTo = "bcc";
            public const string Tags = "tags";
            public const string Headers = "headers";
            public const string InlineAttachment = "inline";
            public const string FileAttachments = "files";
            public const string AttachmentId = "id";
            public const string AttachmentData = "data";
        }

        public override bool CanRead => false;

        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType) => typeof(ISendRequest).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        protected internal override void WriteJson(JsonWriter writer, object value, SerializerProxy serializer)
        {
            var request = EnsureArgument.Of<ISendRequest>(value, nameof(value));

            writer.WriteStartObject();

            this.WriteProperty(writer, serializer, PropertyNames.TemplateId, request.TemplateId, false);
            this.WriteProperty(writer, serializer, PropertyNames.ProviderId, request.ProviderId, true);
            this.WriteProperty(writer, serializer, PropertyNames.TemplateVersionId, request.TemplateVersionId, true);
            this.WriteProperty(writer, serializer, PropertyNames.Locale, request.Locale, true);
            this.WriteProperty(writer, serializer, PropertyNames.Data, request.Data, true);
            this.WriteProperty(writer, serializer, PropertyNames.Tags, request.Tags, true);
            this.WriteProperty(writer, serializer, PropertyNames.Headers, request.Headers, true);
            this.WriteSender(writer, serializer, request);
            this.WritePrimaryRecipient(writer, serializer, request);
            this.WriteCcRecipients(writer, serializer, request);
            this.WriteBccRecipients(writer, serializer, request);
            this.WriteInlineAttachment(writer, serializer, request);
            this.WriteFileAttachments(writer, serializer, request);

            writer.WriteEndObject();
        }

        protected internal virtual void WriteSender(JsonWriter writer, SerializerProxy serializer, ISendRequest request)
        {
            if (String.IsNullOrEmpty(request.SenderName) &&
                String.IsNullOrEmpty(request.SenderAddress) &&
                String.IsNullOrEmpty(request.SenderReplyTo))
            {
                return;
            }
            
            writer.WritePropertyName(PropertyNames.Sender);
            writer.WriteStartObject();
            this.WriteProperty(writer, serializer, PropertyNames.Name, request.SenderName, true);
            this.WriteProperty(writer, serializer, PropertyNames.Address, request.SenderAddress, true);
            this.WriteProperty(writer, serializer, PropertyNames.ReplyTo, request.SenderReplyTo, true);
            writer.WriteEndObject();
        }

        protected internal virtual void WritePrimaryRecipient(JsonWriter writer, SerializerProxy serializer, ISendRequest request)
        {
            writer.WritePropertyName(PropertyNames.Recipient);
            writer.WriteStartObject();
            this.WriteProperty(writer, serializer, PropertyNames.Name, request.RecipientName, true);
            this.WriteProperty(writer, serializer, PropertyNames.Address, request.RecipientAddress, false);
            writer.WriteEndObject();
        }

        protected internal virtual void WriteCcRecipients(JsonWriter writer, SerializerProxy serializer, ISendRequest request)
        {
            this.WriteRecipientsList(writer, serializer, PropertyNames.CopyTo, request.CopyTo);
        }

        protected internal virtual void WriteBccRecipients(JsonWriter writer, SerializerProxy serializer, ISendRequest request)
        {
            this.WriteRecipientsList(writer, serializer, PropertyNames.BlindCopyTo, request.BlindCopyTo);
        }

        protected internal virtual void WriteRecipientsList(JsonWriter writer, SerializerProxy serializer, string name, IEnumerable<string> recipients)
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
                this.WriteProperty(writer, serializer, PropertyNames.Address, address, false);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        protected internal virtual void WriteInlineAttachment(JsonWriter writer, SerializerProxy serializer, ISendRequest request)
        {
            if (request.InlineAttachment == null)
            {
                return;
            }

            writer.WritePropertyName(PropertyNames.InlineAttachment);
            this.WriteAttachmentObject(writer, serializer, request.InlineAttachment);
        }

        protected internal virtual void WriteFileAttachments(JsonWriter writer, SerializerProxy serializer, ISendRequest request)
        {
            if (request.FileAttachments == null)
            {
                return;
            }

            writer.WritePropertyName(PropertyNames.FileAttachments);
            writer.WriteStartArray();

            foreach (var attachment in request.FileAttachments)
            {
                this.WriteAttachmentObject(writer, serializer, attachment);
            }

            writer.WriteEndArray();
        }

        protected void WriteAttachmentObject(JsonWriter writer, SerializerProxy serializer, IAttachment attachment)
        {
            writer.WriteStartObject();
            this.WriteProperty(writer, serializer, PropertyNames.AttachmentId, attachment.Id, false);
            this.WriteProperty(writer, serializer, PropertyNames.AttachmentData, attachment.Data, false);
            writer.WriteEndObject();
        }
    }
}
