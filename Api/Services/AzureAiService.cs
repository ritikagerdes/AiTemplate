using Utils;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace Api.Services;

public class AzureAiService : IAiService
{
	private readonly HttpClient _http;
	private readonly IConfiguration _config;
	private readonly ILogger<AzureAiService> _logger;

	public AzureAiService(HttpClient http, IConfiguration config, ILogger<AzureAiService> logger)
	{
		this._http = http;
		this._config = config;
		this._logger = logger;
	}

	public async Task<string> Generate(string prompt)
	{
		_ = this._config["AzureOpenAiEndpoint"];
		_ = this._config["AzureOpenAiKey"];
		// call Azure OpenAI completions
		return $"[AzureAI] {prompt}"; // placeholder
	}

	public async Task<float[]> GetEmbedding(string input) =>
		// call Azure OpenAI embeddings
		new float[1536]; // placeholder
}
