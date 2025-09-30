# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .

RUN apt-get update && apt-get install -y libgdiplus
RUN ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll

RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "backend-omnis.dll"]