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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using System.Linq;

    public class TestData : DynamicObject
    {
        protected const string DataDirectoryName = "EndToEnd/Data";

        protected IDictionary<string, object> DataSource { get; set; }

        protected TestData(string pathName)
        {
            this.DataSource = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(pathName), new DictionaryConverter());
        }
        
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.DataSource.TryGetValue(binder.Name, out result);
        }
        
        protected class DictionaryConverter : CustomCreationConverter<IDictionary<string, Object>>
        {
            public override IDictionary<string, object> Create(Type objectType)
            {
                return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(object) || base.CanConvert(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.Null)
                {
                    return base.ReadJson(reader, objectType, existingValue, serializer);
                }

                // Fall back on standard deserializer.
                return serializer.Deserialize(reader);
            }
        }

        public static string GetApiKey() => File.ReadAllLines($"{DataDirectoryName}/ApiKey.txt").FirstOrDefault();

        public static TestData Load(string fileName) => new TestData($"{DataDirectoryName}/{fileName}.json");
    }
}
