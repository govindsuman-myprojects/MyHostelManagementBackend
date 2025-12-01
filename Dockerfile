# Base image for running ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy .csproj and restore dependencies
COPY ["MyHostelManagement/MyHostelManagement.csproj", "MyHostelManagement/"]
RUN dotnet restore "MyHostelManagement/MyHostelManagement.csproj"

# Copy all source code
COPY . .

# Publish the project
WORKDIR "/src/MyHostelManagement"
RUN dotnet publish "MyHostelManagement.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyHostelManagement.dll"]
