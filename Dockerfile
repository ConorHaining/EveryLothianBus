FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

EXPOSE 80
EXPOSE 443

# Copy csproj and restore as distinct layers
COPY EveryBus/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./EveryBus/ ./
RUN dotnet publish -c Release -o out

ENV ASPNETCORE_URLS https://+443;http://+80
ENV ASPNETCORE_HTTPS_PORT 443

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "EveryBus.dll"]