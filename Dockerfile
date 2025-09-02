FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY SKNIBot/*.csproj ./SKNIBot/
RUN dotnet restore SKNIBot/SKNIBot.csproj

COPY SKNIBot/. ./SKNIBot/
RUN dotnet publish SKNIBot/SKNIBot.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SKNIBot.dll"]
