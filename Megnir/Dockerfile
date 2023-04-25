#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Megnir/Megnir.csproj", "Megnir/"]
COPY ["Megnir.Core/Megnir.Core.csproj", "Megnir.Core/"]
RUN dotnet restore "Megnir/Megnir.csproj"
COPY . .
WORKDIR "/src/Megnir"
RUN dotnet build "Megnir.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Megnir.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Add the run script for CRON execution
RUN apk add --no-cache dcron
COPY Megnir/run.sh /app/run.sh
# Run backup at 02:00 AM every Sunday
RUN echo "0 2 * * 0 /bin/bash /app/run.sh" > /etc/cron.d/my-cronjob
CMD ["crond", "-f"]

#ENTRYPOINT ["dotnet", "Megnir.dll"]