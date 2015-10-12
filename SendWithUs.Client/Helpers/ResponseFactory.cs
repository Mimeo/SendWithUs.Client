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
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Reflection;

    public class ResponseFactory : IResponseFactory
    {
        private static readonly TypeInfo IResponseTypeInfo = typeof(IResponse).GetTypeInfo();
        private static readonly TypeInfo ICollectionResponseTypeInfo = typeof(ICollectionResponse).GetTypeInfo();
        private static readonly TypeInfo ICollectionItemTypeInfo = typeof(ICollectionItem).GetTypeInfo();
        
        #region IResponseFactory Methods

        public TResponse Create<TResponse>(HttpStatusCode statusCode, JToken json)
            where TResponse : class, IResponse
            => this.CreateResponse(typeof(TResponse), statusCode, json) as TResponse;

        public TCollectionResponse Create<TCollectionResponse, TCollectionItem>(HttpStatusCode statusCode, JToken json)
            where TCollectionResponse : class, ICollectionResponse
            where TCollectionItem : class, ICollectionItem 
            => this.CreateResponse(typeof(TCollectionResponse), typeof(TCollectionItem), statusCode, json) as TCollectionResponse;

        public virtual IResponse CreateResponse(Type responseType, HttpStatusCode statusCode, JToken json)
        {
            EnsureArgument.NotNull(responseType, nameof(responseType));
            this.ValidateInterfaceImplementation(responseType, IResponseTypeInfo);
            return this.BuildInstance<IResponse>(responseType).Initialize(this, statusCode, json);
        }

        public virtual ICollectionResponse CreateResponse(Type collectionType, Type collectionItemType, HttpStatusCode statusCode, JToken json)
        {
            EnsureArgument.NotNull(collectionType, nameof(collectionType));
            EnsureArgument.NotNull(collectionItemType, nameof(collectionItemType));
            this.ValidateInterfaceImplementation(collectionType, ICollectionResponseTypeInfo);
            this.ValidateInterfaceImplementation(collectionItemType, ICollectionItemTypeInfo);
            return this.BuildInstance<ICollectionResponse>(collectionType).Initialize(this, statusCode, json, collectionItemType);
        }

        public ICollectionItem CreateItem(Type itemType, JToken json)
        {
            return this.BuildInstance<ICollectionItem>(itemType).Initialize(this, json);
        }

        public IBatchResponse CreateBatchResponse(HttpStatusCode statusCode, JToken json, IEnumerable<Type> itemTypes)
        {
            EnsureArgument.NotNullOrEmpty<Type>(itemTypes, nameof(itemTypes), false);
            return this.BuildInstance<IBatchResponse>(typeof(BatchResponse)).Initialize(this, statusCode, json, itemTypes);
        }

        #endregion

        #region Helpers

        protected void ValidateInterfaceImplementation(Type responseType, TypeInfo requiredInterface)
        {
            if (!requiredInterface.IsAssignableFrom(responseType.GetTypeInfo()))
            {
                throw new InvalidOperationException($"Type '{responseType.FullName}' does not implement '{requiredInterface.FullName}'.");
            }
        }
        
        protected TReturn BuildInstance<TReturn>(Type type) where TReturn : class
        {
            return Activator.CreateInstance(type) as TReturn;
        }

        #endregion
    }
}
