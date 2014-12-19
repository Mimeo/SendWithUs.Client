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
    using System.Collections.Generic;

    /// <summary>
    /// Describes the interface of objects used to make API requests to send email messages.
    /// </summary>
    /// <remarks>This interface roughly corresponds to the JSON object used with the /send route
    /// in the REST API, with some properties flattened or renamed.</remarks>
    public interface ISendRequest : IRequest
    {
        /// <summary>
        /// Gets the unique identifier of an email template.
        /// </summary>
        /// <remarks>Corresponds to the "email_id" property in the SendWithUs API.</remarks>
        string TemplateId { get; }

        /// <summary>
        /// Gets the email address of the message sender.
        /// </summary>
        string SenderAddress { get; }

        /// <summary>
        /// Gets the name of the message sender.
        /// </summary>
        string SenderName { get; }

        /// <summary>
        /// Gets the email address to which replies should be sent.
        /// </summary>
        string SenderReplyTo { get; }

        /// <summary>
        /// Gets the email address of the message recipient.
        /// </summary>
        string RecipientAddress { get; }

        /// <summary>
        /// Gets the name of the message recipient.
        /// </summary>
        string RecipientName { get; }

        /// <summary>
        /// Gets the set of "carbon copy" email addresses.
        /// </summary>
        /// <remarks>Corresponds to the "cc" property in the SendWithUs API.</remarks>
        IEnumerable<string> CopyTo { get; }

        /// <summary>
        /// Gets the set of "blind carbon copy" email addresses.
        /// </summary>
        /// <remarks>Corresponds to the "bcc" property in the SendWithUs API.</remarks>
        IEnumerable<string> BlindCopyTo { get; }

        /// <summary>
        /// Gets the data to use when expanding the message template.
        /// </summary>
        /// <remarks>Corresponds to the "email_data" property in the SendWithUs API.</remarks>
        IDictionary<string, string> Data { get; }

        /// <summary>
        /// Gets the set of tags to associate with the message.
        /// </summary>
        IEnumerable<string> Tags { get; }

        /// <summary>
        /// Gets an attachment object.
        /// </summary>
        /// <remarks>Corresponds to the "inline." property in the SendWithUs API.</remarks>
        IAttachment InlineAttachment { get; }

        /// <summary>
        /// Gets a set of attachment objects.
        /// </summary>
        /// <remarks>Corresponds to the "files" property in the SendWithUs API.</remarks>
        IEnumerable<IAttachment> FileAttachments { get; }

        /// <summary>
        /// Gets the unique identifier for the provider of email transport services.
        /// </summary>
        /// <remarks>Corresponds to the "esp_account" property in the SendWithUs API.</remarks>
        string ProviderId { get; }

        /// <summary>
        /// Gets the unique identifier for a version of the template identified by TemplateId.
        /// </summary>
        /// <remarks>Corresponds to the "version_name" property in the SendWithUs API.</remarks>
        string TemplateVersionId { get; }

        /// <summary>
        /// Gets a value indicating whether the request object is valid (well-formed).
        /// </summary>
        bool IsValid { get; }
    }
}
