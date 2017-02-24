#!/usr/bin/env bash

restoredir=tmp_restore
publishdir=tmp_publish

echo "copy project json"

rm -rf $restoredir
mkdir -p $restoredir
cp --parents ./**/**/project.json $restoredir
cp ./global.json $restoredir

cp ./NuGet.Config $restoredir

docker build -t build-image -f Dockerfile.build .

docker create --name build-cont build-image

docker cp build-cont:/out ./$publishdir



docker build -t skimia/os -f Dockerfile.lite .

#clean
docker rm build-cont
rm -rf $restoredir

rm -rf $publishdir