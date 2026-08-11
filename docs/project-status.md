# Warehouse ERP Project Status

## Current Phase

Blazor WebAssembly implementation

---

# Completed

## Project Foundation

- Clean Architecture solution structure
- Project references
- Git repository and workflow
- Claude Code configuration
- Architecture Review skill
- Domain Designer agent
- Architecture Reviewer agent
- Project documentation
- AI development workflow

---

## API

Implemented:

- API-to-Infrastructure dependency injection
- SQL Server connection configuration
- EF Core migrations
- WarehouseERP database
- Swagger UI
- Global exception handling
- Category API endpoints
- Product API endpoints
- Category CRUD verified through Swagger and SQL Server
- Product CRUD verified through Swagger and SQL Server

## Domain Layer

Implemented and tested:

- Category
- Product
- Supplier
- Customer
- Warehouse
- StorageLocation
- InventoryItem
- StockMovement
- PurchaseOrder
- PurchaseOrderLine
- SalesOrder
- SalesOrderLine

### Domain Principles

- Rich domain model
- Static factory methods
- Private setters
- Domain exceptions
- Behaviour-rich aggregates
- Aggregate roots
- Value validation
- EF Core compatible constructors

---

## Testing

Completed:

- Domain unit tests
- Application unit tests

All tests are currently passing.

---

## Application Layer

Implemented:

### Common

- ICommandHandler
- IQueryHandler
- Application exceptions

### Category Feature

- Create Category
- Update Category
- Activate Category
- Deactivate Category
- Get Category By Id
- Get Categories

Includes:

- Commands
- Queries
- Handlers
- DTOs
- Repository interface
- Application tests

### Product Feature

- Create Product
- Update Product
- Activate Product
- Deactivate Product
- Get Product By Id
- Get Products

Includes:

- Commands
- Queries
- Handlers
- DTOs
- Repository interface
- Application tests

### Dashboard Reporting Feature

- Get Dashboard Summary

Includes:

- Query and handler (`GetDashboardSummaryQuery`)
- `IDashboardQueryService` abstraction (implemented with Dapper in Infrastructure)
- `DashboardSummary` DTO

### Inventory Low Stock Feature

- `ILowStockInventoryQueryService` abstraction (implemented with Dapper in Infrastructure)
- `LowStockInventoryItem` DTO
- Consumed directly by the `LowStockChecker` Azure Function (no Query/Handler wrapper needed, since there is no HTTP request to validate)

---

## Infrastructure Layer

Implemented:

- Entity Framework Core
- SQL Server provider
- WarehouseErpDbContext
- Category entity configuration
- Product entity configuration
- InventoryItem entity configuration (FK to Product only; StorageLocationId is not yet FK-constrained because StorageLocation is not persisted)
- CategoryRepository
- ProductRepository
- Dependency Injection extension
- Infrastructure constants
- Explicit case-insensitive SQL Server collation for:
  - Category.Name
  - Product.Sku
- Dapper (`DashboardQueryService`) for read-only dashboard reporting, using a dedicated `SqlConnection` built from the same `WarehouseErpDatabase` connection string as EF Core. EF Core remains the only data access technique for transactional persistence.
- Dapper (`LowStockInventoryQueryService`) for the read-only low-stock inventory query used by the Azure Function.

---

## Shared Contracts

Implemented:

- CategoryDto, CreateCategoryRequest, UpdateCategoryRequest
- ProductDto, CreateProductRequest, UpdateProductRequest
- DashboardSummary

The Api project now takes these Shared contracts directly as controller request/response types (mapped from Application DTOs at the controller boundary), so Blazor and the Api consume the same wire contracts instead of duplicated shapes.

---

## Blazor WebAssembly Frontend

Implemented:

- Feature-first folder structure (`Features/{Feature}/Pages|Components|Services`)
- Application layout and grouped navigation (`Shared/Layout`, with `NavSection` for scalable module grouping)
- Reusable `LoadingIndicator` and `ErrorAlert` components (`Shared/Components`)
- Typed HTTP client infrastructure (`ApiOptions` configuration, `ApiException`, `HttpResponseMessageExtensions`)
- Categories feature: list, create, edit, activate, deactivate
- Products feature: list, create, edit, activate, deactivate, category selection
- Dashboard feature: summary cards (Total/Active Categories, Total/Active/Inactive Products) at the root route, backed by `GET /api/dashboard`; `StatCard` in `Shared/Components` is reusable for future metrics

Includes:

- Configuration-driven API base URL (`wwwroot/appsettings.json`)
- CORS enabled on the Api for the Blazor dev origins

Template demo pages (Counter, Weather) were removed as out of scope for the ERP.

---

## Azure Functions

Implemented:

- `WarehouseERP.Functions` project (isolated worker model, `net10.0`, minimal hosting API)
- `LowStockChecker`: timer-triggered function (every 5 minutes) that reads low-stock `InventoryItem` rows via `ILowStockInventoryQueryService` and logs each one (InventoryItemId, ProductId, StorageLocationId, QuantityOnHand, ReorderLevel). Read-only; does not modify inventory.
- References `WarehouseERP.Application` and `WarehouseERP.Infrastructure` only, mirroring the Api project's dependency shape.

A low-stock item is defined as `QuantityOnHand <= ReorderLevel`.

No email/queue/notification integration yet — logging only, per the current demo milestone.

---

# Current Architecture Decisions

- Domain has no external dependencies.
- Application depends only on Domain.
- Infrastructure depends on Application and Domain.
- API is the composition root.
- Blazor communicates with the API over HTTP.
- EF Core is used for transactional persistence.
- Dapper is used for reporting and read-only queries (see Dashboard).
- CQRS is implemented using plain C# handlers.
- Generic repositories are intentionally avoided.
- SQL Server is the system database.

---

# Immediate Next Task

Categories, Products, the initial Dashboard reporting slice (Dapper-based), and the low-stock Azure Function are complete.

Next up:

- Final README and demo preparation
- Consider persisting Warehouse and StorageLocation (currently Domain-only), which would let InventoryItem.StorageLocationId become FK-constrained and enable seed data for a real low-stock demo run

## Frontend Direction

The Blazor WebAssembly frontend is intended to evolve into the full Warehouse ERP user interface.

The initial implementation will focus on Categories and Products for the current demo milestone, but the architecture must support future modules including:

- Dashboard
- Categories
- Products
- Suppliers
- Customers
- Warehouses
- Storage Locations
- Inventory
- Purchase Orders
- Sales Orders
- Reports
- Settings

Do not treat the initial frontend as a throwaway demo.

Prefer reusable layouts, API clients, components, feature folders, and shared UI patterns that can scale as additional ERP modules are implemented.

---

# Remaining Work

## API

- Categories API
- Products API
- Global exception handling
- Swagger verification

## Dapper

- Inventory summary (Dashboard reporting for Categories/Products is done; additional metrics such as inventory value, low stock, purchase orders, sales orders, and warehouse utilization are still to be added)

## Final Polish

- Seed data
- README updates
- Demo preparation