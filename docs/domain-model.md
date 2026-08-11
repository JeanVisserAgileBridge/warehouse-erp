# Warehouse ERP Domain Model

## Purpose

This document defines the initial domain model for the Warehouse ERP learning project.

The first version intentionally keeps the model small enough to implement and understand while still supporting realistic warehouse workflows.

The model may evolve as additional requirements are introduced.

---

## Core Modules

The application is divided into the following business modules:

* Product Catalog
* Warehouses
* Inventory
* Procurement
* Sales
* Reporting

---

## Product Catalog

### Product

Represents an item that can be purchased, stored, and sold.

Initial properties:

* Id
* SKU
* Name
* Description
* CategoryId
* UnitPrice
* IsActive
* CreatedAt
* UpdatedAt

### Category

Groups related products.

Initial properties:

- Id
- Name
- Description
- IsActive

Business rules:

- Id uses `Guid`.
- Name is required.
- Name cannot contain only whitespace.
- Name cannot exceed 100 characters.
- Description is optional.
- Description cannot exceed 500 characters.
- New Categories are active by default.
- Categories are deactivated rather than physically deleted.
- An inactive Category cannot be assigned to a new Product.
- Category name uniqueness is enforced by the Application layer because it requires access to persisted Categories.

Behaviours:

- Create
- Rename
- UpdateDescription
- Activate
- Deactivate
---

## Warehouses

### Warehouse

Represents a physical warehouse.

Initial properties:

* Id
* Code
* Name
* Address
* IsActive

A Warehouse can contain multiple Storage Locations.

### StorageLocation

Represents a specific location inside a warehouse, such as a shelf or bin.

Initial properties:

* Id
* WarehouseId
* Code
* Description
* IsActive

Each Storage Location belongs to one Warehouse.

---

## Inventory

### InventoryItem

Represents the quantity of a Product stored at a specific Storage Location.

Initial properties:

* Id
* ProductId
* StorageLocationId
* QuantityOnHand
* ReorderLevel
* UpdatedAt

Business rules:

* QuantityOnHand cannot be negative.
* Each Product and Storage Location combination must be unique.
* ReorderLevel cannot be negative.

### StockMovement

Represents an inventory quantity change.

Initial properties:

* Id
* InventoryItemId
* MovementType
* Quantity
* Reference
* OccurredAt

Movement types:

* Receipt
* Issue
* Adjustment
* Transfer
* Return

Business rules:

* Movement quantity must be greater than zero.
* Issues cannot reduce QuantityOnHand below zero.
* Stock movements should be treated as historical records and should not normally be edited.

---

## Procurement

### Supplier

Represents a company that supplies products.

Initial properties:

* Id
* Name
* Email
* PhoneNumber
* Address
* IsActive

### PurchaseOrder

Represents an order placed with a Supplier.

Initial properties:

* Id
* SupplierId
* OrderNumber
* OrderDate
* Status
* Notes

Purchase order statuses:

* Draft
* Submitted
* PartiallyReceived
* Received
* Cancelled

A Purchase Order owns one or more Purchase Order Lines.

### PurchaseOrderLine

Represents a Product ordered from a Supplier.

Initial properties:

* Id
* PurchaseOrderId
* ProductId
* QuantityOrdered
* QuantityReceived
* UnitPrice

Business rules:

* QuantityOrdered must be greater than zero.
* QuantityReceived cannot be negative.
* QuantityReceived cannot exceed QuantityOrdered.

---

## Sales

### Customer

Represents a customer who purchases products.

Initial properties:

* Id
* Name
* Email
* PhoneNumber
* Address
* IsActive

### SalesOrder

Represents an order placed by a Customer.

Initial properties:

* Id
* CustomerId
* OrderNumber
* OrderDate
* Status
* Notes

Sales order statuses:

* Draft
* Confirmed
* PartiallyFulfilled
* Fulfilled
* Cancelled

A Sales Order owns one or more Sales Order Lines.

### SalesOrderLine

Represents a Product requested by a Customer.

Initial properties:

* Id
* SalesOrderId
* ProductId
* QuantityOrdered
* QuantityFulfilled
* UnitPrice

Business rules:

* QuantityOrdered must be greater than zero.
* QuantityFulfilled cannot be negative.
* QuantityFulfilled cannot exceed QuantityOrdered.

---

## Aggregate Roots

The initial aggregate roots are:

* Product
* Category
* Warehouse
* StorageLocation
* InventoryItem
* Supplier
* PurchaseOrder
* Customer
* SalesOrder

PurchaseOrderLine belongs to PurchaseOrder.

SalesOrderLine belongs to SalesOrder.

StockMovement is created through inventory workflows associated with InventoryItem.

---

## Future Enhancements

The following concepts are intentionally postponed:

* Warehouse zones
* Product dimensions and weight
* Inventory reservations
* Goods receipts
* Shipments
* Partial delivery workflows
* Backorders
* Money value objects
* Quantity value objects
* Domain events
* Multiple currencies
* Lot and batch tracking
* Serial number tracking
* Expiry dates

These features may be added after the initial system is working.


