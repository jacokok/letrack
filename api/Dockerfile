FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN apt update \
    && apt install -y curl \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
EXPOSE 5021

ENV ASPNETCORE_URLS=http://+:5021

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["letrack-api.csproj", "./"]
RUN dotnet restore "letrack-api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "letrack-api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "letrack-api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "letrack-api.dll"]
