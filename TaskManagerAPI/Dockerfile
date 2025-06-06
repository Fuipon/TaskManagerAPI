# Используем официальный образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы и собираем приложение
COPY . ./
RUN dotnet publish -c Release -o out

# Используем официальный образ ASP.NET Core для запуска
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .

# Открываем порт
EXPOSE 80

# Запускаем приложение
ENTRYPOINT ["dotnet", "TaskManagerAPI.dll"]
