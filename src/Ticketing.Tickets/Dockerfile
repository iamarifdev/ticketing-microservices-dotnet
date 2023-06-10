FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Ticketing.Tickets/Ticketing.Tickets.csproj", "src/Ticketing.Tickets/"]
RUN dotnet restore "src/Ticketing.Tickets/Ticketing.Tickets.csproj"
COPY . .
WORKDIR "/src/src/Ticketing.Tickets"
RUN dotnet build "Ticketing.Tickets.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ticketing.Tickets.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ticketing.Tickets.dll"]
