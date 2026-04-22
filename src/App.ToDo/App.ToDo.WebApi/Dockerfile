FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["App.ToDo.WebApi/App.ToDo.WebApi.csproj", "App.ToDo.WebApi/"]
COPY ["App.ToDo.Application/App.ToDo.Application.csproj", "App.ToDo.Application/"]
COPY ["App.ToDo.Domain/App.ToDo.Domain.csproj", "App.ToDo.Domain/"]
COPY ["App.ToDo.Infra/App.ToDo.Infra.csproj", "App.ToDo.Infra/"]

RUN dotnet restore "App.ToDo.WebApi/App.ToDo.WebApi.csproj"

COPY . .

WORKDIR "/src/App.ToDo.WebApi"
RUN dotnet build "App.ToDo.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "App.ToDo.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.ToDo.WebApi.dll"]
