# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY betsecrets.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish betsecrets.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "betsecrets.dll"]
