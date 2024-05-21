FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["src/", "."]

WORKDIR "/src/Guide"
RUN dotnet restore "Guide.csproj"
RUN dotnet build "Guide.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Guide.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY --chmod=755 wait-for-it.sh /wait-for-it.sh

ENTRYPOINT ["/wait-for-it.sh", "db:3306", "--", "dotnet", "Guide.dll"]
