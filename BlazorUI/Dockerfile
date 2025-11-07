# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["BlazorUI/BlazorUI.csproj", "BlazorUI/"]
COPY ["Utils/Utils.csproj", "Utils/"]
RUN dotnet restore "BlazorUI/BlazorUI.csproj"

# Copy everything else
COPY . .

# Build and publish
RUN dotnet publish "BlazorUI/BlazorUI.csproj" -c Release -o /app/publish

# Final stage - Use nginx to serve static files
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY BlazorUI/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
