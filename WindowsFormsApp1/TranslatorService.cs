using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
public class TranslationService
{
    private readonly string apiKey = "ТВОЙ_API_KEY";

    public async Task<string> TranslateText(string text, string targetLang)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://translation.googleapis.com/language/translate/v2?key={apiKey}";

            var jsonBody = new
            {
                q = text,
                target = targetLang,
                format = "text"
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            return ExtractTranslation(responseBody);
        }
    }

    private string ExtractTranslation(string json)
    {
        var obj = JObject.Parse(json);
        return obj["data"]["translations"][0]["translatedText"].ToString();
    }
}