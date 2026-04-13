# Interior Design CRM - AI Instructions

## Stack (mandatory, never change)
- Database: SQL Server
- Backend: ASP.NET Core Web API (.NET 8)
- ORM: Entity Framework Core
- Frontend: Angular 17+ with Angular Material
- Auth: Single user, no multi-tenancy in v1

## Work Order (always follow this sequence)
1. Database schema + EF migration
2. DTOs + API controllers
3. Angular service + components

## Rules
- After every step, run build and fix ALL errors before continuing
- Never add features outside the defined scope
- Use English for all code, comments, and file names
- Use Hebrew only for user-facing UI text
- Every response must end with: files changed, run commands, manual test steps

## Naming Conventions
- Controllers: `{Entity}Controller.cs`
- DTOs: `{Entity}Dto.cs`, `Create{Entity}Dto.cs`, `Update{Entity}Dto.cs`
- Angular components: `{entity}-list`, `{entity}-detail`, `{entity}-form`
- Angular services: `{entity}.service.ts`

## Error Handling
- API returns consistent format: `{ success, data, message }`
- Angular shows snackbar for all errors
- Never swallow exceptions silently
