using System;
using System.Collections.Generic;

namespace Natukaship
{
    public class LanguageItem
    {
        public string identifier { get; set; }
        public List<Language> originalArray { get; set; }

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        public object SetValue(string langKey, string value) => this[langKey] = value;
        public object GetValue(string langKey) => this[langKey];

        public LanguageItem(string identifier, List<Language> refLang)
        {
            if (refLang == null)
                throw new Exception("refLang is nil");

            identifier = identifier;
            originalArray = refLang;
        }

        public Language GetLang(string lang)
        {
            // Apple being consistent
            Language result = originalArray.Find(current => current.language == lang || current.localeCode == lang);

            if (result != null)
                return result;

            throw new Exception($"Language {lang} is not activated / available for this app version");
        }

        // @return (Array) An array containing all languages that are already available
        private List<string> _keys { get; set; }
        public List<string> keys
        {
            get
            {
                if (_keys != null && _keys.Count > 0)
                    return _keys;

                originalArray.ForEach(current =>
                {
                    if (current.language != null)
                        _keys.Add(current.language);
                    else
                        _keys.Add(current.localeCode);
                });

                return _keys;
            }
        }

        // @return (Array) An array containing all languages that are already available
        //  alias for keys
        public List<string> languages
        {
            get { return keys; }
        }
    }
}
