FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Countries.Web/Countries.Web.csproj", "Countries.Web/"]
RUN dotnet restore "Countries.Web/Countries.Web.csproj"
COPY . .
WORKDIR "/src/Countries.Web"
RUN dotnet build "Countries.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Countries.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Countries.Web.dll"]
