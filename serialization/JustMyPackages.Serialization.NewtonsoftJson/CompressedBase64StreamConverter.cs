using System;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace JustMyPackages.Serialization.NewtonsoftJson
{
    internal class CompressedBase64StreamConverter : JsonConverter<Stream>
    {
        public override Stream? ReadJson(JsonReader reader, Type objectType, Stream? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                throw new ArgumentNullException(nameof(reader.Value), "CompressedBase64StreamConverter.ReadJson called with null when it is not supposed to.");
            }

            using var compressedStream = new MemoryStream(Convert.FromBase64String(reader.Value as string ?? string.Empty));
            using var decompressor = new GZipStream(compressedStream, CompressionMode.Decompress);
            var outputStream = new MemoryStream();
            decompressor.CopyTo(outputStream);
            outputStream.Position = 0;
            return outputStream;
        }

        public override void WriteJson(JsonWriter writer, Stream? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "CompressedBase64StreamConverter.WriteJson called with null when it is not supposed to.");
            }

            using var compressedStream = new MemoryStream();
            using (var compressor = new GZipStream(compressedStream, CompressionMode.Compress, true))
            {
                value.CopyTo(compressor);
            }

            Span<byte> compressedBytes = compressedStream.GetBuffer().AsSpan(0, (int)compressedStream.Length);
            string compressedBase64Bytes = Convert.ToBase64String(compressedBytes);
            writer.WriteValue(compressedBase64Bytes);
        }
    }
}