using Utils;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace Api.Services;

public class OpenAiService : IAiService
{
	private readonly HttpClient _http;
	private readonly IConfiguration _config;
	private readonly ILogger<OpenAiService> _logger;

	public OpenAiService(HttpClient http, IConfiguration config, ILogger<OpenAiService> logger)
	{
		this._http = http;
		this._config = config;
		this._logger = logger;
	}

	public async Task<string> Generate(string prompt)
	{
		_ = this._config["OpenAiApiKey"];
		// Example: call OpenAI completions API
		HttpResponseMessage response = await this._http.PostAsJsonAsync("v1/completions", new
		{
			model = "text-davinci-003",
			prompt,
			max_tokens = 200
		});
		dynamic? data = await response.Content.ReadFromJsonAsync<dynamic>();
		return data.choices[0].text;
	}

	public async Task<float[]> GetEmbedding(string input)
	{
		HttpResponseMessage response = await this._http.PostAsJsonAsync("v1/embeddings", new
		{
			model = "text-embedding-3-large",
			input
		});
		dynamic? data = await response.Content.ReadFromJsonAsync<dynamic>();
		return ((IEnumerable<object>)data.data[0].embedding).Select(x => (float)x).ToArray();
	}
}
