# Base image dùng để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Image dùng để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy file csproj và restore dependencies
COPY ["MathComapare/MathComapare.csproj", "MathComapare/"]
RUN dotnet restore "MathComapare/MathComapare.csproj"

# Copy toàn bộ mã nguồn và thực hiện build
COPY . .
WORKDIR "/src/MathComapare"
RUN dotnet build "MathComapare.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish ứng dụng
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MathComapare.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage để chạy ứng dụng
FROM base AS final
WORKDIR /app

# Copy mã nguồn đã publish từ bước trước
COPY --from=publish /app/publish .

# Command để chạy ứng dụng
ENTRYPOINT ["dotnet", "MathComapare.dll"]
