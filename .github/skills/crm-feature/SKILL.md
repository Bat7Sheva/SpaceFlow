---
name: crm-feature
description: "Step-by-step workflow to add a new entity/feature to the interior-crm project. Use when adding a database table, API endpoint, or Angular screen. Covers EF migration, controller, DTO, service, and Angular component creation."
argument-hint: "Feature name, e.g: Lead management with follow-up"
---
# CRM Feature Development Workflow

## When to Use
When adding any new entity or screen to the interior-crm project.

## Step-by-Step Procedure

### Step 1 - Database
1. Create EF entity class in `/Models/`
2. Add DbSet to `AppDbContext`
3. Run `dotnet ef migrations add <MigrationName>`
4. Run `dotnet ef database update`
5. Verify table exists in SQL Server

### Step 2 - API
1. Create DTOs in `/DTOs/`
2. Create service interface + implementation in `/Services/`
3. Register service in `Program.cs`
4. Create controller in `/Controllers/`
5. Run `dotnet build` - must pass with 0 errors
6. Test endpoints with a REST client

### Step 3 - Angular UI
1. Create feature folder under `src/app/features/`
2. Create Angular service in `/services/`
3. Create list component
4. Create detail/form component
5. Add route to `app.routes.ts`
6. Add navigation link to sidebar

### Step 4 - Verify
1. Run full build: `ng build`
2. Run app and test manually
3. Document: changed files + run commands + test steps
---
