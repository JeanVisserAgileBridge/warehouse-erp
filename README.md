# Warehouse ERP

A learning project that simulates a real enterprise .NET application: an ASP.NET Core Web API backed by SQL Server, a Blazor WebAssembly frontend, and an Azure Function for scheduled background processing — all built with Clean Architecture.

## Learning Goals

This project exists to practice:

- ASP.NET Core Web API
- Blazor WebAssembly
- Azure Functions
- Clean Architecture
- SOLID Principles
- EF Core
- Dapper
- Claude Code as a development workflow tool

Correctness, readability, and maintainability are prioritized over speed of delivery and premature optimization. See `CLAUDE.md` for the full set of conventions this project follows.

## Technology Stack

- **.NET 10** / C#
- **ASP.NET Core Web API** — HTTP API, Swagger/OpenAPI
- **Blazor WebAssembly** — frontend UI
- **Azure Functions (isolated worker, v4)** — scheduled background processing
- **Entity Framework Core 10** (SQL Server provider) — transactional persistence and migrations
- **Dapper** — read-only reporting queries
- **SQL Server** — database
- **Azurite** — local Azure Storage emulator (required by the Functions host)
- **xUnit** — unit testing

## Clean Architecture Overview

Dependencies point inward, toward the Domain layer:

```
Blazor ──────────────► Shared
                           ▲
API ──────────────────────┤
 │                         │
 ▼                         │
Application                │
 │                         │
 ▼                         │
Domain ◄────────────────────
 ▲
 │
Infrastructure
```

- **Domain** has no dependencies on any other project.
- **Application** depends only on Domain.
- **Infrastructure** depends on Application and Domain, and implements interfaces defined by Application.
- **API** depends on Application, Infrastructure, and Shared.
- **Blazor** depends only on Shared, and talks to the API exclusively over HTTP.

Full architecture rules and naming/folder conventions are documented in `CLAUDE.md`. The agreed domain model (entities, aggregates, business rules) is documented in `docs/domain-model.md`.

## Solution Structure

```
WarehouseERP.slnx
src/
  WarehouseERP.Domain/          Entities, value logic, domain exceptions — no external dependencies
  WarehouseERP.Application/     Commands, queries, handlers, DTOs, repository interfaces (CQRS-style)
  WarehouseERP.Infrastructure/  EF Core (WarehouseErpDbContext, migrations, repositories) + Dapper query services
  WarehouseERP.Api/             ASP.NET Core Web API, controllers, DI composition, Swagger
  WarehouseERP.Blazor/          Blazor WebAssembly frontend (feature-first folders)
  WarehouseERP.Functions/       Azure Functions (isolated worker) — LowStockChecker
  WarehouseERP.Shared/          DTOs/contracts shared between the API and Blazor
tests/
  WarehouseERP.Domain.Tests/       Domain unit tests
  WarehouseERP.Application.Tests/  Application unit tests
docs/
  domain-model.md      Agreed domain model and business rules
  project-status.md    Current implementation status and next planned work
```

`WarehouseERP.slnx` is the solution file (the newer XML-based `.slnx` format used by recent .NET SDKs).

## Implemented Features

As of this writing (see `docs/project-status.md` for the authoritative, up-to-date status):

**Domain** — Category, Product, Supplier, Customer, Warehouse, StorageLocation, InventoryItem, StockMovement, PurchaseOrder/Line, SalesOrder/Line entities, with business rules and behaviour-rich aggregates. Only Category, Product, and InventoryItem currently have persistence and API support; the remaining entities exist in the Domain layer as modeled but are not yet wired up end-to-end.

**Application / API / Blazor**
- Category management: create, update, activate, deactivate, list, get by id
- Product management: create, update, activate, deactivate, list, get by id
- Dashboard summary (category/product counts) via Dapper

**Azure Functions**
- `LowStockChecker`: a timer-triggered function that logs inventory items at or below their reorder level

**Cross-cutting**
- Global exception handling and problem-details responses in the API
- CORS configured for the Blazor dev origins
- Swagger UI for interactive API exploration
- EF Core migrations: `InitialCreate`, `AddInventoryItems`

Not yet implemented: authentication/authorization, Suppliers/Customers/Warehouses/Storage Locations/Inventory/Purchase Orders/Sales Orders UI and API, and Azure deployment. See **Future Roadmap** below.

## EF Core vs. Dapper Usage

This project deliberately uses two different data-access tools for two different jobs, and never mixes them within the same repository method:

- **EF Core** is used for transactional persistence — creates, updates, deletes, and simple lookups that go through the domain model (`CategoryRepository`, `ProductRepository`, `WarehouseErpDbContext`, and migrations).
- **Dapper** is used exclusively for read-only reporting and dashboards, where raw SQL and lightweight mapping are more appropriate than loading full aggregates:
  - `DashboardQueryService` — dashboard summary statistics
  - `LowStockInventoryQueryService` — low-stock inventory query used by the Azure Function

## Claude Code Workflow

This project uses [Claude Code](https://claude.com/claude-code) as part of its development process, configured via:

- `CLAUDE.md` — project conventions, architecture rules, and expectations Claude Code must follow before generating code
- `docs/domain-model.md` — the domain model Claude Code must consult before creating or modifying entities
- `docs/project-status.md` — current implementation status, which Claude Code reads before planning new work and updates after completing a milestone
- `.claude/` — project-level Claude Code configuration, including an **Architecture Reviewer** agent (checks changes against Clean Architecture/SOLID/dependency rules) and a **Domain Designer** agent (used before implementing new domain entities)

When asking Claude Code to make changes to this repository, point it at these files first so its suggestions stay consistent with the established architecture.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, SQL Server Express, or a full instance) — used by the API, Blazor's data, and the Azure Function
- [Azurite](https://learn.microsoft.com/azure/storage/common/storage-use-azurite) (Azure Storage emulator) — required by the Azure Functions host
- [Azure Functions Core Tools](https://learn.microsoft.com/azure/azure-functions/functions-run-local) (`func`), or Visual Studio / Rider with Azure Functions tooling
- A code editor (Visual Studio, VS Code, or JetBrains Rider)

## Local Setup

Clone the repository and restore dependencies:

```powershell
git clone <repository-url>
cd warehouseerp
dotnet restore WarehouseERP.slnx
```

## SQL Server Setup

The API and Azure Function both read their connection string from the `WarehouseErpDatabase` connection string entry.

1. Ensure a SQL Server instance is reachable (LocalDB or a full SQL Server install both work).
2. The default connection string, defined in `src/WarehouseERP.Api/appsettings.Development.json`, is:

   ```
   Server=localhost;Database=WarehouseERP;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True
   ```

   Adjust `Server=localhost` if your SQL Server instance runs elsewhere (e.g. `(localdb)\MSSQLLocalDB`).
3. The Azure Functions project supplies the same connection string independently via `src/WarehouseERP.Functions/local.settings.json` (`ConnectionStrings__WarehouseErpDatabase`) — update it to match if you changed the API's connection string.

## EF Core Migration Commands

Run these from the repository root. Migrations live in `src/WarehouseERP.Infrastructure`, and the startup project (for connection-string resolution) is the API.

```powershell
dotnet tool install --global dotnet-ef   # if not already installed

# Apply existing migrations and create the database
dotnet ef database update `
  --project src/WarehouseERP.Infrastructure `
  --startup-project src/WarehouseERP.Api

# Add a new migration after changing entity configurations
dotnet ef migrations add <MigrationName> `
  --project src/WarehouseERP.Infrastructure `
  --startup-project src/WarehouseERP.Api
```

## How to Run the API

```powershell
dotnet run --project src/WarehouseERP.Api
```

By default the API listens on `http://localhost:5091` (HTTPS profile also exposes `https://localhost:7231`). Apply EF Core migrations before first run so the required tables exist.

### Swagger URL

With the API running in the Development environment, Swagger UI is available at:

```
http://localhost:5091/swagger
```

## How to Run Blazor

```powershell
dotnet run --project src/WarehouseERP.Blazor
```

By default Blazor is served at `http://localhost:5096` (HTTPS profile also exposes `https://localhost:7210`) and calls the API at the base URL configured in `src/WarehouseERP.Blazor/wwwroot/appsettings.json` (`http://localhost:5091` by default). The API's CORS policy is pre-configured to allow requests from these Blazor origins — start the API first.

## How to Run Azurite

The Azure Functions host requires a running storage emulator. If you have Azurite installed as an npm package or VS Code extension:

```powershell
azurite --silent --location .\.azurite --debug .\.azurite\debug.log
```

Alternatively, start it from the Visual Studio Code Azurite extension, or via Docker. Leave it running in its own terminal while the Functions host is running.

## How to Run the Azure Functions Project

```powershell
cd src/WarehouseERP.Functions
func start
```

(or `dotnet run --project src/WarehouseERP.Functions` if you prefer the .NET CLI). Azurite must already be running, and the SQL Server database must be reachable, since `LowStockChecker` queries `InventoryItems` via Dapper on a 5-minute timer trigger (`0 */5 * * * *`).

**`local.settings.json` must not be committed** — it can contain local connection strings and storage settings that are specific to your machine. Confirm it is excluded from source control before pushing changes (create or update a `.gitignore` entry for it if one is not already in place).

## How to Run Tests

```powershell
dotnet test WarehouseERP.slnx
```

This runs both `WarehouseERP.Domain.Tests` and `WarehouseERP.Application.Tests`.

## Current Limitations

- Only Category, Product, and the Dashboard/Low-Stock reports have full Application/API/Blazor support. The remaining domain entities (Supplier, Customer, Warehouse, StorageLocation, InventoryItem CRUD, StockMovement, PurchaseOrder, SalesOrder) exist in the Domain layer only.
- No authentication or authorization has been implemented yet.
- The API project template's default `/weatherforecast` sample endpoint is still present in `Program.cs`.
- No CI pipeline, containerization, or Azure deployment configuration exists yet — everything runs locally.
- `local.settings.json` for the Functions project is currently present in the working tree and must be kept out of source control manually.

## Future Roadmap

Planned ERP modules and capabilities (see `docs/project-status.md` for the current priority order):

- Supplier Management
- Customer Management
- Warehouse Management
- Storage Locations
- Inventory Management
- Purchase Orders
- Sales Orders
- Reporting
- Settings
- Authentication & Authorization
- Inventory reservations
- Goods Receipts
- Shipments
- Backorders
- Batch tracking
- Serial numbers
- Azure deployment
