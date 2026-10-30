using System;
using JustMyPackages.Serialization.NewtonsoftJson;
using Newtonsoft.Json;

namespace JustMyPackages.Serialization.Tests
{
    public sealed class NewtonsoftSerializationTest : SerializationTest
    {
        private readonly JsonSerializerSettings serializerSettings = DefaultJsonSerializerSettings.Create();

        protected override string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, serializerSettings);
        }

        protected override T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str, serializerSettings) ?? throw new ArgumentNullException();
        }
    }
}