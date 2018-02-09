#!/bin/bash

#build handlers
dotnet restore
dotnet publish -c release ./ /p:GenerateRuntimeConfigurationFiles=true

#install zip
#apt-get -qq update
#apt-get -qq -y install zip

#create deployment package
rm -f Collector-Api.zip
pushd bin/release/netcoreapp2.0/publish
zip -r ./Collector-Api.zip ./*
popd