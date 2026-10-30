using System.Text.Json;
using System.Text.Json.Serialization;

namespace JustMyPackages.Serialization.TextJson
{
    public static class DefaultJsonSerializerOptions
    {
        public static JsonSerializerOptions Create()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            Configure(options);
            return options;
        }

        public static JsonSerializerOptions Configure(JsonSerializerOptions options)
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.DictionaryKeyPolicy = null;
            options.PropertyNameCaseInsensitive = true;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            options.IgnoreReadOnlyProperties = false;
            options.IgnoreReadOnlyFields = true;
            options.IncludeFields = false;
            options.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
            options.WriteIndented = false;
            options.AllowTrailingCommas = false;
            options.UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip;
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.ReadCommentHandling = JsonCommentHandling.Skip;
            options.Converters.Add(new CompressedBase64StreamConverter());
            return options;
        }
    }
}
