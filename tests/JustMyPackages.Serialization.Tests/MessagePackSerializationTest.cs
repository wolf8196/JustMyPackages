using System;
using JustMyPackages.Serialization.MessagePack;
using MessagePack;

namespace JustMyPackages.Serialization.Tests
{
    public sealed class MessagePackSerializationTest : SerializationTest
    {
        private readonly MessagePackSerializerOptions options = DefaultMessagePackSerializerOptions.Create();

        protected override string Serialize<T>(T obj)
        {
            return Convert.ToBase64String(MessagePackSerializer.Serialize(obj, options));
        }

        protected override T Deserialize<T>(string str)
        {
            return MessagePackSerializer.Deserialize<T>(Convert.FromBase64String(str), options);
        }
    }
}