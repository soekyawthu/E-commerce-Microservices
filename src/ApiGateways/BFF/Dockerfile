FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8011

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ApiGateways/BFF/BFF.csproj", "ApiGateways/BFF/"]
RUN dotnet restore "ApiGateways/BFF/BFF.csproj"
COPY . .
WORKDIR "/src/ApiGateways/BFF"
RUN dotnet build "BFF.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BFF.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BFF.dll"]
