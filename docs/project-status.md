# Warehouse ERP Project Status

## Current Phase

Reporting/Dashboard Expansion is complete. Authentication & Authorization is the likely next phase (see Immediate Next Tasks).
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

`DashboardSummary` now reports operational metrics across every module, not just the Product
Catalog: Inventory (total items, total quantity on hand, low stock count, low stock percentage,
total inventory value), Warehouses (total/active warehouses, total/active storage locations),
Procurement (purchase order counts by status, open purchase order value), and Sales (sales order
counts by status, open sales order value). `GetDashboardSummaryQuery`/`GetDashboardSummaryQueryHandler`
remain an unchanged pass-through to `IDashboardQueryService.GetSummaryAsync` — all new metrics are
computed in SQL (`DashboardQueryService`) except `LowStockPercentage`, which is a derived,
get-only property on `DashboardSummary` itself (`LowStockItemCount / TotalInventoryItems`, guarded
against divide-by-zero) so the division stays out of both SQL and Razor.

Low Stock uses the existing rule unchanged: `QuantityOnHand <= ReorderLevel`. Inventory Value is
`SUM(InventoryItems.QuantityOnHand * Products.UnitPrice)`. Open Purchase/Sales Order Value is
`SUM((QuantityOrdered - QuantityReceived/Fulfilled) * UnitPrice)` over lines whose parent order is
not `Received`/`Fulfilled` and not `Cancelled` — which means `Draft` orders' full line value counts
as open value, since a Draft order is neither of those two terminal/negative statuses.

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
single database transaction. This carve-out from the self-committing convention now also
covers the Purchase Order feature (see below); every other repository (Product, Supplier,
Customer, Warehouse, StorageLocation) keeps self-committing `AddAsync`/`UpdateAsync` as before.

`AdjustStock`'s `StockMovement.Quantity` stores the absolute magnitude of the change
(`|new - old|`), not the resulting quantity, since the Domain already requires movement
quantity to be greater than zero and a valid adjustment can set stock to exactly zero. A
zero-delta adjustment creates no movement.

New exceptions: `DuplicateInventoryItemException`, `InactiveProductException`,
`InactiveStorageLocationException` — following the existing one-exception-per-rule pattern.

### Purchase Order Feature

- Create Purchase Order
- Add Purchase Order Line
- Update Purchase Order Line
- Remove Purchase Order Line
- Submit Purchase Order
- Cancel Purchase Order
- Receive Purchase Order Line
- Get Purchase Orders
- Get Purchase Order By Id
- Get Purchase Orders By Supplier Id

Includes:

- Commands
- Queries
- Handlers
- DTOs (`PurchaseOrderDto`, `PurchaseOrderLineDto`)
- `IPurchaseOrderRepository`
- Application tests

No Domain changes were required. `PurchaseOrder`/`PurchaseOrderLine` already supported the full
workflow (`AddLine`, `RemoveLine`, `Submit`, `Cancel`, `ReceiveProduct`). Two Application-level
decisions worth noting:

- There is no Domain `UpdateLine` behaviour. `UpdatePurchaseOrderLineCommandHandler` implements
  "update a line while Draft" by calling `RemoveLine` then `AddLine` for the same `ProductId` —
  both already require Draft and re-validate quantity/price, and since a line can only be edited
  while Draft, `QuantityReceived` is always zero at that point, so nothing is lost.
- Lines are addressed by `ProductId`, not `PurchaseOrderLine.Id`, in commands and API routes.
  This matches how the Domain aggregate itself looks lines up (`AddLine`/`RemoveLine`/
  `ReceiveProduct` all take a `ProductId`) and avoids exposing an id that would change every
  time a line is edited (since edit = remove + re-add).

`IPurchaseOrderRepository.GetByIdAsync` returns a **tracked** entity (with `Lines` included),
unlike the `AsNoTracking` + explicit `UpdateAsync` pattern used by flat aggregates (Supplier,
Product, InventoryItem). `PurchaseOrder` has a child collection that grows and shrinks via
`AddLine`/`RemoveLine`, and EF Core's change tracker can only detect added/removed/modified
lines correctly if the aggregate stays tracked between load and save. Because of this,
`IPurchaseOrderRepository` has no `UpdateAsync` method at all — command handlers mutate the
tracked aggregate returned by `GetByIdAsync` and commit through `IUnitOfWork`.

`ReceivePurchaseOrderLineCommandHandler` is the key workflow: it loads the `PurchaseOrder`,
calls `ReceiveProduct` (which enforces status/line-exists/quantity rules), validates the
`StorageLocation`, finds or creates the matching `InventoryItem` via
`GetByProductIdAndStorageLocationIdAsync`, creates a `StockMovement` of type `Receipt`, and
commits all three aggregate changes with a single `IUnitOfWork.SaveChangesAsync` call.

New exceptions: `InactiveSupplierException`, `DuplicateOrderNumberException` — following the
existing one-exception-per-rule pattern. `OrderNumber` uniqueness is enforced case-insensitively
and globally (not scoped per Supplier), matching the `Supplier.Name`/`Warehouse.Code` pattern.

### Sales Order Feature

- Create Sales Order
- Add Sales Order Line
- Update Sales Order Line
- Remove Sales Order Line
- Confirm Sales Order
- Cancel Sales Order
- Fulfil Sales Order Line
- Get Sales Orders
- Get Sales Order By Id
- Get Sales Orders By Customer Id

Includes:

- Commands
- Queries
- Handlers
- DTOs (`SalesOrderDto`, `SalesOrderLineDto`)
- `ISalesOrderRepository`
- Application tests

No Domain changes were required. `SalesOrder`/`SalesOrderLine` are structurally identical to
`PurchaseOrder`/`PurchaseOrderLine` (`AddLine`, `RemoveLine`, `Confirm`, `Cancel`,
`FulfillProduct` already supported the full workflow), so this feature is a direct structural
clone of the Purchase Order feature with fulfilment semantics instead of receiving. The same two
Application-level decisions apply:

- There is no Domain `UpdateLine` behaviour. `UpdateSalesOrderLineCommandHandler` implements
  "update a line while Draft" by calling `RemoveLine` then `AddLine` for the same `ProductId`,
  for the same reasons as Purchase Order.
- Lines are addressed by `ProductId`, not `SalesOrderLine.Id`, in commands and API routes,
  matching how `SalesOrder.FindLine` looks lines up internally.

`ISalesOrderRepository.GetByIdAsync` returns a **tracked** entity (with `Lines` included), with
no `UpdateAsync` method, for the same change-tracking reason as `IPurchaseOrderRepository`.

`FulfilSalesOrderLineCommandHandler` is the key workflow: it loads the `SalesOrder`, calls
`FulfillProduct` (which enforces status/line-exists/quantity rules), validates the
`StorageLocation`, finds the matching `InventoryItem` via
`GetByProductIdAndStorageLocationIdAsync`, calls `InventoryItem.IssueStock` (which rejects
over-issuing), creates a `StockMovement` of type `Issue`, and commits all three aggregate changes
with a single `IUnitOfWork.SaveChangesAsync` call. Unlike Purchase Order receiving, the
`InventoryItem` is **not** auto-created when missing — fulfilment throws `NotFoundException`,
since stock cannot be issued from a product/location combination that has never been stocked.

New exception: `InactiveCustomerException` — following the existing one-exception-per-rule
pattern. `DuplicateOrderNumberException` (already introduced for Purchase Orders) is reused
as-is for Sales Order number uniqueness, since it was already generic rather than
Purchase-Order-specific.

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
- PurchaseOrderConfiguration
- PurchaseOrderLineConfiguration
- PurchaseOrderRepository
- SalesOrderConfiguration
- SalesOrderLineConfiguration
- SalesOrderRepository
- UnitOfWork (implements `IUnitOfWork`)
- Dependency Injection
- Infrastructure constants

InventoryItemConfiguration now declares a required foreign key from `InventoryItem.StorageLocationId` to `StorageLocation`, with `Restrict` delete behaviour. No Domain change was needed.

StockMovementConfiguration persists `MovementType` as a string (`.HasConversion<string>()`), `Reference` at the existing `StockMovement.MaxReferenceLength` domain constant, and a required, `Restrict`-delete foreign key to `InventoryItem`.

PurchaseOrderConfiguration maps `PurchaseOrder.Lines` (an `IReadOnlyCollection<PurchaseOrderLine>`) to the private `_lines` backing field via `Navigation(...).HasField("_lines").UsePropertyAccessMode(PropertyAccessMode.Field)`, so EF Core's change tracker observes line additions/removals made through `AddLine`/`RemoveLine` without exposing a mutable collection on the aggregate's public API. `OrderNumber` uses the same case-insensitive collation and unique index pattern as `Supplier.Name`. `PurchaseOrderLineConfiguration` gives `ProductId` a `Restrict`-delete foreign key to `Product`; the `PurchaseOrderId` foreign key is `Cascade`-delete, configured from the `PurchaseOrder` side.

SalesOrderConfiguration/SalesOrderLineConfiguration mirror PurchaseOrderConfiguration/PurchaseOrderLineConfiguration exactly (private `_lines` backing field mapping, case-insensitive unique `OrderNumber` index, `Restrict`-delete FK to `Product`, `Cascade`-delete FK to the owning `SalesOrder`).

### Database

- SQL Server
- InitialCreate migration
- AddInventoryItems migration
- AddSuppliers migration (generated, not yet applied)
- AddCustomers migration (generated, not yet applied)
- AddWarehouses migration (generated, not yet applied) — creates `Warehouses` and `StorageLocations`, and adds the `InventoryItems.StorageLocationId` foreign key
- AddStockMovements migration (generated, not yet applied) — creates `StockMovements`
- AddPurchaseOrders migration (generated, not yet applied) — creates `PurchaseOrders` and `PurchaseOrderLines`
- AddSalesOrders migration (generated, not yet applied) — creates `SalesOrders` and `SalesOrderLines`

### Dapper

Used exclusively for read-only queries.

Implemented:

- DashboardQueryService — extended to a single, multi-metric summary query spanning Product
  Catalog, Inventory, Warehouses, Procurement, and Sales, still one round trip
  (`QuerySingleAsync<DashboardSummary>`)
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
- Purchase Order API
- Sales Order API
- Dashboard API
- Shared contract mapping
- CORS configuration

Storage Location endpoints expose a nested route, `GET /api/warehouses/{warehouseId}/storage-locations`, alongside the standard `/api/storage-locations` resource routes. Purchase Order endpoints follow the same pattern with `GET /api/suppliers/{supplierId}/purchase-orders`. Purchase Order line routes (`.../lines/{productId}`, including `update`, `remove`, and `receive`) are addressed by `ProductId` rather than `PurchaseOrderLine.Id`, matching how the Domain aggregate itself looks up lines.

Sales Order endpoints mirror Purchase Order endpoints exactly: `GET /api/customers/{customerId}/sales-orders` for the nested route, `ProductId`-addressed line routes (`add`, `update`, `remove`, `confirm`, `cancel`), and a fulfilment endpoint at `POST /api/sales-orders/{id}/lines/{productId}/fulfil` (spelled to match the task's route naming, distinct from the Domain's `FulfillProduct` method name).

Verified:

- Category CRUD
- Product CRUD
- Supplier CRUD (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
- Customer CRUD (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
- Warehouse CRUD (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
- Storage Location CRUD (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
- Inventory CRUD and stock workflows — receive, issue, adjust, change reorder level, movement history (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
- Purchase Order workflow — create, add/update/remove line, submit, cancel, receive (partial and full) (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
- Sales Order workflow — create, add/update/remove line, confirm, cancel, fulfil (partial and full) (build + automated tests; not yet exercised against a live database, since the migration has not been applied)
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

### Purchase Orders

- PurchaseOrderDto
- PurchaseOrderLineDto
- PurchaseOrderStatus (mirrors the Domain enum; Shared cannot reference Domain)
- CreatePurchaseOrderRequest
- AddPurchaseOrderLineRequest
- UpdatePurchaseOrderLineRequest
- ReceivePurchaseOrderLineRequest

### Sales Orders

- SalesOrderDto
- SalesOrderLineDto
- SalesOrderStatus (mirrors the Domain enum; Shared cannot reference Domain)
- CreateSalesOrderRequest
- AddSalesOrderLineRequest
- UpdateSalesOrderLineRequest
- FulfilSalesOrderLineRequest

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

- Dashboard summary cards, organized into Product Catalog, Inventory, Warehouses, Procurement,
  and Sales sections
- Dapper-backed statistics
- Monetary values (Total Inventory Value, Open Purchase/Sales Order Value) displayed with
  culture-independent `N2` numeric formatting

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

### Purchase Orders

- List (Supplier display, status badge)
- Create (Supplier selection, restricted to active Suppliers)
- Details page hosting: order header and status, Submit/Cancel actions, a lines table showing
  ordered vs. received quantities with partial/complete badges, an Add Line form (Draft only),
  inline per-line Edit/Remove (Draft only), and an inline per-line Receive form (Submitted/
  PartiallyReceived lines with remaining quantity), with destination Storage Location selection

Added under the existing "Procurement" navigation section, alongside Suppliers. Supplier,
Product, and Storage Location selectors reuse the existing `ISupplierApiClient`/
`IProductApiClient`/`IStorageLocationApiClient`. Receiving business logic (finding/creating the
`InventoryItem`, updating Purchase Order status, creating the `StockMovement`) stays entirely in
the Application layer; the Blazor form only collects Quantity, Storage Location, and an optional
Reference.

### Sales Orders

- List (Customer display, status badge)
- Create (Customer selection, restricted to active Customers)
- Details page hosting: order header and status, Confirm/Cancel actions, a lines table showing
  ordered vs. fulfilled quantities with partial/complete badges, an Add Line form (Draft only),
  inline per-line Edit/Remove (Draft only), and an inline per-line Fulfil form (Confirmed/
  PartiallyFulfilled lines with remaining quantity), with source Storage Location selection

Added under the existing "Sales" navigation section, alongside Customers. Customer, Product, and
Storage Location selectors reuse the existing `ICustomerApiClient`/`IProductApiClient`/
`IStorageLocationApiClient`. Fulfilment business logic (finding the `InventoryItem`, issuing
stock, updating Sales Order status, creating the `StockMovement`) stays entirely in the
Application layer; the Blazor form only collects Quantity, Storage Location, and an optional
Reference. This is a direct structural clone of the Purchase Order Blazor feature.

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

Sales Order Application tests cover: create (valid/missing/inactive Customer, duplicate order
number), add line (valid/missing/inactive Product, non-Draft rejection), update line, remove
line, confirm (including no-lines rejection), cancel (including fully-fulfilled rejection),
fulfilment (issues stock, partial/full status transitions, over-fulfilment rejection, Draft
rejection, missing Sales Order/Storage Location/InventoryItem, inactive Storage Location,
insufficient stock, StockMovement Issue creation, single `IUnitOfWork.SaveChangesAsync` call),
and all three queries. Dashboard Application tests cover the query handler's pass-through
behaviour (including cancellation token propagation) and `DashboardSummary.LowStockPercentage`'s
divide-by-zero guard. 707 tests pass across the Domain and Application test suites.

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

Reporting/Dashboard Expansion is complete (Application, Infrastructure, API, Shared contracts,
Blazor UI, and Application tests). The `AddSalesOrders` migration has been generated but not
applied.

Customer, Inventory, Purchase Order, Sales Order Management, and Reporting/Dashboard Expansion are
all complete. Authentication & Authorization is the likely next phase; applying the outstanding
migrations against a live database remains an open, unscheduled task.

---

# Future Roadmap

Planned ERP modules:

- Supplier Management
- Customer Management
- Warehouse Management
- Storage Locations
- Inventory Management
- Purchase Orders (complete)
- Sales Orders (complete)
- Reporting (complete)
- Settings
- Authentication & Authorization
- Inventory reservations
- Goods Receipts
- Shipments
- Backorders
- Batch tracking
- Serial numbers
- Azure deployment