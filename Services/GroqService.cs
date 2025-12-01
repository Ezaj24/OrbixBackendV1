using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace OrbixBackend.Services
{
    public class GroqService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public GroqService(IConfiguration config)
        {
            _http = new HttpClient();

            // 🔥 FIX: use Render environment variable first
            _apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY")
                      ?? config["Groq:ApiKey"]
                      ?? "";

            _http.BaseAddress = new Uri("https://api.groq.com/openai/v1/");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> GenerateAsync(string systemPrompt, string userMessage)
        {
            var payload = new
            {
                // ✅ Always force FREE model
                model = "llama-3.1-8b-instant",
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userMessage }
                }
            };

            var json = JsonSerializer.Serialize(payload);

            HttpResponseMessage response;

            try
            {
                response = await _http.PostAsync(
                    "chat/completions",
                    new StringContent(json, Encoding.UTF8, "application/json")
                );
            }
            catch
            {
                return "ORBIX: My AI engine is temporarily unavailable due to a connection issue. No charges were made.";
            }

            var result = await response.Content.ReadAsStringAsync();

            // 🚨 If API call failed → analyze error safely
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    using var errorDoc = JsonDocument.Parse(result);

                    if (errorDoc.RootElement.TryGetProperty("error", out var errorObj))
                    {
                        string errorMsg = errorObj.GetProperty("message").GetString() ?? "";

                        // 🔒 FREE-SAFETY LAYER
                        if (errorMsg.Contains("rate limit", StringComparison.OrdinalIgnoreCase) ||
                            errorMsg.Contains("billing", StringComparison.OrdinalIgnoreCase) ||
                            errorMsg.Contains("model_decommissioned", StringComparison.OrdinalIgnoreCase) ||
                            errorMsg.Contains("insufficient", StringComparison.OrdinalIgnoreCase))
                        {
                            return "ORBIX: Free usage limit or backend AI issue detected. No charges were made. Please try again shortly.";
                        }
                    }
                }
                catch { }

                return "ORBIX: An unexpected AI backend error occurred. No charges were made. Try again later.";
            }

            // Extract valid AI response
            using var okDoc = JsonDocument.Parse(result);
            return okDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "";
        }
    }
}
