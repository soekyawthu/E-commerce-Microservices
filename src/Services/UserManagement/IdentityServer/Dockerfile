﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8008

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/UserManagement/IdentityServer/IdentityServer.csproj", "Services/UserManagement/IdentityServer/"]
RUN dotnet restore "Services/UserManagement/IdentityServer/IdentityServer.csproj"
COPY . .
WORKDIR "/src/Services/UserManagement/IdentityServer"
RUN dotnet build "IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "IdentityServer.dll"]
