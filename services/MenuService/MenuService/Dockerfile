# Use the official .NET 9 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["MenuService/MenuService.csproj", "MenuService/"]
COPY ["MenuService.Domain/MenuService.Domain.csproj", "MenuService.Domain/"]
COPY ["MenuService.Application/MenuService.Application.csproj", "MenuService.Application/"]
COPY ["MenuService.Infrastructure/MenuService.Infrastructure.csproj", "MenuService.Infrastructure/"]

RUN dotnet restore "MenuService/MenuService.csproj" --disable-parallel

# Copy everything else and build
COPY . .
WORKDIR "/src/MenuService"
RUN dotnet build "MenuService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MenuService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 9 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "MenuService.dll"]