FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TariffComparisonApp/TariffComparisonApp.csproj", "TariffComparisonApp/"]
RUN dotnet restore "TariffComparisonApp/TariffComparisonApp.csproj"
COPY . .
WORKDIR "/src/TariffComparisonApp"
RUN dotnet build "TariffComparisonApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TariffComparisonApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TariffComparisonApp.dll"]
