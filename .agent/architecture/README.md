# VehicleRegistry Architecture Documentation

**Purpose**: AI-Agent-optimized technical documentation of the VehicleRegistry codebase architecture.

**Last Updated**: 2025-10-10  
**Status**: Partial Implementation (UserManagement Domain Layer Complete)

---

## 📚 Documentation Structure

This directory contains comprehensive architectural documentation organized for optimal LLM consumption:

### Core Architecture

1. **[00-overview.md](./00-overview.md)** - High-level system architecture
2. **[01-clean-architecture.md](./01-clean-architecture.md)** - Clean Architecture implementation details
3. **[02-railway-oriented.md](./02-railway-oriented.md)** - Railway-Oriented Programming patterns
4. **[03-domain-driven-design.md](./03-domain-driven-design.md)** - DDD tactical patterns

### Layer Details

1. **[10-shared-kernel.md](./10-shared-kernel.md)** - Cross-cutting concerns and primitives
2. **[11-domain-layer.md](./11-domain-layer.md)** - Domain entities, value objects, and business logic
3. **[12-application-layer.md](./12-application-layer.md)** - Use cases and validation (TODO)
4. **[13-infrastructure-layer.md](./13-infrastructure-layer.md)** - Data access and external services (TODO)
5. **[14-api-layer.md](./14-api-layer.md)** - HTTP endpoints and presentation (TODO)

### Patterns & Practices

1. **[20-validation-patterns.md](./20-validation-patterns.md)** - Atomic validation rules and composition
2. **[21-factory-patterns.md](./21-factory-patterns.md)** - Factory methods and object creation
3. **[22-nasa-rules.md](./22-nasa-rules.md)** - NASA safety rules implementation
4. **[23-testing-strategy.md](./23-testing-strategy.md)** - TDD approach and test patterns

### Reference

1. **[30-type-catalog.md](./30-type-catalog.md)** - Complete catalog of types and their relationships
2. **[31-code-examples.md](./31-code-examples.md)** - Concrete implementation examples
3. **[32-decision-log.md](./32-decision-log.md)** - Architectural decision records

### Task Documentation

Detailed implementation documentation for specific tasks:

- [2.16-2.17-userrole.md](tasks/2.16-2.17-userrole.md) - UserRole discriminated union implementation
- [2.18-2.19-verificationtoken.md](tasks/2.18-2.19-verificationtoken.md) - VerificationToken value object with TimeProvider

---

## 🎯 How to Use This Documentation

### For AI Agents

1. **Start with** `00-overview.md` for system context
2. **Reference** specific layer docs (`1X-*.md`) for detailed implementation
3. **Check** `30-type-catalog.md` for type definitions and relationships
4. **Use** `31-code-examples.md` for concrete patterns

### For Code Generation

- Each document contains:
  - Mermaid diagrams for visual understanding
  - Type signatures and interfaces
  - Implementation patterns with examples
  - Constraints and invariants
  - Related types and dependencies

### For Code Understanding

- Documents are cross-referenced with `→` links
- Code snippets show actual implementation
- Diagrams illustrate relationships and flows
- Decision rationale is preserved

---

## 📊 Implementation Status

| Layer | Status | Documentation |
|-------|--------|---------------|
| Shared Kernel | ✅ Complete | [10-shared-kernel.md](./10-shared-kernel.md) |
| Domain (UserManagement) | ✅ Complete | [11-domain-layer.md](./11-domain-layer.md) |
| Application | ⏳ Pending | [12-application-layer.md](./12-application-layer.md) |
| Infrastructure | ⏳ Pending | [13-infrastructure-layer.md](./13-infrastructure-layer.md) |
| API | ⏳ Pending | [14-api-layer.md](./14-api-layer.md) |

---

## 🔑 Key Architectural Principles

1. **Clean Architecture** - Dependency rule: inner layers never depend on outer layers
2. **Railway-Oriented Programming** - Explicit error handling with `Result<T>`
3. **Domain-Driven Design** - Rich domain model with value objects and entities
4. **Test-Driven Development** - Tests written before implementation
5. **NASA Safety Rules** - Minimum 2 assertions per function, bounded loops
6. **Atomic Validation** - Single-responsibility validation rules
7. **Factory Pattern** - Controlled object creation with validation
8. **Immutability** - Records and value objects are immutable

---

## 📖 Reading Guide

### Quick Reference

```bash
Need to understand...        → Read...
├─ Overall system            → 00-overview.md
├─ Layer dependencies        → 01-clean-architecture.md
├─ Error handling            → 02-railway-oriented.md
├─ Domain concepts           → 03-domain-driven-design.md
├─ Result<T> pattern         → 10-shared-kernel.md
├─ Value objects             → 11-domain-layer.md
├─ Validation rules          → 20-validation-patterns.md
├─ Object creation           → 21-factory-patterns.md
└─ All types                 → 30-type-catalog.md
```

### Deep Dive

1. Read 00-03 for architectural foundations
2. Read layer-specific docs (10-14) for implementation details
3. Read pattern docs (20-23) for specific techniques
4. Reference catalog (30-32) as needed

---

## 🔄 Updates

This documentation is updated as the codebase evolves:

- **2025-10-10**: Initial documentation created
  - Shared Kernel complete
  - UserManagement Domain complete
  - Email validation rules (13 rules)
  - Value objects: Email, Name, PasswordHash, UserRole, VerificationToken
  - Strongly-typed IDs: UserId, UserProfileId, AddressId
  - UserRole as discriminated union (StandardUser, Admin)
  - VerificationToken with TimeProvider dependency injection

---

## 📝 Conventions

### Notation

- `→` - See also / Related to
- `✅` - Implemented and documented
- `⏳` - Planned but not yet implemented
- `🔴` - Blocked or requires decision
- `TypeName` - Reference to a type
- `MethodName()` - Reference to a method
- `→ [doc]` - Link to documentation

### Code Snippets

```csharp
// Actual implementation from codebase
public sealed record Email(string Value);
```

```csharp
// Conceptual example (not in codebase)
public sealed record Example(string Value);
```

### Diagrams

All diagrams use Mermaid syntax and can be rendered in:

- GitHub/GitLab
- VS Code with Mermaid extension
- Any Markdown viewer with Mermaid support

---

**Note**: This documentation focuses exclusively on implemented code. Planned features are marked but not documented in detail until implementation.
