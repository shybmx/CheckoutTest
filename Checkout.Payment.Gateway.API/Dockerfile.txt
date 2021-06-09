FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

ARG VERSION
ARG PAT
ARG USERNAME
ARG NUGET_PACKAGE_FEED_URL="https://api.nuget.org/v3/index.json"

RUN apk update                                                                                                                                                                                                                         
RUN apk add wget                                                                                                                                                                                                       
RUN apk add curl
RUN wget -qO- https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh | /bin/sh
ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED true

WORKDIR /app

COPY *.sln  .
COPY *.csproj .
RUN for file in $(ls **/*.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore --source ${NUGET_PACKAGE_FEED_URL}
COPY . .

FROM build as publish
ARG VERSION
WORKDIR /app/Checkout.Payment.Gateway.API
RUN dotnet build -c Release --no-restore -p:Version=${VERSION}
RUN dotnet publish -c Release -o out --no-restore -p:Version=${VERSION}

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS runtime
COPY --from=publish /app/Checkout.Payment.Gateway.API/out ./ 
ENTRYPOINT ["dotnet", "Checkout.Payment.Gateway.API"]