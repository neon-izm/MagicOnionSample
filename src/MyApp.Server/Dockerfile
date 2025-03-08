FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS base
USER $APP_UID

EXPOSE 5244
    
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build

RUN apt-get clean \
 && apt-get update \
 && apt-get install -y --no-install-recommends \
    curl \
 && rm -rf /var/lib/apt/lists

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# ※ビルドコンテキストは src/ なので、MyApp.Server/MyApp.Server.csproj と記述する
COPY ["MyApp.Server/MyApp.Server.csproj", "MyApp.Server/"]
COPY ["MyApp.Shared/MyApp.Shared.csproj", "MyApp.Shared/"]
RUN dotnet restore "MyApp.Server/MyApp.Server.csproj"
COPY . .
WORKDIR "/src/MyApp.Server"
RUN dotnet build "MyApp.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyApp.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyApp.Server.dll"]
