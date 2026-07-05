FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Proizvodi.slnx ./
COPY src/Proizvodi.Api/Proizvodi.Api.csproj src/Proizvodi.Api/Proizvodi.Api.csproj
RUN dotnet restore src/Proizvodi.Api/Proizvodi.Api.csproj

COPY . .
WORKDIR /src/src/Proizvodi.Api
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:5188
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5188
ENTRYPOINT ["dotnet", "Proizvodi.Api.dll"]