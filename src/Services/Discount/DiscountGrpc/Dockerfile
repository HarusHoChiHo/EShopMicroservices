﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Discount/DiscountGrpc/DiscountGrpc.csproj", "Services/Discount/DiscountGrpc/"]
RUN dotnet restore "Services/Discount/DiscountGrpc/DiscountGrpc.csproj"
COPY . .
WORKDIR "/src/Services/Discount/DiscountGrpc"
RUN dotnet build "./DiscountGrpc.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DiscountGrpc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscountGrpc.dll"]
