FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/services/webapp/VCommerce.Mvc/VCommerce.Mvc.csproj", "src/services/webapp/VCommerce.Mvc/"]
COPY ["src/modules/core/VCommerce.Modules.Core.Application/VCommerce.Modules.Core.Application.csproj", "src/modules/core/VCommerce.Modules.Core.Application/"]
COPY ["src/modules/core/VCommerce.Modules.Core.Domain/VCommerce.Modules.Core.Domain.csproj", "src/modules/core/VCommerce.Modules.Core.Domain/"]
COPY ["src/modules/core/VCommerce.Modules.Core.Infra/VCommerce.Modules.Core.Infra.csproj", "src/modules/core/VCommerce.Modules.Core.Infra/"]

RUN dotnet restore "src/services/webapp/VCommerce.Mvc/VCommerce.Mvc.csproj"

COPY src/modules/core/VCommerce.Modules.Core.Application /src/src/modules/core/VCommerce.Modules.Core.Application
COPY src/modules/core/VCommerce.Modules.Core.Domain /src/src/modules/core/VCommerce.Modules.Core.Domain
COPY src/modules/core/VCommerce.Modules.Core.Infra /src/src/modules/core/VCommerce.Modules.Core.Infra
COPY src/services/webapp/VCommerce.Mvc /src/src/services/webapp/VCommerce.Mvc

WORKDIR "/src/src/services/webapp/VCommerce.Mvc"
RUN dotnet build "./VCommerce.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VCommerce.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV SendGridKey="BtzK20QlQGqtwHN7Ed8hQQ"
ENTRYPOINT ["dotnet", "VCommerce.Mvc.dll"]
