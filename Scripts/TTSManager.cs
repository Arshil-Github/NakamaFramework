using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TTSManager : MonoBehaviour
{
    private string apiKey = "YOUR_GOOGLE_CLOUD_API_KEY";
    private string apiUrl = "https://texttospeech.googleapis.com/v1/text:synthesize?key=";

    public async Task<string> ConvertTextToSpeech(string text)
    {
        var client = new HttpClient();
        var requestBody = new
        {
            input = new { text = text },
            voice = new { languageCode = "en-US", name = "en-US-Wavenet-D" },
            audioConfig = new { audioEncoding = "MP3" }
        };

        string json = JsonUtility.ToJson(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(apiUrl + apiKey, content);
        var result = await response.Content.ReadAsStringAsync();

        return result; // Process the returned audio file (base64 encoded) here
    }
}
