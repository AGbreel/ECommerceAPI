# استخدم .NET 8 runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# استخدم .NET SDK للبناء
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ECommerceAPI.csproj", "./"]
RUN dotnet restore "ECommerceAPI.csproj"
COPY . .
RUN dotnet publish "ECommerceAPI.csproj" -c Release -o /app/publish

# الصورة النهائية
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ECommerceAPI.dll"]
