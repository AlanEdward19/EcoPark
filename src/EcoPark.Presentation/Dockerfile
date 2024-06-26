#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

# HTTP/1 - Restful API
EXPOSE 5000

ENV ASPNETCORE_URLS=https://+:5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EcoPark.Presentation/EcoPark.Presentation.csproj", "EcoPark.Presentation/"]
COPY ["EcoPark.Infrastructure/EcoPark.Infrastructure.csproj", "EcoPark.Infrastructure/"]
COPY ["EcoPark.Domain/EcoPark.Domain.csproj", "EcoPark.Domain/"]
COPY ["EcoPark.Application/EcoPark.Application.csproj", "EcoPark.Application/"]
RUN dotnet restore "./EcoPark.Presentation/./EcoPark.Presentation.csproj"
COPY . .
WORKDIR "/src/EcoPark.Presentation"
RUN dotnet build "./EcoPark.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EcoPark.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EcoPark.Presentation.dll"]