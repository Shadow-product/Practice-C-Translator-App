using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json; // для JsonConvert.DeserializeObject
using Newtonsoft.Json.Linq; // для работы с JObject (динамический JSON)
using WindowsFormsApp1.Models; // собственные модели: User, Translation
using WindowsFormsApp1.Repositories; // бизнес-логика и работа с API (TranslationService)

namespace WindowsFormsApp1.Services
{
    public class TranslationService
    {
        private readonly string _apiKey;

        public TranslationService(string apiKey)
        {
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];
        }

        public async Task<Translation> TranslateText(string text, string targetLang, int userId)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("text", text),
            new KeyValuePair<string, string>("target_lang", targetLang)
        });

                client.DefaultRequestHeaders.Add("Authorization", $"DeepL-Auth-Key {_apiKey}");

                HttpResponseMessage response = await client.PostAsync(
                    "https://api-free.deepl.com/v2/translate",
                    content
                );

                string json = await response.Content.ReadAsStringAsync();

                var obj = JObject.Parse(json);
                var translations = obj["translations"] as JArray;

                if (translations != null && translations.Count > 0)
                {
                    return new Translation
                    {
                        SourceText = text,
                        TranslatedText = translations[0]["text"]?.ToString() ?? "Нет текста",
                        DetectedLanguage = translations[0]["detected_source_language"]?.ToString() ?? "Неизвестно",
                        TargetLanguage = targetLang,
                        CreatedAt = DateTime.Now,
                        UserId = userId
                    };
                }

                return new Translation
                {
                    SourceText = text,
                    TranslatedText = "Нет перевода",
                    DetectedLanguage = "Неизвестно",
                    TargetLanguage = targetLang,
                    CreatedAt = DateTime.Now,
                    UserId = userId
                };
            }
        }
    }
}