FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_HTTPS_PORT=8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MetaExchange.API/MetaExchange.API.csproj", "MetaExchange.API/"]
COPY ["MetaExchange.Shared/MetaExchange.Shared.csproj", "MetaExchange.Shared/"]
RUN dotnet restore "MetaExchange.API/MetaExchange.API.csproj"
COPY . .
WORKDIR "/src/MetaExchange.API"
RUN dotnet build "MetaExchange.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MetaExchange.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MetaExchange.API.dll"]
