# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy toàn bộ nội dung vào container
COPY . ./

# Restore dependencies
RUN dotnet restore "ttxaphuong/ttxaphuong.csproj"

# Build
RUN dotnet build "ttxaphuong/ttxaphuong.csproj" -c Release -o /app/build

# Publish
RUN dotnet publish "ttxaphuong/ttxaphuong.csproj" -c Release -o /app/publish /p:UseAppHost=false

# STAGE 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "ttxaphuong.dll"]
