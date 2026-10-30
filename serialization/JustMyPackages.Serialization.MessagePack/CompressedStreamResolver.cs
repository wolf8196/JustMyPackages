using System.IO;
using MessagePack;
using MessagePack.Formatters;

namespace JustMyPackages.Serialization.MessagePack
{
    public class CompressedStreamResolver : IFormatterResolver
    {
        private static readonly IMessagePackFormatter<Stream?> Formatter = new CompressedStreamFormatter();
        public static readonly IFormatterResolver Instance = new CompressedStreamResolver();

        private CompressedStreamResolver()
        {
        }

        public IMessagePackFormatter<T>? GetFormatter<T>()
        {
            return typeof(T) == typeof(Stream) ? (IMessagePackFormatter<T>)Formatter : null;
        }
    }
}