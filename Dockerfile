
FROM microsoft/dotnet:2.1-runtime

WORKDIR /opt/bin

COPY Build/netcoreapp2.1/publish /opt/bin 

EXPOSE 5061

ENTRYPOINT ["dotnet", "GB28181.Service.dll"]
