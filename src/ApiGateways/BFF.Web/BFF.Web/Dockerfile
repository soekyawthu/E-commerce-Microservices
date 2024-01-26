FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8011

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiGateways/BFF.Web/BFF.Web/BFF.Web.csproj", "ApiGateways/BFF.Web/BFF.Web/"]
RUN dotnet restore "ApiGateways/BFF.Web/BFF.Web/BFF.Web.csproj"
COPY . .
WORKDIR "/src/ApiGateways/BFF.Web/BFF.Web"
RUN dotnet build "BFF.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BFF.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BFF.Web.dll"]
