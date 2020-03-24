FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY EveryBus/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./EveryBus/ ./
RUN dotnet publish -c Release -o out

ENV ASPNETCORE_URLS http://+:80

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "EveryBus.dll"]