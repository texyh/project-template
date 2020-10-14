#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
COPY /src . 
WORKDIR "/ProjectTemplate.Api"
RUN dotnet restore "ProjectTemplate.Api.csproj"
RUN dotnet build "ProjectTemplate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectTemplate.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "ProjectTemplate.Api.dll"]