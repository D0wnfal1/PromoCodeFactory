# Используем образ с .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем файл решения и проекты
COPY *.sln . 
COPY PromoCodeFactory.WebHost/PromoCodeFactory.WebHost.csproj PromoCodeFactory.WebHost/
COPY PromoCodeFactory.Core/PromoCodeFactory.Core.csproj PromoCodeFactory.Core/
COPY PromoCodeFactory.DataAccess/PromoCodeFactory.DataAccess.csproj PromoCodeFactory.DataAccess/
COPY PromoCodeFactory.UnitTests/PromoCodeFactory.UnitTests.csproj PromoCodeFactory.UnitTests/

# Выполняем восстановление зависимостей
RUN dotnet restore

# Копируем остальную часть приложения
COPY . .

# Задаем рабочий каталог для публикации
WORKDIR /app/PromoCodeFactory.WebHost
RUN dotnet publish -c Release -o out

# Используем образ времени выполнения для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/PromoCodeFactory.WebHost/out ./
ENTRYPOINT ["dotnet", "PromoCodeFactory.WebHost.dll"]
