#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TechTest.Ryanair.Tycoon.Api/TechTest.Ryanair.Tycoon.Api.csproj", "src/TechTest.Ryanair.Tycoon.Api/"]
COPY ["src/TechTest.Ryanair.Tycoon.Infra/TechTest.Ryanair.Tycoon.Infra.csproj", "src/TechTest.Ryanair.Tycoon.Infra/"]
COPY ["src/TechTest.Ryanair.Tycoon.Domain/TechTest.Ryanair.Tycoon.Domain.csproj", "src/TechTest.Ryanair.Tycoon.Domain/"]
COPY ["src/TechTest.Ryanair.Tycoon.Application/TechTest.Ryanair.Tycoon.Application.csproj", "src/TechTest.Ryanair.Tycoon.Application/"]
RUN dotnet restore "src/TechTest.Ryanair.Tycoon.Api/TechTest.Ryanair.Tycoon.Api.csproj"
COPY . .
WORKDIR "/src/src/TechTest.Ryanair.Tycoon.Api"
RUN dotnet build "TechTest.Ryanair.Tycoon.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TechTest.Ryanair.Tycoon.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechTest.Ryanair.Tycoon.Api.dll"]