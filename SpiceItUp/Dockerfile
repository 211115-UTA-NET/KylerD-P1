FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /SpiceItUp

COPY . .

RUN dotnet publish -c Release

FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /SpiceItUp

COPY  --from=build /SpiceItUp/bin/Release/net6.0/publish/ .

ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]