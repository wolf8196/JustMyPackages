using MessagePack;
using MessagePack.Resolvers;

namespace JustMyPackages.Serialization.MessagePack
{
    public static class DefaultMessagePackSerializerOptions
    {
        public static MessagePackSerializerOptions Create()
        {
            var resolver = CompositeResolver.Create(
                CompressedStreamResolver.Instance,
                NativeDateTimeResolver.Instance,
                NativeGuidResolver.Instance,
                NativeDecimalResolver.Instance,
                StandardResolver.Instance);

            var options = MessagePackSerializerOptions.Standard.WithResolver(resolver);
            return options;
        }
    }
}