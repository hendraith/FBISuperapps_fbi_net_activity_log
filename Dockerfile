FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/' /etc/ssl/openssl.cnf
RUN sed -i 's/CipherString = DEFAULT@SECLEVEL=2/CipherString = DEFAULT@SECLEVEL=1/' /etc/ssl/openssl.cnf


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .

RUN echo $(ls -1 /src)

RUN dotnet restore -s "http://10.110.100.61:9090/v3/index.json"  -s "https://api.nuget.org/v3/index.json" "ActivityLog.csproj"

FROM build AS publish
RUN dotnet publish "ActivityLog.csproj" -c Release -o /app/publish

RUN echo $(ls -1 /app)

FROM base AS final
WORKDIR /src/app
COPY --from=publish /app/publish .

RUN apt-get update

RUN apt-get install curl -y

ENTRYPOINT ["dotnet", "ActivityLog.dll"]	

