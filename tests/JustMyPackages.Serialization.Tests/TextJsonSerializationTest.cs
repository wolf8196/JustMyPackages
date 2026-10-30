using System;
using System.Text.Json;
using JustMyPackages.Serialization.TextJson;

namespace JustMyPackages.Serialization.Tests
{
    public sealed class TextJsonSerializationTest : SerializationTest
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = DefaultJsonSerializerOptions.Create();

        protected override string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, jsonSerializerOptions);
        }

        protected override T Deserialize<T>(string str)
        {
            return JsonSerializer.Deserialize<T>(str, jsonSerializerOptions) ?? throw new ArgumentNullException();
        }
    }
}