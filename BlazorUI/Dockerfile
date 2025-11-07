# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY *.sln ./
COPY BlazorUI/*.csproj ./BlazorUI/
COPY Utils/*.csproj ./Utils/
RUN dotnet restore
COPY . ./
RUN dotnet publish ./BlazorUI/BlazorUI.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5001
ENTRYPOINT ["dotnet", "BlazorUI.dll"]
