# استخدم .NET 8 runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# استخدم .NET SDK للبناء
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# انسخ ملفات المشروع واسترجع الباكدچات
COPY ["ECommerceAPI.csproj", "./"]
RUN dotnet restore "ECommerceAPI.csproj"

# انسخ باقي الملفات
COPY . .

# ابني المشروع في وضع Release
RUN dotnet publish "ECommerceAPI.csproj" -c Release -o /app/publish

# الصورة النهائية للتشغيل
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ECommerceAPI.dll"]
