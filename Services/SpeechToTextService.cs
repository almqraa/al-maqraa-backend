using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Al_Maqraa.DTO;
namespace Al_Maqraa.Services
{
    public class SpeechToTextService : ISpeechToTextRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _pythonApiUrl;

        public SpeechToTextService(HttpClient httpClient, string pythonApiUrl= "http://cnn-transcribe-api.eastus.azurecontainer.io:8000/transcribe")
        {

            //connection will be opened here with socket flask
            _httpClient = httpClient;
            _pythonApiUrl = pythonApiUrl;
        }
        // this should byte array
        public async Task<string> ConvertToText(string audioData)
        {
            // Convert byte array to base64 string
            // string base64Audio = Convert.ToBase64String(audioData);

            // Prepare request body
            var requestBody = new
            {
                audio = audioData
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make HTTP POST request to Python API
            var response = await _httpClient.PostAsync(_pythonApiUrl, content);

            // Handle response
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<SpeechToTextResponse>(result);
                string text = responseObject?.transcription ?? "";
                return text;
            }
            else
            {
                // Handle error
                // For now, return an empty string
                return string.Empty;
            }
        }
    }
}
