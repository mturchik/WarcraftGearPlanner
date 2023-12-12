FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WarcraftGearPlanner/WarcraftGearPlanner.Server/WarcraftGearPlanner.Server.csproj", "WarcraftGearPlanner/"]
RUN dotnet restore "./WarcraftGearPlanner.Server/./WarcraftGearPlanner.Server.csproj"
COPY . .
WORKDIR "/src/WarcraftGearPlanner.Server"
RUN dotnet build "./WarcraftGearPlanner.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WarcraftGearPlanner.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WarcraftGearPlanner.Server.dll"]
