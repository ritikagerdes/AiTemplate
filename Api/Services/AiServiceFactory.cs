using Api.Services;
using Utils;

public class AiServiceFactory
{
	private readonly IEnumerable<IAiService> _services;

	public AiServiceFactory(IEnumerable<IAiService> services) => this._services = services;

	public IAiService Get(AiProvider provider) =>  // Utils.AiProvider
		provider switch
		{
			AiProvider.OpenAI => this._services.OfType<OpenAiService>().First(),
			AiProvider.Azure => this._services.OfType<AzureAiService>().First(),
			AiProvider.Bedrock => this._services.OfType<BedrockService>().First(),
			_ => throw new NotImplementedException()
		};
}
