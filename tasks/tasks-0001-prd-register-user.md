# Tasks: PRD-0001 - Register User Feature

**PRD:** [0001-prd-register-user.md](./0001-prd-register-user.md)  
**Status:** In Progress  
**Created:** 8. Oktober 2025  
**Feature Branch:** `feature/0001-register-user`

---

## Pre-Implementation Setup

- [x] 0.1 Create feature branch `feature/0001-register-user` from `main`
- [x] 0.2 Verify all dependencies are installed and projects build successfully

---

## Relevant Files

### Shared Kernel

- `src/Shared.Kernel/Shared.Kernel.csproj` - Shared kernel project for cross-cutting concerns
- `src/Shared.Kernel/Result.cs` - Result type for Railway-Oriented Programming
- `src/Shared.Kernel/ResultT.cs` - Generic Result\<T\> type
- `src/Shared.Kernel/Error.cs` - Error value object
- `src/Shared.Kernel/IValidationRule.cs` - Base interface for validation rules
- `src/Shared.Kernel/RuleComposer.cs` - Composite pattern for combining validation rules
- `src/Shared.Kernel/IRequestHandler.cs` - Request/Response pattern interface
- `tests/Tests.Shared.Kernel/Tests.Shared.Kernel.csproj` - Unit tests for shared kernel
- `tests/Tests.Shared.Kernel/ResultTests.cs` - Tests for Result type
- `tests/Tests.Shared.Kernel/ResultTTests.cs` - Tests for Result\<T\> type
- `tests/Tests.Shared.Kernel/RuleComposerTests.cs` - Tests for RuleComposer

### Domain Layer

- `src/UserManagement.Domain/Common/StronglyTypedId.cs` - Base class for strongly-typed IDs
- `src/UserManagement.Domain/Entities/User.cs` - User entity with factory method
- `src/UserManagement.Domain/Entities/UserProfile.cs` - UserProfile entity
- `src/UserManagement.Domain/Entities/Address.cs` - Address entity with Null Object Pattern
- `src/UserManagement.Domain/ValueObjects/Email.cs` - Email value object
- `src/UserManagement.Domain/ValueObjects/EmailFactory.cs` - Factory for Email (in same file)
- `src/UserManagement.Domain/ValueObjects/PasswordHash.cs` - PasswordHash value object
- `src/UserManagement.Domain/ValueObjects/Name.cs` - Name value object (FirstName, LastName)
- `src/UserManagement.Domain/ValueObjects/UserRole.cs` - UserRole value object
- `src/UserManagement.Domain/ValueObjects/VerificationToken.cs` - VerificationToken value object
- `src/UserManagement.Domain/ValueObjects/AddressComponents/Street.cs` - Street value object
- `src/UserManagement.Domain/ValueObjects/AddressComponents/City.cs` - City value object
- `src/UserManagement.Domain/ValueObjects/AddressComponents/PostalCode.cs` - PostalCode value object
- `src/UserManagement.Domain/ValueObjects/AddressComponents/Country.cs` - Country value object
- `src/UserManagement.Domain/ValueObjects/UserId.cs` - Strongly-typed UserId
- `src/UserManagement.Domain/ValueObjects/UserProfileId.cs` - Strongly-typed UserProfileId
- `src/UserManagement.Domain/ValueObjects/AddressId.cs` - Strongly-typed AddressId
- `src/UserManagement.Domain/Enums/EmailVerificationStatus.cs` - Discriminated Union for email verification
- `src/UserManagement.Domain/Enums/EmailVerificationStatusFactory.cs` - Factory for verification status (in same file)
- `tests/Tests.UserManagement.Domain/Tests.UserManagement.Domain.csproj` - Domain unit tests
- `tests/Tests.UserManagement.Domain/Entities/UserTests.cs` - User entity tests
- `tests/Tests.UserManagement.Domain/Entities/UserProfileTests.cs` - UserProfile tests
- `tests/Tests.UserManagement.Domain/Entities/AddressTests.cs` - Address tests (including Null Object)
- `tests/Tests.UserManagement.Domain/ValueObjects/EmailTests.cs` - Email value object tests
- `tests/Tests.UserManagement.Domain/ValueObjects/PasswordHashTests.cs` - PasswordHash tests
- `tests/Tests.UserManagement.Domain/ValueObjects/NameTests.cs` - Name tests

### Application Layer - Validation Rules (47+ atomic rules)

- `src/UserManagement.Application/Validation/Email/EmailNotEmptyRule.cs` - Email not empty validation
- `src/UserManagement.Application/Validation/Email/OnlyOneAtSymbolRule.cs` - Single @ symbol validation
- `src/UserManagement.Application/Validation/Email/LocalPartMaxLengthRule.cs` - Local part <= 64 chars
- `src/UserManagement.Application/Validation/Email/DomainPartMaxLengthRule.cs` - Domain part <= 255 chars
- `src/UserManagement.Application/Validation/Email/NoTrailingDotRule.cs` - No trailing dot validation
- `src/UserManagement.Application/Validation/Email/NoNewlineCharactersRule.cs` - No newline chars
- `src/UserManagement.Application/Validation/Email/NoUnderscoreInDomainRule.cs` - No underscore in domain
- `src/UserManagement.Application/Validation/Email/ValidIpAddressRule.cs` - Valid IP address format
- `src/UserManagement.Application/Validation/Email/...` - (40+ more atomic email rules based on test cases)
- `tests/Tests.UserManagement.Application/Validation/Email/EmailNotEmptyRuleTests.cs` - Test for each rule
- `tests/Tests.UserManagement.Application/Validation/Email/OnlyOneAtSymbolRuleTests.cs` - Test for each rule
- `tests/Tests.UserManagement.Application/Validation/Email/...` - (Tests for all 47+ rules)

### Application Layer - Core

- `src/UserManagement.Application/UseCases/RegisterUser/RegisterUserRequest.cs` - Registration request DTO
- `src/UserManagement.Application/UseCases/RegisterUser/RegisterUserResponse.cs` - Registration response DTO with HATEOAS
- `src/UserManagement.Application/UseCases/RegisterUser/RegisterUserHandler.cs` - Registration use case handler
- `src/UserManagement.Application/UseCases/RegisterUser/HateoasLink.cs` - HATEOAS link model
- `src/UserManagement.Application/Interfaces/IUserRepository.cs` - User repository interface
- `src/UserManagement.Application/Interfaces/IPasswordHasher.cs` - Password hashing interface
- `src/UserManagement.Application/Interfaces/IEmailService.cs` - Email service interface
- `tests/Tests.UserManagement.Application/Tests.UserManagement.Application.csproj` - Application tests
- `tests/Tests.UserManagement.Application/UseCases/RegisterUserHandlerTests.cs` - Handler unit tests

### Infrastructure Layer

- `src/UserManagement.Infrastructure/Persistence/ApplicationDbContext.cs` - EF Core DbContext
- `src/UserManagement.Infrastructure/Persistence/Configurations/UserConfiguration.cs` - User entity configuration
- `src/UserManagement.Infrastructure/Persistence/Configurations/UserProfileConfiguration.cs` - UserProfile configuration
- `src/UserManagement.Infrastructure/Persistence/Configurations/AddressConfiguration.cs` - Address configuration
- `src/UserManagement.Infrastructure/Persistence/Repositories/UserRepository.cs` - User repository implementation
- `src/UserManagement.Infrastructure/Services/Argon2PasswordHasher.cs` - Argon2 password hashing
- `src/UserManagement.Infrastructure/Services/SmtpEmailService.cs` - SMTP email service
- `src/UserManagement.Infrastructure/Migrations/` - EF Core migrations
- `tests/Tests.UserManagement.Infrastructure/Tests.UserManagement.Infrastructure.csproj` - Infrastructure tests
- `tests/Tests.UserManagement.Infrastructure/Repositories/UserRepositoryTests.cs` - Repository integration tests
- `tests/Tests.UserManagement.Infrastructure/Services/Argon2PasswordHasherTests.cs` - Password hasher tests

### API Layer

- `src/UserManagement.Api/Controllers/UsersController.cs` - Registration endpoint controller
- `src/UserManagement.Api/Middleware/GlobalExceptionHandler.cs` - RFC 7807 Problem Details middleware
- `src/UserManagement.Api/Models/ProblemDetailsResponse.cs` - RFC 7807 response model
- `src/UserManagement.Api/Extensions/ServiceCollectionExtensions.cs` - DI registration extensions
- `src/UserManagement.Api/appsettings.json` - Configuration (updated)
- `tests/Tests.UserManagement.Api/Controllers/UsersControllerTests.cs` - Controller unit tests
- `tests/Tests.UserManagement.Api/Integration/RegisterUserIntegrationTests.cs` - E2E integration tests

---

## Tasks

- [x] 0.1 Create feature branch `feature/0001-register-user` from `main`
- [x] 0.2 Verify all dependencies are installed and projects build successfully

### 1.0 Setup Shared Kernel Project

- [x] 1.1 Create `src/Shared.Kernel` project with `dotnet new classlib`
- [x] 1.2 Create `tests/Tests.Shared.Kernel` test project with `dotnet new xunit3`
- [x] 1.3 Add Shared.Kernel reference to Tests.Shared.Kernel
- [x] 1.4 Update Directory.Packages.props with required NuGet packages (AwesomeAssertions)
- [x] 1.5 Write failing test for `Result.Success()` in `ResultTests.cs` (TDD)
- [x] 1.6 Implement `Result` class with `IsSuccess`, `IsFailure`, `Error` properties
- [x] 1.7 Write failing test for `Result.Failure(error)` (TDD)
- [x] 1.8 Implement `Result.Failure()` static factory method
- [x] 1.9 Write failing tests for `Result<T>.Success(value)` in `ResultTTests.cs` (TDD)
- [x] 1.10 Implement `Result<T>` class with `Value` property and factory methods
- [x] 1.11 Write failing test for `Result<T>.Failure(error)` (TDD)
- [x] 1.12 Implement error handling in `Result<T>` (throw when accessing Value on failure)
- [x] 1.13 Create `Error` record with `Code`, `Message` properties
- [x] 1.14 Write failing tests for `IValidationRule<T>` interface usage (TDD)
- [x] 1.15 Implement `IValidationRule<T>` interface with `Validate(T value)` method returning `Result`
- [x] 1.16 Write failing tests for `RuleComposer<T>` in `RuleComposerTests.cs` (TDD)
- [x] 1.17 Implement `RuleComposer<T>` class that composes multiple `IValidationRule<T>` instances
- [x] 1.18 Write failing tests for `IRequestHandler<TRequest, TResponse>` interface (TDD)
- [x] 1.19 Implement `IRequestHandler<TRequest, TResponse>` interface with `Handle()` method
- [x] 1.20 Run all Shared.Kernel tests and ensure 100% pass rate
- [x] 1.21 Commit: "feat(shared-kernel): add Result, RuleComposer, and RequestHandler patterns"

### 2.0 Implement Domain Layer (Entities, ValueObjects, Discriminated Unions)

- [x] 2.1 Create `tests/Tests.UserManagement.Domain` project with `dotnet new xunit3`
- [x] 2.2 Add references: Shared.Kernel, UserManagement.Domain to test project
- [x] 2.3 Write failing test for `UserId` strongly-typed ID in `UserIdTests.cs` (TDD)
- [x] 2.4 Implement `UserId` record inheriting from `StronglyTypedId<Guid>`
- [x] 2.5 Implement base class `StronglyTypedId<T>` with `Value` property
- [x] 2.6 Write failing tests for `UserProfileId` and `AddressId` (TDD)
- [x] 2.7 Implement `UserProfileId` and `AddressId` strongly-typed IDs
- [x] 2.8 Write failing tests for `Email` value object in `EmailTests.cs` (TDD - basic creation)
- [x] 2.9 Implement `Email` record with `Value` property and `EmailFactory.Create()` method
- [ ] 2.10 Write failing test for Email validation with RuleComposer (TDD)
- [ ] 2.11 Implement Email validation in `EmailFactory.Create()` returning `Result<Email>`
- [ ] 2.12 Write failing tests for `Name` value object (FirstName, LastName) (TDD)
- [ ] 2.13 Implement `Name` record with validation in `NameFactory.Create()`
- [ ] 2.14 Write failing tests for `PasswordHash` value object (TDD)
- [ ] 2.15 Implement `PasswordHash` record with factory method (no plain-text storage)
- [ ] 2.16 Write failing tests for `UserRole` value object with predefined values (TDD)
- [ ] 2.17 Implement `UserRole` record with static instances (User, Admin)
- [ ] 2.18 Write failing tests for `VerificationToken` value object with expiration (TDD)
- [ ] 2.19 Implement `VerificationToken` record with `Token`, `ExpiresAt` properties
- [ ] 2.20 Write failing tests for Address component value objects (Street, City, PostalCode, Country) (TDD)
- [ ] 2.21 Implement Address component value objects with factories
- [ ] 2.22 Write failing tests for `EmailVerificationStatus` discriminated union (TDD)
- [ ] 2.23 Implement `EmailVerificationStatus` abstract record with NotVerified, Verified, Expired variants
- [ ] 2.24 Implement `EmailVerificationStatusFactory` with factory methods for each variant
- [ ] 2.25 Write failing tests for `Address` entity with Null Object Pattern in `AddressTests.cs` (TDD)
- [ ] 2.26 Implement `Address` entity with `AddressId`, properties, and `Address.Empty` static instance
- [ ] 2.27 Implement `Address.IsEmpty` property to identify empty addresses
- [ ] 2.28 Implement `AddressFactory.Create()` returning `Result<Address>`
- [ ] 2.29 Write failing tests for `UserProfile` entity in `UserProfileTests.cs` (TDD)
- [ ] 2.30 Implement `UserProfile` entity with `UserProfileId`, `UserId`, `FirstName`, `LastName`, navigation to `Address`
- [ ] 2.31 Implement `UserProfileFactory.Create()` returning `Result<UserProfile>`
- [ ] 2.32 Implement `UserProfile.HasAddress()` business method
- [ ] 2.33 Write failing tests for `User` entity in `UserTests.cs` (TDD)
- [ ] 2.34 Implement `User` entity with `UserId`, `Email`, `PasswordHash`, `Role`, `EmailVerificationStatus`, timestamps
- [ ] 2.35 Implement navigation property to `UserProfile` in `User`
- [ ] 2.36 Implement `UserFactory.Create()` returning `Result<User>`
- [ ] 2.37 Implement `User.VerifyEmail()` business method (changes status to Verified)
- [ ] 2.38 Implement `User.IsEmailVerified()` business method
- [ ] 2.39 Add minimum 2 assertions per factory method for pre/postconditions
- [ ] 2.40 Run all Domain tests and ensure 100% pass rate
- [ ] 2.41 Commit: "feat(domain): add User, UserProfile, Address entities with value objects"

### 3.0 Implement Application Layer (Request/Response Pattern, Validation Rules, Use Cases)

- [ ] 3.1 Create `tests/Tests.UserManagement.Application` project with `dotnet new xunit3`
- [ ] 3.2 Add references: Shared.Kernel, UserManagement.Domain, UserManagement.Application to test project
- [ ] 3.3 Write failing test for `EmailNotEmptyRule` in `EmailNotEmptyRuleTests.cs` (TDD)
- [ ] 3.4 Implement `EmailNotEmptyRule` implementing `IValidationRule<string>`
- [ ] 3.5 Write failing test for `OnlyOneAtSymbolRule` (TDD)
- [ ] 3.6 Implement `OnlyOneAtSymbolRule` (exactly one @ symbol validation)
- [ ] 3.7 Write failing test for `LocalPartMaxLengthRule` (TDD)
- [ ] 3.8 Implement `LocalPartMaxLengthRule` (max 64 chars before @)
- [ ] 3.9 Write failing test for `DomainPartMaxLengthRule` (TDD)
- [ ] 3.10 Implement `DomainPartMaxLengthRule` (max 255 chars after @)
- [ ] 3.11 Write failing test for `NoTrailingDotRule` (TDD)
- [ ] 3.12 Implement `NoTrailingDotRule` (no trailing dot in domain)
- [ ] 3.13 Write failing test for `NoNewlineCharactersRule` (TDD)
- [ ] 3.14 Implement `NoNewlineCharactersRule` (no \n, \r in email)
- [ ] 3.15 Write failing test for `NoUnderscoreInDomainRule` (TDD)
- [ ] 3.16 Implement `NoUnderscoreInDomainRule` (no _ in domain part)
- [ ] 3.17 Write failing test for `ValidIpAddressRule` (TDD)
- [ ] 3.18 Implement `ValidIpAddressRule` (valid IP in brackets)
- [ ] 3.19 Continue implementing remaining 39+ atomic email validation rules (TDD for each)
- [ ] 3.20 Write test for each rule corresponding to the 47 test cases from Rust suite
- [ ] 3.21 Ensure each rule has corresponding unit test with minimum 2 assertions
- [ ] 3.22 Integrate all email rules into `EmailFactory.Create()` via RuleComposer
- [ ] 3.23 Write failing test for password length validation rule (TDD)
- [ ] 3.24 Implement `PasswordMinLengthRule` (minimum 16 characters)
- [ ] 3.25 Write failing tests for `RegisterUserRequest` DTO validation (TDD)
- [ ] 3.26 Implement `RegisterUserRequest` record with all required properties
- [ ] 3.27 Write failing tests for `RegisterUserResponse` DTO with HATEOAS links (TDD)
- [ ] 3.28 Implement `RegisterUserResponse` record with `UserId`, `Email`, `Message`, `Links` properties
- [ ] 3.29 Implement `HateoasLink` record with `Href`, `Method`, `Description` properties
- [ ] 3.30 Write failing tests for `IUserRepository` interface contract (TDD)
- [ ] 3.31 Create `IUserRepository` interface with `ExistsAsync()`, `AddAsync()`, `SaveChangesAsync()` methods
- [ ] 3.32 Create `IPasswordHasher` interface with `Hash()`, `Verify()` methods
- [ ] 3.33 Create `IEmailService` interface with `SendVerificationEmailAsync()` method
- [ ] 3.34 Write failing tests for `RegisterUserHandler` in `RegisterUserHandlerTests.cs` (TDD)
- [ ] 3.35 Implement `RegisterUserHandler` implementing `IRequestHandler<RegisterUserRequest, Result<RegisterUserResponse>>`
- [ ] 3.36 Inject `IUserRepository`, `IPasswordHasher`, `IEmailService` into handler constructor
- [ ] 3.37 Implement duplicate email check in handler (returns generic error for security)
- [ ] 3.38 Implement password hashing in handler using `IPasswordHasher`
- [ ] 3.39 Implement User, UserProfile, Address creation using factory methods
- [ ] 3.40 Implement repository save operation in handler
- [ ] 3.41 Implement email verification sending in handler (async, fire-and-forget)
- [ ] 3.42 Implement HATEOAS link generation in response (self, verify-email, resend-verification, login)
- [ ] 3.43 Ensure minimum 2 assertions per handler method for pre/postconditions
- [ ] 3.44 Run all Application tests and ensure 100% pass rate
- [ ] 3.45 Commit: "feat(application): add RegisterUser use case with validation rules"

### 4.0 Implement Infrastructure Layer (Repository, EF Core, Email Service, Password Hasher)

- [ ] 4.1 Create `tests/Tests.UserManagement.Infrastructure` project with `dotnet new xunit3`
- [ ] 4.2 Add references: Shared.Kernel, Domain, Application, Infrastructure to test project
- [ ] 4.3 Add NuGet packages: EntityFrameworkCore, Npgsql.EntityFrameworkCore.PostgreSQL, Konscious.Security.Cryptography.Argon2
- [ ] 4.4 Write failing test for `Argon2PasswordHasher.Hash()` in `Argon2PasswordHasherTests.cs` (TDD)
- [ ] 4.5 Implement `Argon2PasswordHasher` implementing `IPasswordHasher`
- [ ] 4.6 Configure Argon2 parameters: memory=64MB, iterations=3, parallelism=4
- [ ] 4.7 Write failing test for `Argon2PasswordHasher.Verify()` (TDD)
- [ ] 4.8 Implement password verification in Argon2PasswordHasher
- [ ] 4.9 Write failing test for `SmtpEmailService.SendVerificationEmailAsync()` (TDD)
- [ ] 4.10 Implement `SmtpEmailService` implementing `IEmailService` with SMTP configuration
- [ ] 4.11 Implement email template for verification email (HTML/plain text)
- [ ] 4.12 Write failing integration test for `ApplicationDbContext` creation (TDD)
- [ ] 4.13 Implement `ApplicationDbContext` with `DbSet<User>`, `DbSet<UserProfile>`, `DbSet<Address>`
- [ ] 4.14 Write failing test for `UserConfiguration` EF Core entity configuration (TDD)
- [ ] 4.15 Implement `UserConfiguration` with table mapping, primary key, indexes (unique on Email.Value)
- [ ] 4.16 Implement EF Core Value Converter for `Email` value object
- [ ] 4.17 Implement EF Core Value Converter for `PasswordHash` value object
- [ ] 4.18 Implement EF Core Value Converter for `UserRole` value object
- [ ] 4.19 Implement EF Core Value Converter for `UserId`, `UserProfileId`, `AddressId` strongly-typed IDs
- [ ] 4.20 Implement EF Core Owned Entity or Value Converter for `EmailVerificationStatus` discriminated union
- [ ] 4.21 Write failing test for `UserProfileConfiguration` (TDD)
- [ ] 4.22 Implement `UserProfileConfiguration` with table mapping, relationships, value converters
- [ ] 4.23 Write failing test for `AddressConfiguration` (TDD)
- [ ] 4.24 Implement `AddressConfiguration` with value converters for address components
- [ ] 4.25 Implement Query Filter for `Address.Empty` (optional: filter out empty addresses)
- [ ] 4.26 Write failing integration tests for `UserRepository.ExistsAsync()` in `UserRepositoryTests.cs` (TDD)
- [ ] 4.27 Implement `UserRepository` implementing `IUserRepository`
- [ ] 4.28 Implement `ExistsAsync()` method with Email lookup
- [ ] 4.29 Write failing test for `UserRepository.AddAsync()` (TDD)
- [ ] 4.30 Implement `AddAsync()` method
- [ ] 4.31 Write failing test for `UserRepository.SaveChangesAsync()` (TDD)
- [ ] 4.32 Implement `SaveChangesAsync()` method
- [ ] 4.33 Create initial EF Core migration: `dotnet ef migrations add InitialCreate`
- [ ] 4.34 Review generated migration SQL for correctness (indexes, constraints, value converters)
- [ ] 4.35 Run all Infrastructure tests (including integration tests with in-memory DB)
- [ ] 4.36 Commit: "feat(infrastructure): add EF Core, repositories, and services"

### 5.0 Implement API Layer (Controller, Middleware, HATEOAS)

- [ ] 5.1 Write failing test for `GlobalExceptionHandler` in `GlobalExceptionHandlerTests.cs` (TDD)
- [ ] 5.2 Implement `GlobalExceptionHandler` middleware for RFC 7807 Problem Details format
- [ ] 5.3 Implement `ProblemDetailsResponse` model with `Type`, `Title`, `Status`, `Detail`, `Instance`, `Errors` properties
- [ ] 5.4 Write failing test for validation error mapping to Problem Details (TDD)
- [ ] 5.5 Implement error mapping logic in middleware
- [ ] 5.6 Write failing test for `UsersController.Register()` endpoint in `UsersControllerTests.cs` (TDD)
- [ ] 5.7 Implement `UsersController` with `Register()` POST endpoint at `/api/v1/users/register`
- [ ] 5.8 Inject `IRequestHandler<RegisterUserRequest, Result<RegisterUserResponse>>` into controller
- [ ] 5.9 Implement request validation in controller (model state validation)
- [ ] 5.10 Call handler in controller and map result to HTTP response
- [ ] 5.11 Return HTTP 201 Created with Location header on success
- [ ] 5.12 Return HTTP 400 Bad Request with Problem Details on validation failure
- [ ] 5.13 Return HTTP 409 Conflict with generic error on duplicate email (security)
- [ ] 5.14 Return HTTP 500 Internal Server Error with Problem Details on unexpected errors
- [ ] 5.15 Write failing test for HATEOAS links in response (TDD)
- [ ] 5.16 Implement HATEOAS link generation based on user state
- [ ] 5.17 Create `ServiceCollectionExtensions` for DI registration
- [ ] 5.18 Register `RegisterUserHandler` in DI container (no dispatcher!)
- [ ] 5.19 Register `IUserRepository` → `UserRepository` in DI
- [ ] 5.20 Register `IPasswordHasher` → `Argon2PasswordHasher` in DI
- [ ] 5.21 Register `IEmailService` → `SmtpEmailService` in DI
- [ ] 5.22 Register `ApplicationDbContext` with PostgreSQL connection string in DI
- [ ] 5.23 Update `Program.cs` to call `ServiceCollectionExtensions`
- [ ] 5.24 Update `appsettings.json` with PostgreSQL connection string, SMTP settings
- [ ] 5.25 Update `appsettings.Development.json` with development-specific settings
- [ ] 5.26 Configure OpenAPI/Swagger for API documentation (optional)
- [ ] 5.27 Run all API unit tests and ensure 100% pass rate
- [ ] 5.28 Commit: "feat(api): add UsersController with registration endpoint"

### 6.0 Integration Tests & End-to-End Validation

- [ ] 6.1 Write integration test for complete registration flow in `RegisterUserIntegrationTests.cs` (TDD)
- [ ] 6.2 Setup test database (in-memory or test PostgreSQL instance)
- [ ] 6.3 Test successful registration with valid data (HTTP 201, Location header, HATEOAS links)
- [ ] 6.4 Test registration with invalid email format (HTTP 400, Problem Details)
- [ ] 6.5 Test registration with short password (HTTP 400, Problem Details)
- [ ] 6.6 Test registration with duplicate email (HTTP 409, generic error message)
- [ ] 6.7 Test registration with missing required fields (HTTP 400, field-specific errors)
- [ ] 6.8 Test registration with optional address (Address.Empty is used)
- [ ] 6.9 Test registration with provided address (Address is saved)
- [ ] 6.10 Test email verification email is sent (verify IEmailService mock call)
- [ ] 6.11 Test password is hashed with Argon2 (verify hash format)
- [ ] 6.12 Test User role is automatically assigned
- [ ] 6.13 Test EmailVerificationStatus is NotVerified with token
- [ ] 6.14 Test all 47 email validation rules via integration tests
- [ ] 6.15 Test HATEOAS links are state-dependent (verify links based on user state)
- [ ] 6.16 Test RFC 7807 Problem Details format for all error cases
- [ ] 6.17 Verify database schema matches expectations (EF Core migration)
- [ ] 6.18 Verify indexes exist (unique index on Email.Value)
- [ ] 6.19 Performance test: registration completes in < 500ms (without email send)
- [ ] 6.21 Commit: "test(integration): add end-to-end registration tests"

### Final Steps

- [ ] 7.1 Run all tests across all projects: `dotnet test`
- [ ] 7.1 Run all tests across all projects: `dotnet test`
- [ ] 7.2 Verify test coverage > 80% (use coverage tool)
- [ ] 7.3 Review code for NASA safety rules compliance (min 2 assertions, bounded loops, etc.)
- [ ] 7.5 Create pull request from `feature/0001-register-user` to `main`
- [ ] 7.6 Code review checklist: Clean Architecture, TDD, Result\<T\>, Null Object Pattern, atomic rules
- [ ] 7.7 Merge to `main` after approvalrchitecture, TDD, Result<T>, Null Object Pattern, atomic rules
- [ ] 7.7 Merge to `main` after approval

---

## Notes

- **TDD Approach:** For each task, write failing tests first, then implement code to make tests pass
- **Test Naming:** Follow existing xUnit v3 conventions from `Tests.UserManagement.Api` project
- **NO Comments:** Avoid 'Arrange', 'Act', 'Assert' comments in tests
- **Atomic Commits:** Commit after each completed sub-task with meaningful commit messages
- **Assertions:** Minimum 2 assertions per function (NASA-inspired safety rules)
- **Feature Branch:** All work must be done in `feature/0001-register-user` branch
- **Assertion Library:** Use **AwesomeAssertions** (<https://awesomeassertions.org>) for all test assertions
  - Example: `result.Should().BeSuccess()` instead of `Assert.True(result.IsSuccess)`
  - AwesomeAssertions provides fluent, expressive assertions for better test readability
  - Supports custom assertions and extension methods for domain-specific types
  - Better error messages compared to traditional xUnit assertions
