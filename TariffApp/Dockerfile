FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TariffApp/TariffApp.csproj", "TariffApp/"]
RUN dotnet restore "TariffApp/TariffApp.csproj"
COPY . .
WORKDIR "/src/TariffApp"
RUN dotnet build "TariffApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TariffApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TariffApp.dll"]
