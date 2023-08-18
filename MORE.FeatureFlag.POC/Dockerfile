#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MORE.FeatureFlag.POC/MORE.FeatureFlag.POC.csproj", "MORE.FeatureFlag.POC/"]
RUN dotnet restore "MORE.FeatureFlag.POC/MORE.FeatureFlag.POC.csproj"
COPY . .
WORKDIR "/src/MORE.FeatureFlag.POC"
RUN dotnet build "MORE.FeatureFlag.POC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MORE.FeatureFlag.POC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MORE.FeatureFlag.POC.dll"]