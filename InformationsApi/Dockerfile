FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["InformationsApi/InformationsApi.csproj", "InformationsApi/"]
COPY ["SeasonsApiClient/SeasonsApiClient.csproj", "SeasonsApiClient/"]
COPY ["ApiHelpers/ApiHelpers.csproj", "ApiHelpers/"]
RUN dotnet restore "InformationsApi/InformationsApi.csproj"
COPY . .
WORKDIR "/src/InformationsApi"
RUN dotnet build "InformationsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InformationsApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InformationsApi.dll"]