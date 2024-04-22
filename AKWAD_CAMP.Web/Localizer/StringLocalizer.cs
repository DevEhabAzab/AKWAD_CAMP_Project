using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace AKWAD_CAMP.Web.Localizer
{
    public class StringLocalizer : IStringLocalizer
    {
        private readonly JsonSerializer _serializer = new();
        private readonly IDistributedCache _cashe;

        public StringLocalizer(IDistributedCache cashe)
        {
            _cashe = cashe;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetValue(name);
                return new LocalizedString(name, value);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var value = this[name];
                return !value.ResourceNotFound ?
                       new LocalizedString(name, String.Format(value.Value, arguments)) :
                       value;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
            var fullPath = Path.GetFullPath(filePath);
            using FileStream fileStream = new(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new(fileStream);
            using JsonTextReader jsonTextReader = new(streamReader);

            while (jsonTextReader.Read())
            {
                if (jsonTextReader.TokenType != JsonToken.PropertyName)
                    continue;
                var Key = jsonTextReader.Value as string;
                jsonTextReader.Read();
                var value = _serializer.Deserialize<string>(jsonTextReader);
                yield return new LocalizedString(Key, value);
            }
        }


        private string GetValue(string Key)
        {
            
            var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
            var fullPath = Path.GetFullPath(filePath);
            if (File.Exists(fullPath))
            {
                var casheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{Key}";
                var casheValue = _cashe.GetString(casheKey);
                if (!string.IsNullOrEmpty(casheValue))
                    return casheValue;
                
                var result = GetValueFromJson(Key,fullPath);
                if(result != null)
                    _cashe.SetString(casheKey, result);
                return result;
            }
            return string.Empty;
        }


        private string GetValueFromJson(string Key , string FilePath)
        {
            if(string.IsNullOrEmpty(Key) || string.IsNullOrEmpty(FilePath))
                return string.Empty;

            using FileStream fileStream = new(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new(fileStream);
            using JsonTextReader jsonTextReader = new(streamReader);

            while (jsonTextReader.Read())
            {
                if(jsonTextReader.TokenType == JsonToken.PropertyName && jsonTextReader.Value as string == Key)
                {
                    jsonTextReader.Read();
                    return _serializer.Deserialize<string>(jsonTextReader);
                }
            }
            return string.Empty;
        }
    }
}
