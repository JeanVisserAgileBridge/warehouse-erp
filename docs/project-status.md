# Warehouse ERP Project Status

## Current Phase

ERP expansion — Inventory Management complete. Next up: Purchase Orders.
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

### Supplier Feature

- Create Supplier
- Update Supplier
- Activate Supplier
- Deactivate Supplier
- Get Supplier By Id
- Get Suppliers

Includes:

- Commands
- Queries
- Handlers
- DTOs
- Repository interface
- Application tests

Supplier name uniqueness is enforced case-insensitively, reusing the existing `DuplicateNameException`.

### Customer Feature

- Create Customer
- Update Customer
- Activate Customer
- Deactivate Customer
- Get Customer By Id
- Get Customers

Includes:

- Commands
- Queries
- Handlers
- DTOs
- Repository interface
- Application tests

Customer name uniqueness is enforced case-insensitively, reusing the existing `DuplicateNameException`. Implementation follows the Supplier pattern exactly.

### Warehouse Feature

- Create Warehouse
- Update Warehouse
- Activate Warehouse
- Deactivate Warehouse
- Get Warehouse By Id
- Get Warehouses

Includes:

- Commands
- Queries
- Handlers
- DTOs
- Repository interface
- Application tests

Warehouse code uniqueness is enforced case-insensitively via a new `DuplicateCodeException`.

### Storage Location Feature

- Create Storage Location
- Update Storage Location
- Activate Storage Location
- Deactivate Storage Location
- Get Storage Location By Id
- Get Storage Locations
- Get Storage Locations By Warehouse Id

Includes:

- Commands
- Queries
- Handlers
- DTOs
- Repository interface
- Application tests

Storage Location code uniqueness is enforced case-insensitively within a Warehouse, reusing `DuplicateCodeException`. A new Storage Location can only be assigned to an active Warehouse, enforced via a new `InactiveWarehouseException`. `WarehouseId` is immutable after creation.

### Dashboard Reporting

- Get Dashboard Summary

Includes:

- Query
- Query Handler
- IDashboardQueryService abstraction
- DashboardSummary DTO

### Inventory Low Stock

Includes:

- ILowStockInventoryQueryService abstraction
- LowStockInventoryItem DTO

Used by the Azure Function for scheduled background processing.

### Inventory Feature

- Create Inventory Item
- Receive Stock
- Issue Stock
- Adjust Stock
- Change Reorder Level
- Get Inventory Items
- Get Inventory Item By Id
- Get Inventory By Product Id
- Get Inventory By Storage Location Id
- Get Stock Movements By Inventory Item Id

Includes:

- Commands
- Queries
- Handlers
- DTOs (`InventoryItemDto`, `StockMovementDto`)
- `IInventoryItemRepository`, `IStockMovementRepository`
- Application tests

A new `IUnitOfWork` abstraction (`Application/Common/IUnitOfWork.cs`) was introduced because
`ReceiveStock`, `IssueStock`, and `AdjustStock` must persist an `InventoryItem` change and a
`StockMovement` together, atomically. `InventoryItemRepository` and `StockMovementRepository`
no longer call `SaveChangesAsync` themselves — every Inventory command handler calls
`IUnitOfWork.SaveChangesAsync` once after all repository calls, so both writes commit in a
single database transaction. This is scoped to the Inventory feature only; every other
repository (Product, Supplier, Customer, Warehouse, StorageLocation) keeps self-committing
`AddAsync`/`UpdateAsync` as before.

`AdjustStock`'s `StockMovement.Quantity` stores the absolute magnitude of the change
(`|new - old|`), not the resulting quantity, since the Domain already requires movement
quantity to be greater than zero and a valid adjustment can set stock to exactly zero. A
zero-delta adjustment creates no movement.

New exceptions: `DuplicateInventoryItemException`, `InactiveProductException`,
`InactiveStorageLocationException` — following the existing one-exception-per-rule pattern.

---

## Infrastructure Layer

Implemented:

### Entity Framework Core

- SQL Server provider
- WarehouseErpDbContext
- Category configuration
- Product configuration
- InventoryItem configuration
- Supplier configuration
- Customer configuration
- CategoryRepository
- ProductRepository
- SupplierRepository
- CustomerRepository
- Warehouse configuration
- StorageLocation configuration
- WarehouseRepository
- StorageLocationRepository
- StockMovementConfiguration
- InventoryItemRepository
- StockMovementRepository
- UnitOfWork (implements `IUnitOfWork`)
- Dependency Injection
- Infrastructure constants

InventoryItemConfiguration now declares a required foreign key from `InventoryItem.StorageLocationId` to `StorageLocation`, with `Restrict` delete behaviour. No Domain change was needed.

StockMovementConfiguration persists `MovementType` as a string (`.HasConversion<string>()`), `Reference` at the existing `StockMovement.MaxReferenceLength` domain constant, and a required, `Restrict`-delete foreign key to `InventoryItem`.

### Database

- SQL Server
- InitialCreate migration
- AddInventoryItems migration
- AddSuppliers migration (generated, not yet applied)
- AddCustomers migration (generated, not yet applied)
- AddWarehouses migration (generated, not yet applied) — creates `Warehouses` and `StorageLocations`, and adds the `InventoryItems.StorageLocationId` foreign key
- AddStockMovements migration (generated, not yet applied) — creates `StockMovements`

### Dapper

Used exclusively for read-only queries.

Implemented:

- DashboardQueryService
- LowStockInventoryQueryService

EF Core remains responsible for transactional persistence.

---

## API

Implemented:

- Dependency Injection
- SQL Server configuration
- EF Core migrations
- Swagger UI
- Global exception handling
- Category API
- Product API
- Supplier API
- Customer API
- Warehouse API
- Storage Location API
- Inventory API
- Dashboard API
- Shared contract mapping
- CORS configuration

Storage Location endpoints expose a nested route, `GET /api/warehouses/{warehouseId}/storage-locations`, alongside the standard `/api/storage-locations` resource routes.

Verified:

- Category CRUD
- Product CRUD
- Supplier CRUD (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
- Customer CRUD (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
- Warehouse CRUD (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
- Storage Location CRUD (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
- Inventory CRUD and stock workflows — receive, issue, adjust, change reorder level, movement history (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
- Dashboard endpoint

---

## Shared Contracts

Implemented:

### Categories

- CategoryDto
- CreateCategoryRequest
- UpdateCategoryRequest

### Products

- ProductDto
- CreateProductRequest
- UpdateProductRequest

### Suppliers

- SupplierDto
- CreateSupplierRequest
- UpdateSupplierRequest

### Customers

- CustomerDto
- CreateCustomerRequest
- UpdateCustomerRequest

### Warehouses

- WarehouseDto
- CreateWarehouseRequest
- UpdateWarehouseRequest

### Storage Locations

- StorageLocationDto
- CreateStorageLocationRequest
- UpdateStorageLocationRequest

### Dashboard

- DashboardSummary

### Inventory

- InventoryItemDto
- StockMovementDto
- StockMovementType (mirrors the Domain enum; Shared cannot reference Domain)
- CreateInventoryItemRequest
- ReceiveStockRequest
- IssueStockRequest
- AdjustStockRequest
- ChangeReorderLevelRequest

WarehouseERP.Shared is the contract boundary between the API and the Blazor frontend.

---

## Blazor WebAssembly

Implemented:

### Foundation

- Feature-first folder structure
- Application layout
- Navigation
- Typed HttpClient infrastructure
- Configuration-driven API base URL
- Reusable loading components
- Reusable error components

### Dashboard

- Dashboard summary cards
- Dapper-backed statistics

### Categories

- List
- Create
- Edit
- Activate
- Deactivate

### Products

- List
- Create
- Edit
- Activate
- Deactivate
- Category selection

### Suppliers

- List
- Create
- Edit
- Activate
- Deactivate

Added under a new "Procurement" navigation section.

### Customers

- List
- Create
- Edit
- Activate
- Deactivate

Added under a new "Sales" navigation section.

### Warehouses

- List
- Create
- Edit
- Activate
- Deactivate

### Storage Locations

- List (with Warehouse filter and display)
- Create (with Warehouse dropdown, restricted to active Warehouses)
- Edit
- Activate
- Deactivate

Added under a new "Warehouse Management" navigation section.

Template pages were removed and replaced with ERP functionality.

### Inventory

- List (Product and Storage Location display, low-stock rows highlighted)
- Create
- Details page hosting Receive Stock, Issue Stock, Adjust Stock, and Change Reorder Level forms, plus stock movement history

Added under a new "Inventory" navigation section. Product and Storage Location selectors
reuse the existing `IProductApiClient`/`IStorageLocationApiClient`.

---

## Azure Functions

Implemented:

### LowStockChecker

- Timer-triggered function
- Executes every five minutes
- Uses Dapper through Application abstractions
- Reads InventoryItems
- Logs low-stock products
- Read-only and idempotent

Low stock is defined as:

```
QuantityOnHand <= ReorderLevel
```

---

## Testing

Completed:

- Domain unit tests
- Application unit tests
- API verification through Swagger
- Blazor integration testing
- Azure Function execution verified

All tests are currently passing.

---

# Current Architecture

- Clean Architecture
- SOLID principles
- CQRS using command/query handlers
- Rich Domain Model
- EF Core for transactional persistence
- Dapper for reporting
- SQL Server database
- ASP.NET Core Web API
- Blazor WebAssembly frontend
- Azure Functions
- Shared API contracts
- Dependency Injection throughout
- Generic repositories intentionally avoided

---

# Frontend Direction

The Blazor application is intended to evolve into the complete Warehouse ERP system.

The current implementation establishes the foundation for future modules including:

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

The architecture emphasizes reusable components, feature-first organization, and scalable API clients.

---

# Immediate Next Tasks

Inventory Management is implemented end-to-end across Application, Infrastructure, API,
Shared contracts, and Blazor WebAssembly, including the new `IUnitOfWork` abstraction for
atomic InventoryItem + StockMovement writes.

Remaining before this migration is live:

- Apply the AddStockMovements migration (not yet applied), alongside the still-pending
  AddSuppliers, AddCustomers, and AddWarehouses migrations

Next:

- Continue with Purchase Orders

---

# Future Roadmap

Planned ERP modules:

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