FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

ADD https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/download/v1.4.0/otel-dotnet-auto-install.sh /otel-dotnet-auto-install.sh
USER root
RUN apt-get update && \
    apt-get install -y unzip curl
RUN chmod +x /otel-dotnet-auto-install.sh
RUN set -x && OTEL_DOTNET_AUTO_HOME="/otel-dotnet-auto" /otel-dotnet-auto-install.sh

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PocConsumer/PocConsumer.csproj", "PocConsumer/"]
RUN dotnet restore "PocConsumer/PocConsumer.csproj"
COPY . .
WORKDIR "/src/PocConsumer"
RUN dotnet build "PocConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PocConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PocConsumer.dll"]
