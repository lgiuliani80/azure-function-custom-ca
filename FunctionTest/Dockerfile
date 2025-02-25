FROM mcr.microsoft.com/dotnet/sdk:8.0 AS installer-env

COPY . /src/dotnet-function-app
RUN cd /src/dotnet-function-app && \
mkdir -p /home/site/wwwroot && \
dotnet publish *.csproj --output /home/site/wwwroot

# To enable ssh & remote debugging on app service change the base image to the one below
# FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0-appservice
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true \
    AzureFunctionsJobHost__Logging__Console__LogLevel__Default=Information

COPY ca.cer /usr/local/share/ca-certificates/terna-ca.crt
RUN echo terna-ca.crt >> /etc/ca-certificates.conf && update-ca-certificates

COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]