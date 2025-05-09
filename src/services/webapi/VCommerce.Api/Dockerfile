FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/webapi/VCommerce.Api/VCommerce.Api.csproj", "src/services/webapi/VCommerce.Api/"]
COPY ["src/modules/core/VCommerce.Modules.Core.Application/VCommerce.Modules.Core.Application.csproj", "src/modules/core/VCommerce.Modules.Core.Application/"]
COPY ["src/modules/core/VCommerce.Modules.Core.Domain/VCommerce.Modules.Core.Domain.csproj", "src/modules/core/VCommerce.Modules.Core.Domain/"]
COPY ["src/modules/core/VCommerce.Modules.Core.Infra/VCommerce.Modules.Core.Infra.csproj", "src/modules/core/VCommerce.Modules.Core.Infra/"]
RUN dotnet restore "src/services/webapi/VCommerce.Api/VCommerce.Api.csproj"
COPY . .
WORKDIR "/src/src/services/webapi/VCommerce.Api"
RUN dotnet build "./VCommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VCommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

ENV JWT__SecretKey="x0HhzrgVfOJgG3V6Hpl9Ev9ccojN+bfZxPxX5TWi6uY="

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VCommerce.Api.dll"]
