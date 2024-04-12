FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Guide/Guide/Guide.csproj", "Guide/Guide/"]
COPY ["Guide/Guide.Client/Guide.Client.csproj", "Guide/Guide.Client/"]
RUN dotnet restore "Guide/Guide/Guide.csproj"
COPY . .
WORKDIR "/src/Guide/Guide"
RUN dotnet build "Guide.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Guide.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Guide.dll"]
