# QuickApp AI Development Rules

**Reference this file in your AI prompts when extending QuickApp.**

## Quick Start

When asking AI to extend QuickApp, include this in your prompt:
- Reference the AI rules file: ai-rules/AI_RULES.md
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

This project supports **two data access technologies**: Entity Framework Core for standard CRUD on owned entities (writes, migrations, change tracking), and Dapper via DataAccessLibrary for read-heavy queries, complex multi-table joins, reporting views, and cross-database queries. Use EF Core for writes and Dapper for reads/reporting. Always use parameterized queries. Inject IDataAccessService for Dapper alongside ApplicationDbContext.

| Technology | When to Use |
|---|---|
| **Entity Framework Core** | Standard CRUD on owned entities, migrations, change tracking, and relationship navigation |
| **Dapper** (via `DataAccessLibrary`) | Read-heavy queries, complex multi-table joins, reporting views, cross-database queries, and any scenario where raw SQL performance matters |

**Data Access Rules:**

1. ✅ **Use EF Core** for write operations on EF-tracked entities (insert, update, delete)
2. ✅ **Use Dapper** for read-heavy or reporting queries (e.g., views like `VwhConsultingDetailsForBillingAlt`)
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

### Components

**Component Rules:**

1. ✅ **Use standalone components** - no NgModules
2. ✅ **Use `inject()` function** for dependency injection (not constructor injection)
3. ✅ **Implement `OnInit`** for initialization logic
4. ✅ **Use `fadeInOut` animation** from `services/animations`
5. ✅ **Import `TranslateModule`** for i18n support
6. ✅ **Use `AlertService`** for loading states and error messages
7. ✅ **Use `loadingIndicator`** boolean for UI loading states
8. ✅ **Cache data in `rowsCache`** for filtering/searching
9. ✅ **Handle errors** in subscribe error callback
10. ✅ **For main pages include page header with fadeInOut animation**. See `products.component.html` for the standard pattern.
11. ❌ **DO NOT use constructor injection** - use `inject()` function
12. ❌ **DO NOT make HTTP calls directly** - use endpoint services

**Admin user registration (create user):** There is no public self-service register route. Creating a user is done from **Settings → Users** via `UsersManagementComponent`, which opens `UserInfoComponent` with `isGeneralEditor` and `isNewUser`. The form posts to `POST /api/account/users` with `UserEdit` (including `roles: string[]`, `newPassword`, and profile fields). The **Role assignment** section uses `ng-select` with multiple selection; users who can assign roles need the `assignRoles` permission. Backend reference: `UserAccountController.Register` and `UserEditVM.Roles` in `UserVMs.cs`.

### Reusable Components

**Use `attendance-summary.component` as a reusable standalone UI component** and display it in the header sections of all clinical pages; the component is already used in the Add Invoice dialog header, and `BillNo` is the same as `consultID`. **AttendanceSummaryComponent is the sole source of truth for the receipt dialog header, and no extra patient photo lookup from HPatients should be added for that header flow.** However, it is acceptable to load the patient photo from HPatients and supply it to AttendanceSummaryComponent for the receipt dialog attendance header flow.

### Services

**ALL API services MUST extend `EndpointBase`** which provides:

- Automatic token refresh on 401 errors
- Request headers with Bearer token
- Error handling with retry logic

**Service Rules:**

1. ✅ **MUST extend `EndpointBase`**
2. ✅ **Use `@Injectable({ providedIn: 'root' })`**
3. ✅ **Use `inject()` function** for dependencies
4. ✅ **Use getter for URL** (e.g., `get productsUrl()`)
5. ✅ **Use `this.requestHeaders`** from base class (includes Bearer token)
6. ✅ **Use `this.handleError()`** from base class for error handling
7. ✅ **Use `JSON.stringify()`** for POST/PUT requests
8. ✅ **Return `Observable<T>`** with generic type
9. ✅ **Follow naming**: `get{Action}{Entity}Endpoint` (e.g., `getProductsEndpoint`, `getNewProductEndpoint`)
10. ❌ **DO NOT create HTTP client directly** - extend EndpointBase

### Models

**Model Rules:**

1. ✅ **Use TypeScript interfaces** (not classes for data models)
2. ✅ **Use `?` for optional properties**
3. ✅ **Match ViewModel structure** from backend
4. ✅ **Use camelCase** for property names (TypeScript convention)
5. ✅ **Flatten navigation properties** (e.g., `categoryName` instead of `category.name`)
6. ❌ **DO NOT use classes** for simple data models

### Routing

**Routing Rules:**

1. ✅ **Use lazy loading** with `loadComponent`
2. ✅ **Use `AuthGuard`** for protected routes
3. ✅ **Set `title`** for each route
4. ✅ **Use `path: '**'` for 404 route (must be last)**
5. ❌ **DO NOT use eager loading** - always lazy load feature components

**Example:**{
  path: 'products',
  loadComponent: () => import('./components/products/products.component').then(m => m.ProductsComponent),
  canActivate: [AuthGuard],
  title: 'Products'
}

### Translation Files

**Important**: When adding new UI text, add translation keys to all locale files.

**Location**: `quickapp.client/public/locale/`  
**Files**: `en.json`, `fr.json`, `de.json`, `es.json`, `pt.json`, `zh.json`, `ko.json`, `ar.json`

**Usage in Templates:**<h4>{{ 'Products' | translate }}</h4>
<p>{{ 'Description' | translate }}</p>

**Translation Rules:**

1. ✅ **Add keys to `en.json` first** (primary language)
2. ✅ **Add same keys to all other locale files** (can use English as placeholder)
3. ✅ **Use nested structure** for organization (e.g., `pageHeader.Products`, `mainMenu.Customers`)
4. ✅ **Use `translate` pipe** in templates
5. ❌ **DO NOT hardcode strings** in templates or components

### Error Handling

**Error Handling Pattern:**loadData(): void {
  this.alertService.startLoadingMessage();
  this.loadingIndicator = true;

  this.endpoint.getItemsEndpoint<Item[]>()
    .subscribe({
      next: items => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.items = items;
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage(
          'Load Error',
          `Unable to retrieve items.\r\nError: "${this.getErrorMessage(error)}"`,
        );
      }
    });
}

### Patient Management

**Private Patients Identification:**
- Check if a patient is private by verifying `HRetainership.RetainCode = "0001"`.
- Use `Patient.CoyName` as the foreign key linking to `HRetainership.RetainCode`.
- Note that debt carry-forward applies **ONLY** to private patients.
- Avoid using `CoyType` - utilize the retainership lookup instead.

**Debt Carry-Forward Logic:**
- The debt carry-forward logic is implemented in `AttendanceService.cs`.
- The main debt methods are:
  - `SaveDebtAsync`: Calculates and updates patient debt (called **before** saving changes to the database).
  - `SaveBillAsync`: Creates a billing record with the debt (called **after** saving changes to the database).
- These methods are invoked from `CreateAsync` during attendance saving.

**Billing Debt Flow Logic:**
- In this codebase's billing debt flow logic, DebtBF (debt brought forward from previous transaction) must be included in debt calculations/running balance.

**Multi-Clinic Data Isolation (HConsulting):**
- **Each clinic must have its own entry** in the `HConsulting` table (e.g., dental clinic, aesthetics clinic, etc.)
- **Each consultation is identified by `consultID`** and belongs to a specific clinic
- **Clinic pages can ONLY update their own records** - filter queries by the clinic identifier and consultation ID
- **DO NOT allow cross-clinic access** - enforce clinic isolation at the service/controller level
- **All HConsulting CRUD operations MUST validate** that the current clinic owns the record before allowing read/write
- ✅ **Reference**: Filter HConsulting queries using clinic context from `GetCurrentUserId()` or clinic claim in JWT token
- ❌ **DO NOT allow generic HConsulting access** without clinic validation

### Billing Consultation UI

For billing consultation sub-header UI, when multiple `VwhConsultingDetailsForBillingAlt` records exist per `consultId/billNo`, the component should iterate all records while using minimal screen space (compact layout).

### Alert Banner Focus Rule

- **Alert banner must show on the UI layer that has focus** (active dialog or parent page).
- **Never allow alert banners/toasts to render behind modal dialogs or overlays.**

### Bank Account Dropdown

- For receipt bank account dropdown, always use `vwAccountsInfo` in Accounting DB as the single source of truth, filter by `emrAppDefaults Acct_Banks` using case/trim-safe `GroupId` matching, use `AccountNo` as account id, and do not use `hRevenueTypes` fallback. In this flow, `AccountNo`, `AccountId`, and `AccountName` are expected to be non-empty after that bank-group filter.

### Login/Auth Card UI Rule

When adding Google login or any external authentication buttons to login/auth cards:

1. ✅ Keep buttons inside the card content container only
2. ✅ Use `width: 100%`, `max-width: 100%`, and `box-sizing: border-box` on external-login button containers
3. ✅ Keep card border explicit (`border-style`, `border-width`, `border-color`) so it remains consistent across breakpoints
4. ❌ Do not use negative margins or overflow-breaking positioning that can clip or visually break the card border

### Configuration Management

When introducing new configuration keys, also update the base `appsettings.json` alongside environment-specific files.

### Custom Rules for Entry Form UI

**Entry form UI implementation design:**
- Create a listing/worklist page (search, table, add/edit actions), like Dental Clinical Session.
- Create a separate dialog component (for both New and Edit), as seen in DentalEncounterDialogComponent.
- Use one reusable dialog for create/update:
  - Open empty for new entry.
  - Open prefilled for edit entry.
- Implement full CRUD operations for the UI.
- Save from dialog, close dialog, then refresh parent list.
- Create a header section in the add/edit dialog page.
- When a patient is selected, it should display the AttendanceSummary component (patient attendance summary) in the header section of the dialog page.
- If the add/edit dialog page has tabs, keep patient header/summary and tabbed form inside the dialog, not the main page.
- Use Angular Material/material icons instead of Bootstrap.
- Material table/grid should have page size = 10.
- use @ng-select/ng-select as select dropdown with searchable 
- The dialog can only be explicitly closed using the close (X) icon or cancel button.
- The entry form UI must be responsive (for mobile, tablet, and desktop devices).

### Date Format for Expenses

- Use 'dd-MMM-yyyy' date format for the expenses page and expenses dialog date inputs/displays.

### Optimistic UI Updates

- User prefers not to show optimistic/fake records in grids before server-confirmed save/update; UI should only reflect records after authoritative reload.

### Accounting Transactions

For accounting transactions (Tranxaction, TranxactionJournalTemp, TranxactionJournal):
- Update/delete operations must use `TranID` (same as `TranNo`).
- Delete must call `Deletetranxaction` with `TranID`.
- Updates should be implemented as delete-then-insert.
- **Insert operations must use the `InsertTranxaction` stored procedure.**
- **Insert operations for `TranxactionJournal` must use `InsertTranxactionJournal` to insert into `TranxactionJournal`.**
- **`TranxactionJournalTemp` uses direct SQL insert (no stored procedure).**
- When implementing accounting delete/update flows, do not use fallback values for delete stored procedure parameters; use explicit parameters from the UI grid row/record.

### Expense Posting Flow

- Transaction dates must come from the UI payload (`TranDate`) and not from `DateTime.Now`.

### Search Interactions

- User prefers Angular Signals over RxJS in UI features; always use Signals where possible except when Signals cannot support the required feature.

### SQL/View Definition Usage

When the user provides an explicit SQL/view definition for a feature, use it exactly as-is with no fallback or inferred extra fields.

### Accounting GroupID Prefix Mapping

- Use the following mapping when implementing accounting account filters:
  - '1' Assets
  - '2' Liabilities
  - '3' Equity
  - '4' Sales/Revenue/Income
  - '5' Expenses

### Accounting Wrapper Pages

For accounting wrapper pages (expenses, incomes, journal, debtors, creditors), use the same underlying table/grid data source pattern across all pages.





