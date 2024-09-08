FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RabbitMqPublish/RabbitMqPublish.csproj", "RabbitMqPublish/"]
RUN dotnet restore "RabbitMqPublish/RabbitMqPublish.csproj"
COPY . .
WORKDIR "/src/RabbitMqPublish"
RUN dotnet build "RabbitMqPublish.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMqPublish.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMqPublish.dll"]
