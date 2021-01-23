FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SeasonsApi/SeasonsApi.csproj", "SeasonsApi/"]
COPY ["ApiHelpers/ApiHelpers.csproj", "ApiHelpers/"]
RUN dotnet restore "SeasonsApi/SeasonsApi.csproj"
COPY . .
WORKDIR "/src/SeasonsApi"
RUN dotnet build "SeasonsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeasonsApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeasonsApi.dll"]