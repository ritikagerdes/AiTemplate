using Utils;
using Microsoft.Extensions.Logging;

namespace Api.Services;

// Simple local stub AI service for development/testing when external providers aren't configured
public class LocalAiService : IAiService
{
    private readonly ILogger<LocalAiService> _logger;

    public LocalAiService(ILogger<LocalAiService> logger)
    {
        _logger = logger;
    }

    public Task<float[]> GetEmbedding(string input)
    {
        // Return a dummy embedding (empty) for development
        _logger.LogDebug("LocalAiService.GetEmbedding called");
        return Task.FromResult(Array.Empty<float>());
    }

    public Task<string> Generate(string prompt)
    {
        _logger.LogInformation("LocalAiService generating stub response");
        return Task.FromResult($"[Local stub] {prompt}");
    }
}
