FROM microsoft/aspnetcore-build:1.1-projectjson 

WORKDIR /app

COPY . /sources

RUN cd /sources && dotnet restore

RUN cd /sources && git submodule init && git submodule update && cd /sources/src/SkimiaOS.ApiHost && dotnet publish --output /app/ --configuration Release

RUN rm -rf /sources

EXPOSE 80

ENTRYPOINT ["dotnet", "SkimiaOS.ApiHost.dll"]
