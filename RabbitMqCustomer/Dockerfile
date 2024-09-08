FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RabbitMqCustomer/RabbitMqCustomer.csproj", "RabbitMqCustomer/"]
RUN dotnet restore "RabbitMqCustomer/RabbitMqCustomer.csproj"
COPY . .
WORKDIR "/src/RabbitMqCustomer"
RUN dotnet build "RabbitMqCustomer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMqCustomer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMqCustomer.dll"]
