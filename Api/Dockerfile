# Use .NET 8 SDK to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["Api/Api.csproj", "Api/"]
COPY ["Utils/Utils.csproj", "Utils/"]
RUN dotnet restore "Api/Api.csproj"

# Copy everything else
COPY . .

# Build and publish
RUN dotnet build "Api/Api.csproj" -c Release -o /app/build
RUN dotnet publish "Api/Api.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 80

ENTRYPOINT ["dotnet", "Api.dll"]
