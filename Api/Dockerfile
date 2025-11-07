# Use .NET 8 SDK to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY *.sln ./
COPY Api/*.csproj ./Api/
COPY Utils/*.csproj ./Utils/
RUN dotnet restore

# Copy everything else
COPY . ./

# Publish
RUN dotnet publish ./Api/Api.csproj -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 5000

# Start
ENTRYPOINT ["dotnet", "Api.dll"]
