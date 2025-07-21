using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlackBoxAI.VSExtension.Services
{
    public class AIService
    {
        private readonly HttpClient httpClient;
        private readonly SettingsService settingsService;

        public AIService()
        {
            httpClient = new HttpClient();
            settingsService = new SettingsService();
        }

        public async Task<string> SendMessageAsync(string message)
        {
            string apiKey = settingsService.GetApiKey();
            if (string.IsNullOrEmpty(apiKey))
            {
                return "Please configure your API key in settings first.";
            }

            try
            {
                var requestData = new
                {
                    model = "blackbox-ai",
                    messages = new[]
                    {
                        new { role = "user", content = message }
                    },
                    max_tokens = 1000,
                    temperature = 0.7
                };

                string json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var response = await httpClient.PostAsync("https://api.blackbox.ai/v1/chat/completions", content);
                
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    return responseData.choices[0].message.content.ToString();
                }
                else
                {
                    return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                return $"Error communicating with BlackBox AI: {ex.Message}";
            }
        }
    }
}
