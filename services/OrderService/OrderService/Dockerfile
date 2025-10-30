# Use the official .NET 9 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["OrderService/OrderService/OrderService.csproj", "OrderService/OrderService/"]
COPY ["OrderService/OrderService.Domain/OrderService.Domain.csproj", "OrderService/OrderService.Domain/"]
COPY ["OrderService/OrderService.Application/OrderService.Application.csproj", "OrderService/OrderService.Application/"]
COPY ["OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj", "OrderService/OrderService.Infrastructure/"]
COPY ["KitchenService/KitchenService.Domain/KitchenService.Domain.csproj", "KitchenService/KitchenService.Domain/"]

RUN dotnet restore "OrderService/OrderService/OrderService.csproj" --disable-parallel

# Copy everything else and build
COPY . .
WORKDIR "/src/OrderService"
RUN dotnet build "OrderService/OrderService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "OrderService/OrderService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 9 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "OrderService.dll"]