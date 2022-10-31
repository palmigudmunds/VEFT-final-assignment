# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src

COPY . .
RUN dotnet restore "cryptocop-payments.csproj" --disable-parallel
RUN dotnet publish "cryptocop-payments.csproj" -c release -o /app --no-restore

# Serve stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "cryptocop-payments.dll"]



# FROM microsoft/dotnet:2.2-runtime-stretch-slim AS base
# WORKDIR /app

# FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
# WORKDIR /src
# COPY ["cryptocop-payments.csproj", "cryptocop-payments/"]
# RUN dotnet restore "cryptocop-payments/cryptocop-payments.csproj"
# COPY . .
# WORKDIR "/src/cryptocop-payments"
# RUN dotnet build "cryptocop-payments.csproj" -c Release -o /app

# FROM build AS publish
# RUN dotnet publish "cryptocop-payments.csproj" -c Release -o /app

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app .
# ENTRYPOINT ["dotnet", "cryptocop-payments.dll"]