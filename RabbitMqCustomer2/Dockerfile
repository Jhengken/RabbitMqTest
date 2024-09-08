FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RabbitMqCustomer2/RabbitMqCustomer2.csproj", "RabbitMqCustomer2/"]
RUN dotnet restore "RabbitMqCustomer2/RabbitMqCustomer2.csproj"
COPY . .
WORKDIR "/src/RabbitMqCustomer2"
RUN dotnet build "RabbitMqCustomer2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMqCustomer2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMqCustomer2.dll"]
