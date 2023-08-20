FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApps/TraditionalWebApp/TraditionalWebApp.csproj", "WebApps/TraditionalWebApp/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
RUN dotnet restore "WebApps/TraditionalWebApp/TraditionalWebApp.csproj"
COPY . .
WORKDIR "/src/WebApps/TraditionalWebApp"
RUN dotnet build "TraditionalWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TraditionalWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TraditionalWebApp.dll"]
