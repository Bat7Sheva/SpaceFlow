# SpaceFlow CRM Bootstrap

## Prerequisites
- .NET SDK 10+ (backend targets net10.0)
- Node.js 20+
- SQL Server LocalDB or local SQL Server instance

## Backend (ASP.NET Core Web API)
1. Go to backend project:
   - `cd backend/SpaceFlow.Api`
2. Build:
   - `dotnet build`
3. Run:
   - `dotnet run`
4. Verify endpoints:
   - `http://localhost:5284/health`
   - `http://localhost:5284/api/test`

## Frontend (Angular)
1. Go to frontend project:
   - `cd frontend/spaceflow-web`
2. Install packages (if needed):
   - `npm install`
3. Run:
   - `npm start`
4. Open:
   - `http://localhost:4200`

## Notes
- Frontend calls backend using `src/environments/environment.ts`.
- Default API base URL is `http://localhost:5284`.

## MCP SQL Server (Local)
This workspace includes a local MCP config at `.vscode/mcp.json` for SQL Server access during development.

1. Edit `.vscode/mcp.json` and set:
   - `--server` (usually `localhost`)
   - `--port` (usually `1433`)
   - `--database` (`SpaceFlowCrmDb`)
2. Make sure SQL Server allows TCP/IP and your Windows user can access the DB.
3. In VS Code, run `MCP: Reload Servers` (or reload the window).
4. Test in chat with a prompt like:
   - "List top 10 tables in my SQL Server database"

Current configured server package:
- `@shadypixel/mssql-mcp` (Windows integrated auth)
