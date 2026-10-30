using System;
using System.IO;
using System.IO.Compression;
using CommunityToolkit.HighPerformance;
using MessagePack;
using MessagePack.Formatters;

namespace JustMyPackages.Serialization.MessagePack
{
    public class CompressedStreamFormatter : IMessagePackFormatter<Stream?>
    {
        public void Serialize(ref MessagePackWriter writer, Stream? value, MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            using var compressedStream = new MemoryStream();
            using (var compressor = new GZipStream(compressedStream, CompressionMode.Compress, true))
            {
                value.CopyTo(compressor);
            }

            Span<byte> compressedBytes = compressedStream.GetBuffer().AsSpan(0, (int)compressedStream.Length);
            writer.WriteBinHeader(compressedBytes.Length);
            writer.WriteRaw(compressedBytes);
        }

        public Stream? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            var bytes = reader.ReadBytes();
            if (!bytes.HasValue)
            {
                return null;
            }

            using var compressedStream = bytes.Value.AsStream();
            using var decompressor = new GZipStream(compressedStream, CompressionMode.Decompress);
            var outputStream = new MemoryStream();
            decompressor.CopyTo(outputStream);
            outputStream.Position = 0;
            return outputStream;
        }
    }
}