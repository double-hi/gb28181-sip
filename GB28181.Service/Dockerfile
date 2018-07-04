
FROM microsoft/dotnet

WORKDIR /app

COPY publish /app

EXPOSE 5061

ENTRYPOINT ["dotnet", "GB28181.Service.dll"]
