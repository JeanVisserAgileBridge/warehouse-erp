# Warehouse ERP Project Status

## Current Phase

ERP expansion — Warehouse Management complete. Next up: Inventory Management.
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
- Dependency Injection
- Infrastructure constants

InventoryItemConfiguration now declares a required foreign key from `InventoryItem.StorageLocationId` to `StorageLocation`, with `Restrict` delete behaviour. No Domain change was needed.

### Database

- SQL Server
- InitialCreate migration
- AddInventoryItems migration
- AddSuppliers migration (generated, not yet applied)
- AddCustomers migration (generated, not yet applied)
- AddWarehouses migration (generated, not yet applied) — creates `Warehouses` and `StorageLocations`, and adds the `InventoryItems.StorageLocationId` foreign key

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

Warehouse and Storage Location Management is implemented end-to-end across Application, Infrastructure, API, Shared contracts, and Blazor WebAssembly, including the `InventoryItem.StorageLocationId` foreign key to `StorageLocation`.

Remaining before this migration is live:

- Apply the AddWarehouses migration (not yet applied)

Next:

- Continue with Inventory Management

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