# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
ARG RUNTIME=linux-musl-x64
WORKDIR /src

# Copy only project files for caching
COPY ["Kafka.EventDriven/Kafka.EventDriven/Kafka.EventDriven.csproj", "Kafka.EventDriven/"]
COPY ["Kafka.EventDriven/Producer.Service/Producer.Service.csproj", "Producer.Service/"]
COPY ["Kafka.EventDriven/Consumer.Service/Consumer.Service.csproj", "Consumer.Service/"]
# Restore dependencies

RUN dotnet restore \
    "./Kafka.EventDriven/Kafka.EventDriven.csproj" \
    -r "$RUNTIME"

# Copy the entire source after restore to prevent re-restoring
COPY . .

# Publish the application
RUN dotnet publish \
    "./Kafka.EventDriven/Kafka.EventDriven.csproj" \
    -c "$BUILD_CONFIGURATION" \
    -r "$RUNTIME" \
    --self-contained false \
    -o /app/publish \
    /p:UseAppHost=false \
    /p:PublishReadyToRun=true

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
RUN apk add --no-cache icu-libs tzdata

WORKDIR /app
USER app
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Kafka.EventDriven.dll"]    