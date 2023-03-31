FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["IrcClientJupe/IrcClientJupe.csproj", "IrcClientJupe/"]
COPY ["NuGet.Config", "IrcClientJupe/"]
RUN dotnet restore "IrcClientJupe/IrcClientJupe.csproj"
COPY . .
WORKDIR "/src/IrcClientJupe"
RUN dotnet build "IrcClientJupe.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IrcClientJupe.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IrcClientJupe.dll"]
