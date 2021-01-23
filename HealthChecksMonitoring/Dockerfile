FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["HealthChecksMonitoring.csproj", "./"]
RUN dotnet restore "HealthChecksMonitoring.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "HealthChecksMonitoring.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HealthChecksMonitoring.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthChecksMonitoring.dll"]
