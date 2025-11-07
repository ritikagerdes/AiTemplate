# AI Gateway

A versatile AI Gateway that provides unified access to multiple AI providers with RAG capabilities.

## Features

- **Multiple AI Providers Support**:
  - OpenAI
  - Azure OpenAI
  - AWS Bedrock
  - Extensible design for adding new providers

- **Advanced Capabilities**:
  - RAG (Retrieval-Augmented Generation)
  - Prompt templating
  - Vector search integration
  - Configurable context windows

- **Robust Architecture**:
  - Polly-based resilience patterns
  - Circuit breaker implementation
  - Retry policies with exponential backoff
  - Timeout handling

- **Security & Integration**:
  - Azure Key Vault integration
  - Application Insights monitoring
  - Structured logging

## Project Structure

```
AiGateway/
├── Api/                    # Backend API Service
│   ├── Models/            # API data models
│   ├── Policies/          # Polly resilience policies
│   └── Services/          # AI service implementations
├── BlazorUI/              # Frontend Blazor WebAssembly
│   ├── Components/        # Reusable UI components
│   ├── Layout/           # Layout components
│   └── Pages/            # Application pages
└── Utils/                # Shared utilities and interfaces
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Docker (optional)
- AI Provider Credentials:
  - OpenAI API Key
  - Azure OpenAI Key
  - AWS Bedrock Access

### Running Locally

1. Clone the repository:
   ```bash
   git clone [repository-url]
   cd AiGateway
   ```

2. Configure your AI provider credentials in appsettings.json or use Key Vault.

3. Run with Docker:
   ```bash
   docker-compose up --build
   ```

   Or run services individually:
   ```bash
   # Run API
   cd Api
   dotnet run

   # Run Blazor UI
   cd BlazorUI
   dotnet run
   ```

4. Access the applications:
   - API: http://localhost:5000
   - Blazor UI: http://localhost:5001

## Configuration

### API Configuration

The API can be configured through `appsettings.json` or environment variables:

```json
{
  "OpenAIKey": "your-key",
  "AzureKey": "your-key",
  "BedrockKey": "your-key",
  "KeyVaultUri": "your-keyvault-uri"
}
```

### Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `KeyVaultUri`: Azure Key Vault URI
- `APPLICATIONINSIGHTS_CONNECTION_STRING`: App Insights connection string

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

