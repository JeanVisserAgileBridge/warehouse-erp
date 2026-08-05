# Warehouse ERP

## Project Overview

Warehouse ERP is a learning project designed to simulate a real enterprise .NET application.

The purpose of this project is to learn:

- ASP.NET Core Web API
- Blazor WebAssembly
- Azure Functions
- Clean Architecture
- SOLID Principles
- EF Core
- Dapper
- Claude Code

This project should prioritize maintainability, readability and good architecture over speed of development.

---

# Architecture

This project follows Clean Architecture.

Dependency flow:

Presentation

↓

Application

↓

Domain

↑

Infrastructure

Rules:

- Domain must not depend on any other project.
- Application depends only on Domain.
- Infrastructure depends on Application and Domain.
- Presentation depends on Application.
- Dependency Injection should be used everywhere.

## Solution Structure

- `WarehouseERP.Domain`
  - Entities
  - Value objects
  - Enums
  - Domain rules
  - Domain events

- `WarehouseERP.Application`
  - Use cases
  - Commands and queries
  - Application interfaces
  - Validation
  - Business workflows

- `WarehouseERP.Infrastructure`
  - EF Core
  - Dapper
  - Repository implementations
  - SQL Server
  - External service implementations

- `WarehouseERP.Api`
  - API endpoints
  - Authentication
  - Dependency injection
  - Middleware
  - OpenAPI documentation

- `WarehouseERP.Blazor`
  - Blazor WebAssembly user interface
  - Pages and components
  - HTTP API clients

- `WarehouseERP.Shared`
  - API request and response contracts
  - Shared DTOs
  - Shared constants

## Project Dependency Rules

Allowed project references:

- Application references Domain.
- Infrastructure references Application and Domain.
- API references Application, Infrastructure, and Shared.
- Blazor references Shared.
- Domain references no other project.

The Blazor application communicates with the API over HTTP. It must never reference API, Application, Infrastructure, or Domain directly.

---

# Coding Standards

Always:

- Write readable code.
- Follow SOLID principles.
- Prefer composition over inheritance.
- Use constructor injection.
- Keep methods small.
- Keep classes focused on one responsibility.
- Use meaningful names.

Never:

- Put business logic inside controllers.
- Put SQL inside Blazor pages.
- Put EF Core inside Domain.
- Duplicate logic.

---

# Data Access

Use EF Core for:

- Create
- Update
- Delete
- Simple lookups

Use Dapper for:

- Dashboards
- Reports
- Complex joins
- Read-only queries

---

# APIs

Use REST conventions.

Controllers should remain thin.

Business logic belongs in the Application layer.

---

# Blazor

Pages should be lightweight.

Move business logic into services.

Reuse components whenever possible.

---

# Azure Functions

Functions should perform one task only.

Keep them idempotent.

---

# Testing

Business logic should be testable.

Prefer dependency injection.

Avoid static classes.

---

# Definition of Done

Every feature should:

- Build successfully.
- Follow Clean Architecture.
- Follow SOLID.
- Have meaningful names.
- Be understandable by another developer.