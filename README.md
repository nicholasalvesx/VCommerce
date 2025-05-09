Exemplo de migration usando o comando dotnet ef

Criando a migração

Core: dotnet ef migrations add [migrationName] \ 
  --context AppDbContext \
  --project src/modules/core/VCommerce.Modules.Core.Infra/VCommerce.Modules.Core.Infra.csproj \
  --startup-project src/services/webapi/VCommerce.Api/VCommerce.Api.csproj \
  --output-dir Migrations

Atualizando o banco com a migração criada

Core: dotnet ef database update \
  --context AppDbContext \
  --project src/modules/core/VCommerce.Modules.Core.Infra/VCommerce.Modules.Core.Infra.csproj \
  --startup-project src/services/webapi/VCommerce.Api/VCommerce.Api.csproj
