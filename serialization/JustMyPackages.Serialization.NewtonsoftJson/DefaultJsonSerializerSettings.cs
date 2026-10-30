using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JustMyPackages.Serialization.NewtonsoftJson
{
    public static class DefaultJsonSerializerSettings
    {
        public static JsonSerializerSettings Create()
        {
            var settings = new JsonSerializerSettings();
            Configure(settings);
            return settings;
        }

        public static JsonSerializerSettings Configure(JsonSerializerSettings settings)
        {
            settings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = false,
                    OverrideSpecifiedNames = false,
                    ProcessExtensionDataNames = false,
                },
                IgnoreIsSpecifiedMembers = true,
                IgnoreSerializableAttribute = true,
                IgnoreSerializableInterface = true,
                IgnoreShouldSerializeMembers = true,
                SerializeCompilerGeneratedMembers = false,
            };
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Error;
            settings.TypeNameHandling = TypeNameHandling.None;
            settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
            settings.FloatFormatHandling = FloatFormatHandling.String;

            settings.Converters.Add(new CompressedBase64StreamConverter());
            return settings;
        }
    }
}