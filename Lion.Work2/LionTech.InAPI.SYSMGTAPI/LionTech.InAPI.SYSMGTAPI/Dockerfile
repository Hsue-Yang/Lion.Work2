FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev
RUN TZ=Asia/Taipei && \
ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && \
echo $TZ > /etc/timezone && \
dpkg-reconfigure -f noninteractive tzdata
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LionTech.InAPI.SYSMGTAPI/LionTech.InAPI.SYSMGTAPI.csproj", "LionTech.InAPI.SYSMGTAPI/"]
RUN dotnet restore "LionTech.InAPI.SYSMGTAPI/LionTech.InAPI.SYSMGTAPI.csproj" -s "https://api.nuget.org/v3/index.json;https://nexus.liontravel.com/repository/nuget-hosted"
COPY . .
WORKDIR "/src/LionTech.InAPI.SYSMGTAPI"
RUN dotnet build "LionTech.InAPI.SYSMGTAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LionTech.InAPI.SYSMGTAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LionTech.InAPI.SYSMGTAPI.dll"]
