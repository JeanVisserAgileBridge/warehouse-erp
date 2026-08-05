---

name: review-architecture
description: Review Warehouse ERP changes for Clean Architecture, SOLID, project dependency, and domain-model compliance.
disable-model-invocation: true
------------------------------

# Review Architecture

Review the requested files or current changes against the project architecture.

Before reviewing:

1. Read `CLAUDE.md`.
2. Read `docs/domain-model.md`.
3. Inspect the relevant project files.
4. Inspect project references when dependencies are involved.
5. Do not modify files.

Check for:

* Incorrect project dependencies
* Domain depending on external frameworks
* Infrastructure concerns leaking into Domain or Application
* Business logic inside controllers or Blazor components
* EF Core or Dapper outside Infrastructure
* Incorrect placement of DTOs or API contracts
* Violations of documented business rules
* SOLID principle violations
* Unnecessary abstractions
* Unrelated changes

Return the review using these headings:

## Summary

Provide a concise overall assessment.

## Findings

For every finding, include:

* Severity: Critical, High, Medium, or Low
* File and location
* Explanation
* Recommended correction

## Positive Observations

Mention decisions that correctly follow the architecture.

## Recommendation

Choose one:

* Approved
* Approved with minor changes
* Changes required

Do not implement corrections unless explicitly requested.
