using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;

namespace SimpleSocialApp.External.AI;

public class OpenAIService : IOpenAIService, IDisposable
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    private const string Model = "gpt-3.5";
    private const int Temperature = 1;
    private const int MaxTokens = 2048;
    private const int TopP = 1;
    private const int FrequencyPenalty = 0;
    private const int PresencePenalty = 0;

    public OpenAIService(IConfiguration config)
    {
        _apiKey = "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";
        _httpClient = new HttpClient { BaseAddress = new Uri("https://api.openai.com") };
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<string?> Prompt(string message, string responseFormat = "text")
    {
        var payload = new StringContent(CreatePayload(message, responseFormat), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("v1/chat/completions", payload);
        var responseString = await response.Content.ReadAsStringAsync();

        // 🔥 Debugging: Log the full API response
        Console.WriteLine("Raw API Response: " + responseString);

        if (!response.IsSuccessStatusCode)
        {
            return $"Error: {response.StatusCode} - {responseString}";
        }

        return ParseResponse(responseString);
    }

    private string ParseResponse(string response)
    {
        try
        {
            var responseObject = JsonNode.Parse(response);
            if (responseObject?["choices"] is JsonArray choices && choices.Count > 0)
            {
                return choices[0]?["message"]?["content"]?.GetValue<string>() ?? "No content in response";
            }
            return "Invalid API response format";
        }
        catch (Exception ex)
        {
            return $"Error parsing response: {ex.Message}";
        }
    }

    private string CreatePayload(string message, string responseFormat)
    {
        var payload = new
        {
            model = Model,
            messages = new[]
            {
                new { role = "user", content = message }
            },
            response_format = new { type = responseFormat },
            temperature = Temperature,
            max_tokens = MaxTokens,
            top_p = TopP,
            frequency_penalty = FrequencyPenalty,
            presence_penalty = PresencePenalty
        };

        return JsonSerializer.Serialize(payload);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
