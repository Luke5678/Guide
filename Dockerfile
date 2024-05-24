FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH

COPY src/ /src
WORKDIR /src/Guide
RUN dotnet restore -a $TARGETARCH
RUN dotnet publish -a $TARGETARCH --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
COPY --chmod=755 wait-for-it.sh /wait-for-it.sh

ENTRYPOINT ["/wait-for-it.sh", "db:3306", "--", "dotnet", "Guide.dll"]