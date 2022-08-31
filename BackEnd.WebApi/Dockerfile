#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BackEnd.WebApi/BackEnd.WebApi.csproj", "BackEnd.WebApi/"]
COPY ["BackEnd.Interface/BackEnd.Interface.csproj", "BackEnd.Interface/"]
COPY ["BackEnd.Model/BackEnd.Model.csproj", "BackEnd.Model/"]
COPY ["BackEnd.Entity/BackEnd.Entity.csproj", "BackEnd.Entity/"]
COPY ["BackEnd.Data/BackEnd.Data.csproj", "BackEnd.Data/"]
COPY ["BackEnd.Common/BackEnd.Common.csproj", "BackEnd.Common/"]
COPY ["BackEnd.Business/BackEnd.Business.csproj", "BackEnd.Business/"]
COPY ["BackEnd.Toolkit/BackEnd.Toolkit.csproj", "BackEnd.Toolkit/"]
RUN dotnet restore "BackEnd.WebApi/BackEnd.WebApi.csproj"
COPY . .
WORKDIR "/src/BackEnd.WebApi"
RUN dotnet build "BackEnd.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackEnd.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackEnd.WebApi.dll"]