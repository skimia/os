FROM microsoft/aspnetcore
WORKDIR /app
COPY RELEASE .
EXPOSE 80

ENTRYPOINT ["dotnet", "SkimiaOS.ApiHost.dll"]
