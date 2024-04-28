FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App
COPY [".", "."]
#RUN dotnet restore "./MobileClient/Server/MobileClient.Server.csproj"
#RUN dotnet publish "./MobileClient/Server/MobileClient.Server.csproj" -c Release -o out
RUN dotnet restore "./AdminClient/Server/AdminClient.Server.csproj"
RUN dotnet publish "./AdminClient/Server/AdminClient.Server.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0

COPY --from=build-env /App/out /AdminClient/Server
COPY ["/uploads", "/uploads"]
WORKDIR /AdminClient/Server

#ENTRYPOINT ["dotnet", "/MobileClient/Server/MobileClient.Server.dll"]
ENTRYPOINT ["dotnet", "/AdminClient/Server/AdminClient.Server.dll"]