using Api.Services;
using Utils;

public class AiServiceFactory
{
	private readonly IEnumerable<IAiService> _services;

	public AiServiceFactory(IEnumerable<IAiService> services) => this._services = services;

	public IAiService Get(AiProvider provider) =>  // Utils.AiProvider
		provider switch
		{
			AiProvider.OpenAI => (IAiService?)this._services.OfType<OpenAiService>().FirstOrDefault()
				?? this._services.FirstOrDefault()!
			,
			AiProvider.Azure => (IAiService?)this._services.OfType<AzureAiService>().FirstOrDefault()
				?? this._services.FirstOrDefault()!
			,
			AiProvider.Bedrock => (IAiService?)this._services.OfType<BedrockService>().FirstOrDefault()
				?? this._services.FirstOrDefault()!
			,
			_ => this._services.FirstOrDefault()!
		};
}
