---
description: "Use when creating or modifying ASP.NET Core controllers, services, DTOs, EF migrations, or database schema. Covers naming, validation, error handling patterns for this CRM project."
applyTo: "**/*.cs"
---
# .NET API Guidelines

## Controller Pattern
- Inherit from `ControllerBase`
- Use `[ApiController]` and `[Route("api/[controller]")]`
- Return `ActionResult<ApiResponse<T>>` always
- Inject service via constructor, never use DbContext directly in controller

## Service Pattern
- Interface + implementation in `/Services/`
- DbContext injected into service only
- All async methods with `Async` suffix

## DTO Rules
- Never expose EF entities directly
- Use `CreateXxxDto` for POST, `UpdateXxxDto` for PUT
- Add `[Required]` and `[MaxLength]` on all string fields

## Migration Rules
- Never delete columns - mark as nullable instead
- Name migrations descriptively: `AddFollowUpToLeads`
- Run `dotnet ef database update` after every migration
