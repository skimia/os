FROM microsoft/aspnetcore:1.1
WORKDIR /app
COPY tmp_publish .
EXPOSE 800

ENTRYPOINT ["dotnet", "SkimiaOS.ApiHost.dll"]
