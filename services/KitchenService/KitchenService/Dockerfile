# Use the official .NET 9 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["KitchenService/KitchenService.csproj", "KitchenService/"]
COPY ["KitchenService.Domain/KitchenService.Domain.csproj", "KitchenService.Domain/"]
COPY ["KitchenService.Application/KitchenService.Application.csproj", "KitchenService.Application/"]
COPY ["KitchenService.Infrastructure/KitchenService.Infrastructure.csproj", "KitchenService.Infrastructure/"]

RUN dotnet restore "KitchenService/KitchenService.csproj" --disable-parallel

# Copy everything else and build
COPY . .
WORKDIR "/src/KitchenService"
RUN dotnet build "KitchenService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "KitchenService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 9 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "KitchenService.dll"]