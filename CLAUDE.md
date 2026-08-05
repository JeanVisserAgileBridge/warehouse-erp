# Warehouse ERP

## Project Overview

Warehouse ERP is a learning project designed to simulate a real enterprise .NET application.

The purpose of this project is to learn:

* ASP.NET Core Web API
* Blazor WebAssembly
* Azure Functions
* Clean Architecture
* SOLID Principles
* EF Core
* Dapper
* Claude Code

This project should prioritize maintainability, readability, and good architecture over speed of development.

---

# Project Philosophy

This project is intended to demonstrate enterprise software development practices.

Priorities, in order:

1. Correctness
2. Readability
3. Maintainability
4. Testability
5. Performance

Avoid clever solutions when a simpler solution is easier to understand.

Prefer explicit code over magic.

Favor maintainability over premature optimization.

---

# Architecture

This project follows Clean Architecture.

## Dependency Flow

```text
Blazor
    │
    ▼
Shared

API
 │
 ▼
Application
 │
 ▼
Domain
 ▲
 │
Infrastructure
```

## Architecture Rules

* Domain must not depend on any other project.
* Application depends only on Domain.
* Infrastructure depends on Application and Domain.
* API depends on Application, Infrastructure, and Shared.
* Blazor depends only on Shared.
* The Blazor application communicates with the API over HTTP.
* Dependency Injection should be used throughout the application.
* Dependencies must always point inward toward the Domain.

---

# Solution Structure

## WarehouseERP.Domain

Contains:

* Entities
* Value Objects
* Enums
* Domain Rules
* Domain Events
* Domain Exceptions

No dependencies on:

* EF Core
* Dapper
* SQL Server
* ASP.NET Core
* Blazor
* Azure Functions

---

## WarehouseERP.Application

Contains:

* Use Cases
* Commands
* Queries
* DTOs
* Interfaces
* Validation
* Business Workflows

Application contains business logic but no infrastructure implementation.

---

## WarehouseERP.Infrastructure

Contains:

* EF Core
* Dapper
* Repository Implementations
* SQL Server
* Azure Services
* External Service Implementations
* File Storage
* Email Services

Infrastructure implements interfaces defined by the Application layer.

---

## WarehouseERP.Api

Contains:

* Controllers
* Dependency Injection
* Middleware
* Authentication
* Authorization
* OpenAPI / Swagger Configuration

Controllers should remain thin.

---

## WarehouseERP.Blazor

Contains:

* Pages
* Components
* Layouts
* HTTP API Clients
* UI Services

Business logic should not live inside components.

---

## WarehouseERP.Shared

Contains:

* Shared DTOs
* API Contracts
* Shared Enums
* Shared Constants
* Shared Models

This project exists to share contracts between the API and the Blazor application.

---

# Project Dependency Rules

Allowed project references:

* Application → Domain
* Infrastructure → Application
* Infrastructure → Domain
* API → Application
* API → Infrastructure
* API → Shared
* Blazor → Shared

Never allow:

* Domain → Any other project
* Application → Infrastructure
* Blazor → Application
* Blazor → Domain
* Blazor → Infrastructure
* Infrastructure → API

---

# Domain Documentation

The agreed domain model is documented in:

`docs/domain-model.md`

Before creating or modifying domain entities:

* Review the domain model.
* Follow the documented relationships.
* Do not introduce additional entities without updating the documentation first.
* Keep business rules inside the Domain layer.

---

# Coding Standards

Always:

* Write readable code.
* Follow SOLID principles.
* Prefer composition over inheritance.
* Use constructor injection.
* Keep methods small.
* Keep classes focused on one responsibility.
* Use meaningful names.
* Write self-explanatory code.
* Keep dependencies explicit.

Never:

* Put business logic inside controllers.
* Put business logic inside Blazor components.
* Put SQL inside Blazor.
* Put EF Core inside Domain.
* Duplicate business logic.
* Create God classes.

---

# Naming Conventions

Use the following conventions:

* PascalCase for classes.
* PascalCase for methods.
* PascalCase for properties.
* camelCase for local variables.
* Private fields begin with "_".
* Interfaces begin with "I".
* Enums use singular names.
* One public class per file.
* File names match class names.
* Namespaces match folder structure.

---

# Folder Organization

Group files by feature where practical.

Avoid placing unrelated classes in the same folder.

Keep namespaces aligned with folder structure.

Prefer:

```
Products
    Commands
    Queries
    DTOs
    Validators
```

instead of:

```
Commands
Queries
DTOs
```

Feature-first organization is preferred whenever it improves maintainability.

---

# Data Access

Use EF Core for:

* Create
* Update
* Delete
* Simple lookups
* Transactions
* Aggregate persistence

Use Dapper for:

* Dashboards
* Reports
* Complex joins
* Read-only queries
* Performance-critical queries

Never mix EF Core and Dapper inside the same repository method.

---

# APIs

Follow REST conventions.

Controllers should:

* Validate requests.
* Delegate work to the Application layer.
* Return appropriate HTTP status codes.
* Contain no business logic.

---

# Blazor

Pages should:

* Be lightweight.
* Delegate work to services.
* Reuse components whenever possible.
* Avoid business logic.
* Use dependency injection.

---

# Azure Functions

Functions should:

* Perform one responsibility.
* Be idempotent.
* Contain minimal logic.
* Delegate business rules to the Application layer.

Examples:

* Low stock notifications
* Daily inventory reports
* Scheduled maintenance jobs

---

# Testing

Business logic should be testable.

Prefer dependency injection.

Avoid static classes.

Write tests for:

* Business rules
* Application services
* Validation
* Domain behavior

---

# Claude Expectations

When assisting with this project:

* Read this file before generating code.
* Review the domain model before creating entities.
* Explain architectural decisions before generating code.
* Prefer incremental changes over large rewrites.
* Do not modify unrelated files.
* Follow existing project patterns.
* Do not introduce new libraries unless requested.
* Keep generated code simple and readable.
* Explain trade-offs when multiple approaches exist.
* Ask questions when requirements are ambiguous.
* Suggest improvements, but do not implement them unless requested.

---

# Definition of Done

Every completed feature must:

* Build successfully.
* Follow Clean Architecture.
* Follow SOLID principles.
* Pass all tests.
* Contain meaningful names.
* Keep controllers thin.
* Avoid duplicated logic.
* Keep business logic out of the UI.
* Be understandable by another developer.
* Follow the architecture defined in this document.
* Be reviewed before being committed.
