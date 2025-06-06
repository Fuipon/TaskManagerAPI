# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем sln и csproj
COPY TaskManagerAPI.sln ./
COPY TaskManagerAPI/ ./TaskManagerAPI/

# Восстанавливаем зависимости
RUN dotnet restore "TaskManagerAPI/TaskManagerAPI.csproj"

# Публикуем
RUN dotnet publish "TaskManagerAPI/TaskManagerAPI.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "TaskManagerAPI.dll"]
