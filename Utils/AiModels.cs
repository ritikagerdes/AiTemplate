namespace Utils;

public enum AiProvider
{
	OpenAI,
	Azure,
	Bedrock
}

public class AiRequest
{
	public string Input { get; set; }
	public AiProvider Provider { get; set; } = AiProvider.OpenAI;

	public AiRequest() { }

	public AiRequest(string input, AiProvider provider = AiProvider.OpenAI)
	{
		this.Input = input;
		this.Provider = provider;
	}
}

public class AiResponse
{
	public string Output { get; set; } = string.Empty;

	// Add this constructor
	public AiResponse() { }

	public AiResponse(string output) => this.Output = output;
}