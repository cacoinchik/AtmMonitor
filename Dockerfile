FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/AtmMonitor.API/AtmMonitor.API.csproj", "src/AtmMonitor.API/"]
COPY ["src/AtmMonitor.Core/AtmMonitor.Core.csproj", "src/AtmMonitor.Core/"]
COPY ["src/AtmMonitor.Infrastructure/AtmMonitor.Infrastructure.csproj", "src/AtmMonitor.Infrastructure/"]

RUN dotnet restore "src/AtmMonitor.API/AtmMonitor.API.csproj"

COPY . .

WORKDIR "/src/src/AtmMonitor.API"
RUN dotnet build "AtmMonitor.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AtmMonitor.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AtmMonitor.API.dll"]