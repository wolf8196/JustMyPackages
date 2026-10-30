using MessagePack;
using MessagePack.Resolvers;

namespace JustMyPackages.Serialization.MessagePack
{
    public static class DefaultMessagePackSerializerOptions
    {
        /// <summary>
        /// Creates my default settings for MessagePack serialization.
        /// </summary>
        /// <returns>The configured settings.</returns>
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