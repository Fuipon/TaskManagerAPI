# Билдим на основе SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Копируем .sln
COPY TaskManagerApp.sln ./

# Копируем проект
COPY TaskManagerAPI/ ./TaskManagerAPI/

# Восстанавливаем зависимости
RUN dotnet restore "TaskManagerAPI/TaskManagerAPI.csproj"

# Сборка проекта
RUN dotnet publish "TaskManagerAPI/TaskManagerAPI.csproj" -c Release -o /publish

# Рантайм-образ
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "TaskManagerAPI.dll"]
