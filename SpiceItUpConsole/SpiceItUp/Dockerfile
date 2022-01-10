FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /SpiceItUp

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /SpiceItUp
COPY ["SpiceItUp.csproj", "SpiceItUp/"]
RUN dotnet restore "SpiceItUp/SpiceItUp.csproj"
COPY . .
WORKDIR "/SpiceItUp"
RUN dotnet build "SpiceItUp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpiceItUp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]