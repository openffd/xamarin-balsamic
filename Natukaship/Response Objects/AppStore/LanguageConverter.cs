using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Natukaship
{
    public class LanguageConverter
    {
        //public Dictionary

        // Converts the iTC format (English_CA, Brazilian Portuguese) to language short codes: (en-US, de-DE)
        public static string FromItcToStandard(string fromLang)
        {
            JToken result = Mapping.Where(item => item.Value<string>("name") == fromLang).Select(item => item).First();

            return result.Value<string>("locale");
        }

        // Converts the Language "UK English" to itc locale en-GB
        public static string FromStandardToItcLocale(string fromLang)
        {
            JToken result = Mapping.Where(item => item.Value<string>("name") == fromLang).Select(item => item).First();

            return result.Value<string>("itc_locale") ?? result.Value<string>("locale");
        }

        // Converts the language short codes: (en-US, de-DE) to the iTC format (English_CA, Brazilian Portuguese)
        public static string FromStandardToItc(string fromLang)
        {
            JToken result = Mapping.Where(item => item.Value<string>("locale") == fromLang || item.Values("alternatives").Contains(fromLang)).Select(item => item).First();

            return result.Value<string>("name");
        }

        // Converts the language "UK English" (user facing) to "English_UK" (value)
        public static KeyValuePair<string, string> FromItcReadableToItc(string fromLang)
        {
            if (ReadableMapping.Values.Contains(fromLang))
                return readableMapping.Where(item => item.Value == fromLang).Select(item => item).First();

            return new KeyValuePair<string, string>("", "");
        }

        // Converts the language "English_UK" (value) to "UK English" (user facing)
        public static string FromItcToItcReadable(string fromLang)
        {
            return readableMapping[fromLang];
        }

        //private

        // Get the mapping JSON parsed
        private static JArray mapping = new JArray();
        public static JArray Mapping
        {
            get
            {
                if (mapping.Count > 0)
                    return mapping;

                string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Natukaship", "Assets/languageMapping.json"));
                var result = JArray.Parse(File.ReadAllText(path));

                return result;
            }
        }

        private static Dictionary<string, string> readableMapping = new Dictionary<string, string>();
        public static Dictionary<string, string> ReadableMapping
        {
            get
            {
                if (readableMapping.Count > 0)
                    return readableMapping;

                string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Natukaship", "Assets/languageMappingReadable.json"));
                readableMapping = JObject.Parse(File.ReadAllText(path)).ToObject<Dictionary<string, string>>();

                return readableMapping;
            }
        }
    }

    public static class LanguageConverterExtension
    {
        public static string ToLanguageCode(this string str)
        {
            return LanguageConverter.FromItcToStandard(str);
        }

        public static string ToItcLocale(this string str)
        {
            return LanguageConverter.FromStandardToItcLocale(str);
        }

        public static string ToFullLanguage(this string str)
        {
            return LanguageConverter.FromStandardToItc(str);
        }
    }
}
