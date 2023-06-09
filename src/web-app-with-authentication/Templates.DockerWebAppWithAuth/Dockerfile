# IMPORTANT (if you have just recently started using Docker): See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile
# to build your images for faster debugging (THIS IS THE IMPORTANT PART).

# Using hard-specified versions is recommended to make sure no errors arise when different images are published to the repository
# These are the latest at the day of the creation/update of this starter
FROM mcr.microsoft.com/dotnet/aspnet:6.0.16 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Using hard-specified versions is recommended to make sure no errors arise when different images are published to the repository
# These are the latest at the day of the creation/update of this starter
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
