using Amazon;
using Amazon.BedrockRuntime;
using Api.Policies;
using Api.Services;
using Azure.Identity;
using Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Key Vault
string? keyVaultUri = builder.Configuration["KeyVaultUri"];
if (!string.IsNullOrWhiteSpace(keyVaultUri))
{
	_ = builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
}

// App Insights
builder.Services.AddApplicationInsightsTelemetry();

// AI Services
if (!string.IsNullOrWhiteSpace(builder.Configuration["OpenAIKey"]))
{
	_ = builder.Services.AddHttpClient<OpenAiService>()
		.AddPolicyHandler(HttpPolicyFactory.GetRetryPolicy())
		.AddPolicyHandler(HttpPolicyFactory.GetCircuitBreakerPolicy())
		.AddPolicyHandler(HttpPolicyFactory.GetTimeoutPolicy())
		.AddTypedClient<IAiService>((http, sp) =>
			new OpenAiService(
				http,
				sp.GetRequiredService<IConfiguration>(),
				sp.GetRequiredService<ILogger<OpenAiService>>()
			)
		);
}

if (!string.IsNullOrWhiteSpace(builder.Configuration["AzureKey"]))
{
	_ = builder.Services.AddHttpClient<AzureAiService>()
		.AddPolicyHandler(HttpPolicyFactory.GetRetryPolicy())
		.AddPolicyHandler(HttpPolicyFactory.GetCircuitBreakerPolicy())
		.AddPolicyHandler(HttpPolicyFactory.GetTimeoutPolicy())
		.AddTypedClient<IAiService>((http, sp) =>
			new AzureAiService(
				http,
				sp.GetRequiredService<IConfiguration>(),
				sp.GetRequiredService<ILogger<AzureAiService>>()
			)
		);
}

if (!string.IsNullOrWhiteSpace(builder.Configuration["BedrockKey"]))
{
	_ = builder.Services.AddSingleton<IAmazonBedrockRuntime>(sp =>
		new AmazonBedrockRuntimeClient(RegionEndpoint.USEast1));

	_ = builder.Services.AddSingleton<IAiService>(sp =>
	{
		IAmazonBedrockRuntime client = sp.GetRequiredService<IAmazonBedrockRuntime>();
		ILogger<BedrockService> logger = sp.GetRequiredService<ILogger<BedrockService>>();
		return new BedrockService(client, logger);
	});
}

builder.Services.AddSingleton<AiServiceFactory>();

// Utils services
builder.Services.AddSingleton<PromptTemplateService>();
builder.Services.AddSingleton<VectorSearchService>();
builder.Services.AddCors();

WebApplication app = builder.Build();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// AI endpoint
app.MapPost("/ai", async (AiRequest req, AiServiceFactory factory) =>
{
	IAiService service = factory.Get(req.Provider); // use selected provider
	string output = await service.Generate(req.Input);
	return Results.Ok(new AiResponse(output));
});

// AI endpoint with template
app.MapPost("/ai/template/{templateKey}", async (string templateKey, AiRequest req, AiServiceFactory factory, PromptTemplateService templates) =>
{
	string prompt = templates.Build(templateKey, req.Input);
	IAiService service = factory.Get(req.Provider); // use selected provider
	string output = await service.Generate(prompt);
	return Results.Ok(new AiResponse(output));
});

// RAG endpoint
app.MapPost("/rag", async (AiRequest req, AiServiceFactory factory, VectorSearchService vector) =>
{
	List<string> contextDocs = await vector.SearchSimilarAsync(req.Input);
	string context = string.Join("\n\n", contextDocs);
	string prompt = $"Answer the question using context:\n{context}\n\nQuestion: {req.Input}";

	IAiService service = factory.Get(req.Provider); // use selected provider
	string output = await service.Generate(prompt);
	return Results.Ok(new AiResponse(output));
});

// Root and health endpoints so visiting the API base URL doesn't return 404
app.MapGet("/", () => Results.Ok(new { message = "AI Gateway API is running", routes = new[] { "/ai", "/ai/template/{templateKey}", "/rag", "/health" } }));

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));


app.Run();
