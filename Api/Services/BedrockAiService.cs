using Amazon.BedrockRuntime.Model;
using Amazon.BedrockRuntime;
using Polly;
using Utils;

public class BedrockService : IAiService
{
	private readonly IAmazonBedrockRuntime _client;
	private readonly IAsyncPolicy _policy;
	private readonly ILogger<BedrockService> _logger;

	public BedrockService(IAmazonBedrockRuntime client, ILogger<BedrockService> logger)
	{
		this._client = client;
		this._logger = logger;

		Polly.Retry.AsyncRetryPolicy retryPolicy = Policy.Handle<Exception>()
								.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)));

		Polly.Timeout.AsyncTimeoutPolicy timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(10));

		this._policy = Policy.WrapAsync(retryPolicy, timeoutPolicy);
	}

	public async Task<string> Generate(string prompt)
	{
		InvokeModelRequest request = new()
		{
			ModelId = "your-model-id:0", // replace with your Bedrock model
            Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes($"{{\"prompt\": \"{prompt}\"}}")),
			ContentType = "application/json"
		};

		InvokeModelResponse response = await this._policy.ExecuteAsync(() => this._client.InvokeModelAsync(request));
		using StreamReader reader = new(response.Body);
		return await reader.ReadToEndAsync();
	}

	public Task<float[]> GetEmbedding(string input) =>
		// placeholder; implement if your Bedrock model supports embeddings
		Task.FromResult(new float[1536]);
}
