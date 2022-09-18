#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["stress-testing-app/stress-testing-app.csproj", "stress-testing-app/"]
RUN dotnet restore "stress-testing-app/stress-testing-app.csproj"
COPY . .
WORKDIR "/src/stress-testing-app"
RUN dotnet build "stress-testing-app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "stress-testing-app.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "stress-testing-app.dll"]
