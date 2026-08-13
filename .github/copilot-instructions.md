# QuickApp AI Development Rules

**Reference this file in your AI prompts when extending QuickApp.**

## Quick Start

When asking AI to extend QuickApp, include this in your prompt:
- Follow the exact patterns and conventions documented in this file.
- Reference UserAccountController, UserRoleController, and UserVMs.cs for real implementation examples.

## What QuickApp Provides

QuickApp is a **hardened foundation** that solves the hard problems so AI can focus on features:

### ✅ Already Solved (Don't Let AI Recreate)

- **Authentication**: OpenIddict/OAuth2 with JWT tokens
- **Authorization**: Role and permission-based policies
- **Error Handling**: Centralized patterns
- **Logging**: Already wired
- **Database**: Entity Framework Core with migrations
- **API Structure**: Consistent controller patterns
- **Frontend Auth**: Guards and token management
- **Service Layer**: EndpointBase with automatic token refresh

### 🎯 What AI Should Build

- **New Entities**: Following BaseEntity pattern
- **New Features**: Following existing CRUD patterns
- **UI Components**: Following Angular component structure
- **Business Logic**: In services, not controllers or components

---

## Backend Patterns (ASP.NET Core)

### Project Structure
QuickApp.Core/
  ├── Models/           # Domain entities
  │   ├── Account/      # User, Role, Permission entities
  │   └── Shop/         # Business domain entities
  ├── Services/         # Business logic interfaces and implementations
  └── Infrastructure/  # DbContext, Database seeding

QuickApp.Server/
  ├── Controllers/      # API controllers
  ├── ViewModels/       # DTOs for API responses/requests
  ├── Authorization/   # Policies, requirements, handlers
  └── Configuration/    # AutoMapper, OIDC config

### Entity Models

**ALL entities MUST inherit from `BaseEntity`** which provides:

- `Id` (int)
- `CreatedBy`, `UpdatedBy` (string?, max 40 chars)
- `CreatedDate`, `UpdatedDate` (DateTime)

**Entity Rules:**

1. ✅ **MUST inherit from `BaseEntity`**
2. ✅ **Use `required` keyword** for non-nullable reference types
3. ✅ **Use `string?` for optional string properties**
4. ✅ **Navigation properties use `ICollection<T>` with initializer `[]`**
5. ✅ **Foreign keys follow pattern: `{RelatedEntity}Id`**
6. ❌ **DO NOT add validation attributes to entities** (use ViewModels)
7. ❌ **DO NOT add business logic to entities** (use Services)

### ViewModels (DTOs)

**Location**: `QuickApp.Server/ViewModels/{Domain}/`

**Validation**: Use **DataAnnotations** for ViewModel validation. See `UserVMs.cs` for examples. FluentValidation is available but DataAnnotations is preferred.

**ViewModel Rules:**

1. ✅ **Use `VM` suffix** (e.g., `UserVM`, `RoleVM`)
2. ✅ **Use DataAnnotations** (`[Required]`, `[StringLength]`, `[EmailAddress]`, etc.)
3. ✅ **Flatten navigation properties** when needed (e.g., `CategoryName` instead of `Category.Name`)
4. ✅ **Use nullable reference types** (`string?`) for optional fields
5. ✅ **Include `Id` for updates, exclude for creates**
6. ❌ **DO NOT include navigation collections** unless specifically needed

**Reference**: See `QuickApp.Server/ViewModels/Account/UserVMs.cs` for validation examples.

### Services

**Interface Location**: `QuickApp.Core/Services/{Domain}/Interfaces/`  
**Implementation Location**: `QuickApp.Core/Services/{Domain}/`

**Service Rules:**

1. ✅ **Primary constructors are preferred** (C# 12), but traditional constructors are acceptable
2. ✅ **Inject `ApplicationDbContext`** directly, not repository pattern
3. ✅ **Use `Include()` for eager loading** navigation properties
4. ✅ **Use async/await** for database operations
5. ✅ **Return `IEnumerable<T>`** for collections, `T?` for single items
6. ✅ **Use `AsSingleQuery()`** for complex includes to avoid cartesian explosion
7. ❌ **DO NOT use repository pattern** - use DbContext directly
8. ❌ **DO NOT add ViewModels to Core project** - only entities
9. ❌ **DO NOT add authorization logic** - that belongs in controllers

**Service Registration** (in `Program.cs`): builder.Services.AddScoped<IProductService, ProductService>();

### Data Access Platforms

This project supports **two data access technologies**: Entity Framework Core for standard CRUD on owned entities (writes, migrations, change tracking), and Dapper via DataAccessLibrary for read-heavy queries, complex multi-table joins, reporting views, and cross-database queries. Use EF Core for writes. Use Dapper for Legacy vb6/vb.net write operations, reads/reporting. Always use parameterized queries. Inject IDataAccessService for Dapper alongside ApplicationDbContext.

| Technology | When to Use |
|---|---|
| **Entity Framework Core** | Standard CRUD on owned entities, migrations, change tracking, and relationship navigation |
| **Dapper** (via `DataAccessLibrary`) | Legacy vb6/vb.net write operations, Read-heavy queries, complex multi-table joins, reporting views, cross-database queries, and any scenario where raw SQL performance matters |

**Data Access Rules:**

1. ✅ **Use EF Core** for write operations on EF-tracked entities (insert, update, delete)
2. ✅ **Use Dapper** for Legacy vb6/vb.net write operations, read-heavy or reporting queries (e.g., views like `VwhConsultingDetailsForBillingAlt`)
3. ✅ **Use Dapper** for cross-database queries (e.g., Accounting DB, EMR DB)
4. ✅ **Use Dapper** when raw SQL is clearer or more performant than LINQ
5. ✅ **Inject `IDataAccessService` (DataAccessLibrary)** for Dapper queries alongside `ApplicationDbContext`
6. ✅ **Use parameterized queries** always — never string-concatenate SQL
7. ✅ **Respect explicit overrides** — if the user explicitly specifies Dapper or EF Core for an operation, use exactly that technology regardless of the defaults above
8. ❌ **DO NOT mix EF change-tracking with Dapper writes** on the same entity in one transaction
9. ❌ **DO NOT default to one technology for everything** — choose per scenario

### Controllers

**ALL controllers MUST inherit from `BaseApiController`** which provides:

- `_mapper` (IMapper)
- `_logger` (ILogger)
- `GetCurrentUserId()` method
- `AddModelError()` methods

**Controller Rules:**

1. ✅ **MUST inherit from `BaseApiController`**
2. ✅ **Use `[Route("api/[controller]")]`** attribute (or custom route like `[Route("api/account")]`)
3. ✅ **Use `[ApiController]`** attribute (inherited from BaseApiController)
4. ✅ **All endpoints must be protected** - Use `[Authorize]`, `[Authorize(Policy)]`, or inline `AuthorizeAsync()` checks as appropriate
5. ✅ **Use `[ProducesResponseType]`** attributes for Swagger documentation
6. ✅ **Use AutoMapper** to convert between entities and ViewModels
7. ✅ **Return `IActionResult`** (not concrete types)
8. ✅ **Use appropriate HTTP status codes** (e.g., 200 OK, 201 Created, 204 NoContent, 400 BadRequest, 404 NotFound)
9. ✅ **Validate ModelState** before processing
10. ❌ **DO NOT access DbContext directly** - use services
11. ❌ **DO NOT put business logic in controllers** - delegate to services
12. ❌ **DO NOT return entities directly** - always map to ViewModels

**Reference Implementations:**

- `UserAccountController.cs` - Shows authorization patterns (inline `AuthorizeAsync()` checks and policy-based `[Authorize]`)
- `UserRoleController.cs` - Shows CRUD patterns with authorization

### Authorization

All endpoints must be protected. Use one of these approaches:

1. **Policy-based authorization** (attribute): [HttpGet("users")]
[Authorize(AuthPolicies.ViewAllUsersPolicy)]
   public async Task<IActionResult> GetUsers() {...}
2. **Inline authorization checks** (for resource-based authorization): [HttpGet("users/{id}")]
public async Task<IActionResult> GetUserById(string id)
{
    if (!(await _authorizationService.AuthorizeAsync(User, id,
        UserAccountManagementOperations.ReadOperationRequirement)).Succeeded)
        return new ChallengeResult();
    // ... rest of method
}

**Authorization Rules:**

1. ✅ **All endpoints must be protected** - no exceptions
2. ✅ **Use policy constants** from `AuthPolicies` class
3. ✅ **Use inline checks** for resource-based authorization
4. ✅ **Create custom requirements** for complex authorization logic
5. ✅ **Register authorization handlers** in `Program.cs`
6. ❌ **DO NOT hardcode permission strings** - use `ApplicationPermissions` constants
7. ❌ **DO NOT skip authorization** - every endpoint must be protected

**Reference**: See `UserAccountController.cs` lines 39-55, 78-80, 126-132 for authorization patterns.

### AutoMapper Configuration

**Location**: `QuickApp.Server/Configuration/MappingProfile.cs`

**Mapping Rules:**

1. ✅ **Add mappings for all new entities** to their ViewModels
2. ✅ **Use `ReverseMap()`** when bidirectional mapping is needed
3. ✅ **Use `ForMember()`** to customize property mappings
4. ✅ **Use `Ignore()`** for properties that shouldn't be mapped
5. ❌ **DO NOT add business logic** in mapping configuration

### Error Handling

**Error Handling Pattern:**[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    try
    {
        var item = _service.GetById(id);
        if (item == null)
            return NotFound(id);

        await _service.DeleteAsync(id);
        return NoContent();
    }
    catch (CustomException ex)
    {
        _logger.LogError(ex, "Error deleting item {Id}", id);
        AddModelError(ex.Message);
        return BadRequest(ModelState);
    }
}

**Error Handling Rules:**

1. ✅ **Use custom exceptions** for domain-specific errors
2. ✅ **Log errors** using `_logger.LogError()`
3. ✅ **Return appropriate HTTP status codes**
4. ✅ **Use `AddModelError()`** from BaseApiController for validation errors
5. ❌ **DO NOT expose internal exceptions** to API responses

---

## Frontend Patterns (Angular)

### Project Structure
quickapp.client/src/app/
  ├── components/       # Feature components (lazy-loaded)
  ├── services/         # API services (extend EndpointBase)
  ├── models/           # TypeScript interfaces
  └── app.routes.ts     # Route configuration

### Report UI Pattern Baseline (Applies to ALL Reports)

Use `billing-receipt-report.component` as the baseline UI pattern for all report pages in this workspace.

1. ✅ Use a report page shell with responsive padding, centered content, and a top header section.
2. ✅ Header uses left/right layout: left = report icon + title + subtitle, right = export/actions.
3. ✅ Include export links in header (`Excel | CSV | PDF`) plus explicit `Refresh` and `Print Report` actions.
4. ✅ Include summary KPI cards near the top (compact cards with icon, value, and label) for key report metrics.
5. ✅ Summary KPI cards must be computed from the same filtered/paginated report data flow used by the grid/table source of truth.
6. ✅ Summary values must always tally with the active totals/counts represented by the current table/grid dataset (after filters/date range/search/module selectors are applied).
7. ✅ Summary KPI definitions must match the page/module context (for example: billing = revenue/income/payment metrics; frontdesk = attendance/registration/patient flow metrics).
8. ✅ Place filters in a dedicated Angular Material card with responsive wrapping.
9. ✅ Standard report filters should support search, date range (From/To), and domain-specific selectors.
10. ✅ Date inputs must use Angular Material datepicker with `dd-MMM-yyyy` display format.
11. ✅ Date filters should initialize to today by default, and Clear resets filters to default state.
12. ✅ Use an explicit `Run Report` action to apply filters and update summaries/table.
13. ✅ Use Angular Material table inside a horizontal-scroll wrapper with sticky header and row hover state.
14. ✅ Use Angular Material paginator with default page size `10` (additional options allowed when needed).
15. ✅ Provide loading and empty states in the report content area (icon + short status text + reset action for empty).
16. ✅ Use Angular Material icons/buttons consistently for actions and row commands.
17. ✅ Long text cells should be truncated with tooltip for full value.
18. ✅ Provide print-friendly behavior: hide non-report chrome (filters/actions/paginator) in print mode.
19. ✅ Prefer Angular Signals/computed state for report filtering, pagination, and summary calculations.
20. ❌ Do not create one-off report layouts that diverge from this baseline unless explicitly requested.
21. ✅ Apply discretion: this baseline is the default standard, but context-specific reports may intentionally not follow every rule 100% when justified by functional, usability, or domain requirements.
22. ✅ Discretion applies only to report UI presentation/layout. It does not apply to report behavior, business logic, filters, calculations, or data integrity/tally requirements.
23. ✅ Report pages must be responsive across device sizes (desktop, tablet, mobile).
24. ✅ During search, table/grid results must remain constrained to the active date range selected in the date pickers.
25. ✅ When `Run Report` is clicked, table/grid results must remain constrained to the active date range selected in the date pickers.









