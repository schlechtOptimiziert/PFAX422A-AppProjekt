FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App
COPY [".", "."]
RUN dotnet restore "./MobileClient/Server/MobileClient.Server.csproj"
RUN dotnet publish "./MobileClient/Server/MobileClient.Server.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App

COPY --from=build-env /App/out /MobileClient/Server
COPY ["/uploads", "/uploads"]

ENTRYPOINT ["dotnet", "/MobileClient/Server/MobileClient.Server.dll"]