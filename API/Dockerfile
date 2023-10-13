#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY ["API/API.csproj", "API/API.csproj"]
COPY ["Application/Application.csproj", "Application/Application.csproj"]
COPY ["Domain/Domain.csproj", "Domain/Domain.csproj"]
COPY ["Persistence/Persistence.csproj", "Persistence/Persistence.csproj"]

RUN dotnet restore "FindAndSee.sln"

COPY . .
WORKDIR "/app"
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=publish /app/out .
ENTRYPOINT ["dotnet", "API.dll"]