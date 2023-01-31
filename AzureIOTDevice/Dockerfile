FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AzureIOTDevice.csproj", "./"]
RUN dotnet restore "AzureIOTDevice.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AzureIOTDevice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AzureIOTDevice.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureIOTDevice.dll"]
