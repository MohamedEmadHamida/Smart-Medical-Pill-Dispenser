using HealthCare.Project.Core.Dtos;
using HealthCare.Project.Core.Repository.Contract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare.Project.Service.Services.DrugInteractions
{
    public class GeminiInteractionChecker : IInteractionChecker
    {
        private readonly HttpClient _httpClient;
        private readonly string _geminiApiKey;

        public GeminiInteractionChecker(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _geminiApiKey = config["GeminiAI:ApiKey"];
        }
        
            public async Task<InteractionResult> CheckInteractionAsync(string medicine1, string medicine2)
            {
                var prompt = $"Do {medicine1} and {medicine2} have any known medical interaction? " +
                             $"If yes, explain the cause briefly. If no, just say 'No interaction found'.";

                var body = new
                {
                    contents = new[]
                    {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
                };

                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"https://generativelanguage.googleapis.com/v1/models/gemini-1.5-pro:generateContent?key={_geminiApiKey}");

                request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                // Handle non-200 responses
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error from Gemini API: {response.StatusCode} - {json}");
                }

                Console.WriteLine($"Response JSON: {json}");

                // Safe JSON property access
                var jsonDoc = JsonDocument.Parse(json);
                if (jsonDoc.RootElement.TryGetProperty("candidates", out var candidates) &&
                    candidates[0].TryGetProperty("content", out var content) &&
                    content.TryGetProperty("parts", out var parts) &&
                    parts[0].TryGetProperty("text", out var text))
                {
                    var resultText = text.GetString();

                // Remove newlines (\n) from the result text
                //resultText = resultText.Replace("\n", " ");


                bool hasInteraction = !string.IsNullOrWhiteSpace(resultText) &&
                                          !resultText.Contains("no interaction", StringComparison.OrdinalIgnoreCase);

                    return new InteractionResult
                    {
                        InteractionFound = hasInteraction,
                        Description = resultText ?? "No interaction found"
                    };
                }

                throw new Exception("Invalid response structure from Gemini API");
            }


        }
    }


