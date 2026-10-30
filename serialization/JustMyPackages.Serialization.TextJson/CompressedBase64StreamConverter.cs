using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JustMyPackages.Serialization.TextJson
{
    internal class CompressedBase64StreamConverter : JsonConverter<Stream>
    {
        public override Stream Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                return new MemoryStream();
            }

            using var compressedStream = new MemoryStream(reader.GetBytesFromBase64());
            using var decompressor = new GZipStream(compressedStream, CompressionMode.Decompress);
            var outputStream = new MemoryStream();
            decompressor.CopyTo(outputStream);
            outputStream.Position = 0;
            return outputStream;
        }

        public override void Write(Utf8JsonWriter writer, Stream value, JsonSerializerOptions options)
        {
            using var compressedStream = new MemoryStream();
            using (var compressor = new GZipStream(compressedStream, CompressionMode.Compress, true))
            {
                value.CopyTo(compressor);
            }

            Span<byte> compressedBytes = compressedStream.GetBuffer().AsSpan(0, (int)compressedStream.Length);
            writer.WriteBase64StringValue(compressedBytes);
        }
    }
}