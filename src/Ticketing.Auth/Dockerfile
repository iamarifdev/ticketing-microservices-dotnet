FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Ticketing.Auth/Ticketing.Auth.csproj", "src/Ticketing.Auth/"]
COPY ["src/Ticketing.Common/Ticketing.Common.csproj", "src/Ticketing.Common/"]

RUN dotnet restore "src/Ticketing.Auth/Ticketing.Auth.csproj"
COPY . .
WORKDIR "/src/src/Ticketing.Auth"
RUN dotnet build "Ticketing.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ticketing.Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 443
EXPOSE 80
ENTRYPOINT ["dotnet", "Ticketing.Auth.dll"]
