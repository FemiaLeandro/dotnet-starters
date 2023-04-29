# IMPORTANT (if you have just recently started using Docker): See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile
# to build your images for faster debugging (THIS IS THE IMPORTANT PART).

# Using hard-specified versions is recommended to make sure no errors arise when different images are published to the repository
FROM mcr.microsoft.com/dotnet/aspnet:6.0.16 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Add the created certificates if the ContainerCerts.dev file has been created (after running Create-Scripts.ps1), see Container-Certs.props for more details on when this step runs when debugging in VS
# During regular release, this step might or might not need to run but you will have to have your Release Certificates in the Container somehow
FROM base AS testCerts
ADD Certs/test-ca.crt /usr/local/share/ca-certificates/test-ca.crt
RUN chmod 644 /usr/local/share/ca-certificates/test-ca.crt && update-ca-certificates

# Using hard-specified versions is recommended to make sure no errors arise when different images are published to the repository
# This step does not run when debugging from VS by default (read documentation linked above)
FROM mcr.microsoft.com/dotnet/sdk:6.0.408 AS build
WORKDIR /src
COPY ["Templates.DockerWebAppWithAuth/Templates.DockerWebAppWithAuth.csproj", "Templates.DockerWebAppWithAuth/"]
RUN dotnet restore "Templates.DockerWebAppWithAuth/Templates.DockerWebAppWithAuth.csproj"
COPY . .
WORKDIR "/src/Templates.DockerWebAppWithAuth"
RUN dotnet build "Templates.DockerWebAppWithAuth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Templates.DockerWebAppWithAuth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Templates.DockerWebAppWithAuth.dll"]