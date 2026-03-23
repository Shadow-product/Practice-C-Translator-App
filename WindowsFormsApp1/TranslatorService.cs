using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
public class TranslationService
{
    private readonly string apiKey = "ТВОЙ_API_KEY";

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

            return await response.Content.ReadAsStringAsync();
        }
    }
}