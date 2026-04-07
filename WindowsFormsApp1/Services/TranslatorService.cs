using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; // для JsonConvert.DeserializeObject
using Newtonsoft.Json.Linq; // для работы с JObject (динамический JSON)
using WindowsFormsApp1.Models; // собственные модели: User, Translation
using WindowsFormsApp1.Repositories; // бизнес-логика и работа с API (TranslationService)

namespace WindowsFormsApp1.Services
{
    public class TranslationService
    {
        private readonly string apiKey;

        public TranslationService()
        {
            apiKey = ConfigurationManager.AppSettings["ApiKey"];
        }

        public async Task<string> TranslateText(string text, string targetLang)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("text", text),
                new KeyValuePair<string, string>("target_lang", targetLang)
            });

                client.DefaultRequestHeaders.Add("Authorization", $"DeepL-Auth-Key {apiKey}");

                HttpResponseMessage response = await client.PostAsync(
                    "https://api-free.deepl.com/v2/translate",
                    content
                );

                string json = await response.Content.ReadAsStringAsync();

                var obj = JObject.Parse(json);
                return obj["translations"][0]["text"].ToString();
            }
        }
    }
}