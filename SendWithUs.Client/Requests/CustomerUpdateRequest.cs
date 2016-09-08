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
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the data necessary to make API requests to send email messages.
    /// </summary>
    [JsonConverter(typeof(CustomerUpdateRequestConverter))]
    public class CustomerUpdateRequest : ICustomerUpdateRequest
    {
        #region Constructors

        public CustomerUpdateRequest()
        { }

        public CustomerUpdateRequest(string email) : this(email, (IEnumerable<KeyValuePair< string, object >>)null)
        { }

        public CustomerUpdateRequest(string email, IEnumerable<KeyValuePair<string, object>> data)
        {
            EnsureArgument.NotNullOrEmpty(email, "email");

            this.Email = email;
            this.Data = data;
        }

        #endregion

        #region ISendRequest members

        public virtual string Email { get; set; }

        public virtual IEnumerable<KeyValuePair<string, object>> Data { get; set; }

        public virtual IEnumerable<string> Groups { get; set; }

        public virtual string Locale { get; set; }

        #endregion

        #region IRequest members

        public string GetUriPath()
        {
            return "/api/v1/customers";
        }

        public string GetHttpMethod()
        {
            return "POST";
        }

        public Type GetResponseType()
        {
            return typeof(CustomerUpdateResponse);
        }

        public virtual IRequest Validate()
        {
            // Customer email is required.
            if (String.IsNullOrEmpty(this.Email))
            {
                throw new ValidationException(ValidationFailureMode.MissingCustomerAddress);
            }

            return this;
        }

        public virtual bool IsValid
        {
            get
            {
                try
                {
                    this.Validate();
                }
                catch (ValidationException)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion
    }
}
