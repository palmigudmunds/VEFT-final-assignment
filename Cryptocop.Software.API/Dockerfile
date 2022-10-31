
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src

COPY . .
RUN dotnet restore "./Cryptocop.Software.API/Cryptocop.Software.API.csproj" --disable-parallel
RUN dotnet publish "./Cryptocop.Software.API/Cryptocop.Software.API.csproj" -c release -o /app --no-restore

# Serve stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "Cryptocop.Software.API.dll"]