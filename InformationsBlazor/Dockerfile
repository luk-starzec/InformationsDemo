FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["InformationsBlazor/InformationsBlazor.csproj", "InformationsBlazor/"]
COPY ["InformationsApiClient/InformationsApiClient.csproj", "InformationsApiClient/"]
RUN dotnet restore "InformationsBlazor/InformationsBlazor.csproj"
COPY . .
WORKDIR "/src/InformationsBlazor"
RUN dotnet build "InformationsBlazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InformationsBlazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InformationsBlazor.dll"]