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
11. ✅ **For scrollable dialog pages, use the shared `appDialogKeyboardScroll` directive on `mat-dialog-content`** so the vertical scrollbar responds to `ArrowUp` and `ArrowDown` keys.
12. ❌ **DO NOT use constructor injection** - use `inject()` function
13. ❌ **DO NOT make HTTP calls directly** - use endpoint services

**Admin user registration (create user):** There is no public self-service register route. Creating a user is done from **Settings → Users** via `UsersManagementComponent`, which opens `UserInfoComponent` with `isGeneralEditor` and `isNewUser`. The form posts to `POST /api/account/users` with `UserEdit` (including `roles: string[]`, `newPassword`, and profile fields). The **Role assignment** section uses `ng-select` with multiple selection; users who can assign roles need the `assignRoles` permission. Backend reference: `UserAccountController.Register` and `UserEditVM.Roles` in `UserVMs.cs`.

### Reusable Components

**Use `attendance-summary.component` as a reusable standalone UI component** and display it in the header sections of all clinical pages; the component is already used in the Add Invoice dialog header, and `BillNo` is the same as `consultID`. **AttendanceSummaryComponent is the sole source of truth for the receipt dialog header, and no extra patient photo lookup from HPatients should be added for that header flow.** However, it is acceptable to load the patient photo from HPatients and supply it to AttendanceSummaryComponent for the receipt dialog attendance header flow.

### Global Icon Styling Rule

**All new icon usage in the Angular UI must use the shared global icon system** unless a feature has a clear special case.

1. ✅ Use the reusable global icon classes for table action icons, page header icons, tab icons, toolbar icons, and sidebar/navigation icons.
2. ✅ For Angular Material tabs, render tab icons through `ng-template mat-tab-label` so the icon and text share the global icon styling system.
3. ✅ Prefer semantic icon colors and consistent sizing instead of ad-hoc per-component icon styling.
4. ✅ Use shared button/icon utility classes for icon-only buttons so hover, focus, radius, and disabled states stay consistent.
5. ✅ Keep icon styling professional, colorful, and visually balanced across the app.
6. ✅ For dialog top-right close (X) buttons, use the shared `.ui-dialog-close-btn` class instead of redefining per-component close button styles.
7. ❌ Do not create new one-off icon color patterns in feature components when the shared global system fits.
8. ❌ Do not revert to monochrome icon defaults for standard action, navigation, or page-level icons when a shared style is available.

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
- **Clinic pages can ONLY update their own records** - filter queries to the clinic identifier and consultation ID
- **DO NOT allow cross-clinic access** - enforce clinic isolation at the service/controller level
- **All HConsulting CRUD operations MUST validate** that the current clinic owns the record before allowing read/write
- ✅ **Reference**: Filter HConsulting queries using clinic context from `GetCurrentUserId()` or clinic claim in JWT token
- ❌ **DO NOT allow generic HConsulting access** without clinic validation

### Roster Logic

- In roster logic for this codebase, there is only one employee per GroupID.

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
- use @ng-select/ng-select as select dropdown with searchable, ensuring simple, professional, transparent styling for dropdown controls in UI forms.
- The dialog can only be explicitly closed using the close (X) icon or cancel button.
- The entry form UI must be responsive (for mobile, tablet, and desktop devices).

### Date Format for Expenses

- Use 'dd-MMM-yyyy' date format for the expenses page and expenses dialog date inputs/displays.
- **DatePicker format must always be dd-MMM-yyyy across all components in the AestheticClinic workspace.**
- During save/POST/PUT operations, date values sent to the backend must be formatted as yyyy-mm-dd (e.g. 2026-07-15).

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

### Debtors Wrapper Debit Dropdown

- Debtors wrapper debit dropdown must filter accounts by `SUBSTRING(GroupID, 1, 3) = '123'`.

### Task Confirmation

- Always confirm and report only tasks actually completed; do not assume tasks are done.

### Legacy Variables

- Public variables from legacy projects/modules (for example accounting and staffRoster), including CoyID, should be sourced from `emrAppDefaults.json`.

### SQL Functions

- In this workspace, do not use SQL TRY_CONVERT(); use older supported built-in SQL functions instead.

### Development Environment

- User is using Microsoft SQL Server 2022 in this development environment.

---

## Legacy Crystal Reports Rendering Rules

When rendering legacy Crystal Reports from a VB.NET accounting app in a modern .NET web UI:

1. ALWAYS pass selected DISPLAY TEXT (not just IDs) from Angular dropdown selections through the full call chain: component → endpoint service → controller → proxy service → legacy Crystal controller.
2. Use the display text to build report headers (txtPrd, txtHead), not just codes/IDs. Follow VB logic: if account is not "(ALL)", use account display text; otherwise use ledger display text.
3. When converting Dapper result rows to DataSet in the legacy Crystal service, ALWAYS preserve real CLR column types (detect with Nullable.GetUnderlyingType()). Do NOT force all columns to object type, as Crystal Reports is type-sensitive and will render placeholder values like "1" if types are wrong.
4. Ensure the legacy stored procedures (getGL, etc.) return columns that match the Crystal report template field references exactly.
5. Follow the three-layer architecture: Frontend captures + passes display text → Backend controller forwards it → Proxy service includes it in query → Legacy Crystal service uses it for headers and sets correct DataSet types.
6. **Non-SELECT stored procedures (those called with `cmd.ExecuteNonQuery()` in the VB source) MUST use `DapperReportData.ExecuteNonQueryAsync()` in the Crystal Web API, NOT `ExecuteDataSetAsync()`. Calling a non-SELECT proc with `ExecuteDataSetAsync` will crash with `Column '' does not belong to table`. Check the VB form for `ExecuteNonQuery()` vs `da.Fill(ds)` to determine the correct call.**

**Reference file**: LEGACY_CRYSTAL_REPORTS_RENDERING_RULES.md in solution root.

---

## Procedures Entry Validation

- For procedures entry validation, do not require `patientId > 0`; use `PNo` from `AttendanceSummary/header` as the patient identifier for save payload and validation.

At backend, use `PNo` as the patient identifier and reject missing `PNo` with no fallback.

### Employee Info Page

**CRUD Operations**: Employee-info page CRUD operations must use Dapper with SmartHRConnection, not DefaultConnection.

### UTC DateTime Display Rule (Global)

When backend DateTime values are serialized by `UtcAwareDateTimeConverter` (UTC/`Z`), frontend UI must use the shared UTC-safe formatter pattern to prevent local timezone shifts (for example +1 hour in DST regions):

1. ✅ Use shared helpers from `src/app/shared/utils/utc-date.util.ts`:
   - `parseUtcDate`
   - `formatUtcForDisplay`
   - `formatUtcDateForDisplay`
   - `formatUtcDateDashForDisplay`
   - `formatUtcTimeForDisplay`
2. ✅ Prefer reusable pipe `src/app/pipes/utc-display.pipe.ts` in templates:
   - `{{ value | utcDisplay:'datetime' }}`
   - `{{ value | utcDisplay:'date' }}`
   - `{{ value | utcDisplay:'dateDash' }}`
   - `{{ value | utcDisplay:'time' }}`
3. ✅ Treat timezone-less DateTime strings as UTC (legacy fallback), matching server converter behavior.
4. ❌ Do not use Angular `date` pipe directly for server UTC DateTime fields where timezone shift would alter displayed business time.

### Auditrail ID Handling

- In this workspace, `Auditrail.ID` is an identity column; inserts into `Auditrail` should not provide ID values.
- For typical CRUD write operations, map `Auditrail` columns as follows:
  - `UserAction`: payload in a JSON object, (use viewModel property names as keys not model property names)
  - `ActionDate`: date only in local user/server time (`DateTime.Now.Date`)
  - `ActionTime`: full timestamp in local user/server time (`DateTime.Now`) so stored value matches user-facing local time
  - `Remarks`: type of CRUD operation, for example `deleted record with priKey/id/sno: xxxx`
  - `TranCode`: `consultID`, `BillNo`, or `pNo` depending on the page/module transaction key
  - `Src`: the page/module where the payload is coming from
  - `AuditCat`: the module name where the payload originates, for example `frontDesk` or `billing`

### Backend Dual Audit Logging Rule (Write Operations)

- For backend CRUD write operations (insert, update, delete), write to hospital DB `Auditrail` as the primary audit destination.
- Logging to hospital `AppAuditLogs` through `ApplicationDbContext` is secondary and should be treated as backup only.
- Use a boolean configuration setting to control whether secondary `AppAuditLogs` logging is enabled.
- The `Auditrail` write path must use Dapper/DataAccess wrapper with connection id `DefaultConnection`.
- `Auditrail.TranCode` must follow page/module context transaction key:
  - Attendance: `consultID`
  - Billing module pages: `billNo`
  - Patient info pages: `pNo`
  - Use the relevant context transaction key for other pages similarly.
- `Auditrail.ID` is identity; never provide `ID` in insert payload.

**ALL Dapper calls MUST go through `ISqlDataAccess`** — inject and use `LoadData`, `LoadDataText`, `SaveData`, or `SaveDataText`. This is mandatory so that `AuditedSqlDataAccess` (the registered decorator) can intercept every write and commit the audit trail.
11. ❌ **NEVER create `SqlConnection` / `IDbConnection` directly in a service** — doing so bypasses `AuditedSqlDataAccess` and silently drops the audit trail. `DepartmentService` was a historical violation of this rule and has been corrected.

### Admin Audit Report

- For the admin audit report, the grid's single source of truth must be `vwAudiTrail` using the explicit provided SELECT/view columns, with ID and UserName hidden in the UI.

### Dropdown Placeholder Rule

- For dropdowns, default selected value should be the explicit first option text (for example '--Select Account--'), with '(ALL)' if present, as the next explicit option; do not use empty-string defaults.

### Aesthetic Service Table Usage


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


### Connection Management

In CrystalReportWebAPI, connection ids like DefaultConnection are provided by the calling project/request and should not be read from this API project's Web.config. 

### Logging Configuration

In CrystalReportWebAPI, Serilog should use Web.config connection id DefaultConnection instead of SerilogConnection.

### File Management

When asked to delete duplicate files in `Models/Legacy`, avoid modifying other files unless explicitly requested.

### SPA Dialog Header Info

For SPA dialog header info, use `attendance-summary` as the single source of truth backed by `vwhRecords` fetched by `ConsultId`: in edit mode, use `ConsultId` from the selected grid row; in add/new mode, use `ConsultId` from patient dropdown; do not use fallback sources. For SPA services in edit mode, ensure `ConsultId` comes from the selected grid row's `ConsultId` column, not dropdown-derived patient context.


