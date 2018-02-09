#!/bin/bash

#build handlers
dotnet restore
dotnet publish -c release ./ /p:GenerateRuntimeConfigurationFiles=true

#install zip
#apt-get -qq update
#apt-get -qq -y install zip

#create deployment package
pushd bin/release/netcoreapp2.0/publish
rm -f Oversight-Collector-Api.zip
zip -r ./Oversight-Collector-Api.zip ./*
popd