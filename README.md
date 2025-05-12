# VCommerce

### Exemplo de migration usando o comando `dotnet ef`

Core:  
```bash
dotnet ef migrations add [migration_name] \
  --context AppDbContext \
  --project src/modules/core/VCommerce.Modules.Core.Infra/VCommerce.Modules.Core.Infra.csproj \
  --startup-project src/services/webapi/VCommerce.Api/VCommerce.Api.csproj

dotnet ef database update \
  --context AppDbContext \
  --project src/modules/core/VCommerce.Modules.Core.Infra/VCommerce.Modules.Core.Infra.csproj \
  --startup-project src/services/webapi/VCommerce.Api/VCommerce.Api.csproj
