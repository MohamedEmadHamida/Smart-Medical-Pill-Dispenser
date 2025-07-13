using HealthCare.Project.Core.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace HealthCare.Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _geminiApiKey;

        public GeminiController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _geminiApiKey = configuration["GeminiAI:ApiKey"];
        }

        [HttpPost("get-tips")]
        public async Task<IActionResult> GetSymptomTips([FromBody] SymptomInputModel userInput)
        {
            if (string.IsNullOrEmpty(userInput.Symptoms))
                return BadRequest("Please provide symptoms.");

            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = $"I have the following symptoms: {userInput.Symptoms}. Provide only health tips related to these symptoms." }
                }
            }
        }
            };

            var requestJson = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_geminiApiKey}";

            var response = await _httpClient.PostAsync(requestUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, responseContent);

            try
            {
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

                if (jsonResponse.TryGetProperty("candidates", out var candidatesArray) && candidatesArray.GetArrayLength() > 0)
                {
                    var firstCandidate = candidatesArray[0];

                    if (firstCandidate.TryGetProperty("content", out var contentObj) &&
                        contentObj.TryGetProperty("parts", out var partsArray) && partsArray.GetArrayLength() > 0)
                    {
                        var rawText = partsArray[0].GetProperty("text").GetString();

                        // Convert to list format
                        var tipsList = rawText.Split('\n')
                                              .Where(line => !string.IsNullOrWhiteSpace(line))
                                              .Select(line => line.Replace("**", "").Replace("*", "").Trim()) // Remove "**", "*", and trim spaces
                                              .ToList();



                        return Ok(new
                        {
                            status = "success",
                            generatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            source = "Gemini AI",
                            tips = tipsList
                        });
                    }
                }

                return StatusCode(500, "Error extracting health tips from the response.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing response from Gemini: {ex.Message}");
            }

        }

    }
}
