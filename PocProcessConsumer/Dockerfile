FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

ADD https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/download/v1.4.0/otel-dotnet-auto-install.sh /otel-dotnet-auto-install.sh
USER root
RUN apt-get update && \
    apt-get install -y unzip curl
RUN chmod +x /otel-dotnet-auto-install.sh
RUN set -x && OTEL_DOTNET_AUTO_HOME="/otel-dotnet-auto" /otel-dotnet-auto-install.sh

USER $APP_UID
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["PocProcessConsumer/PocProcessConsumer.csproj", "PocProcessConsumer/"]
RUN dotnet restore "PocProcessConsumer/PocProcessConsumer.csproj"
COPY . .
WORKDIR "/src/PocProcessConsumer"
RUN dotnet build "PocProcessConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PocProcessConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PocProcessConsumer.dll"]
