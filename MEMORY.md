# AestheticClinic — MEMORY

Vibe-coding workspace. `.NET 10` + `Angular 21`, Visual Studio 2026 on Windows.
Canonical AI rules live in **`AGENTS.md`** — always read it before extending QuickApp.
This file is the **quick-recall index** so future-me doesn't re-read AGENTS.md every turn.

---

## Stack at a glance

- **Backend:** ASP.NET Core, EF Core (writes/migrations), Dapper via `IDataAccessService` (reads/reporting/cross-DB)
- **Frontend:** Angular standalone components, `inject()` not constructor DI, Angular Material, `@ng-select/ng-select`
- **Auth:** OpenIddict/OAuth2 + JWT, `[Authorize]` or inline `AuthorizeAsync()` on every endpoint
- **DBs:** EMR DB (owned entities), Accounting DB (cross-DB via Dapper, e.g. `vwAccountsInfo`)

---

## Rules I keep tripping on

### 🏥 Multi-clinic isolation (`HConsulting`)

- Each clinic has its own row in `HConsulting` (dental, aesthetics, etc.)
- Identified by `consultID` + clinic context
- **Always filter queries by clinic** — service AND controller level
- Get clinic from `GetCurrentUserId()` or the JWT clinic claim
- ❌ Never allow generic HConsulting access / cross-clinic reads or writes
- Source: AGENTS.md → "Multi-Clinic Data Isolation (HConsulting)"

### 🏦 Receipt bank-account dropdown

- Single source of truth: **`vwAccountsInfo` in Accounting DB**
- Filter by `emrAppDefaults.Acct_Banks` via **case/trim-safe `GroupId` match**
- Use `AccountNo` as the dropdown id (not `hRevenueTypes` — no fallback!)
- After filter, `AccountNo` / `AccountId` / `AccountName` must all be non-empty
- Source: AGENTS.md → "Bank Account Dropdown"

### 📋 Entry-form UI (new feature checklist)

For any new clinical/data entry module, build it like Dental Clinical Session:

1. **Listing/worklist page** — search + table + add/edit actions
2. **Separate dialog component** for new AND edit (`DentalEncounterDialogComponent` is the template)
3. **One reusable dialog** — open empty for create, prefilled for edit
4. **Header section inside the dialog** with `AttendanceSummary` (patient attendance summary)
5. **Tabs stay inside the dialog**, not the parent page
6. Angular Material only (no Bootstrap); `@ng-select/ng-select` for searchable selects
7. Material table page size = **10**
8. Dialog closes **only** via X icon or Cancel button
9. Responsive (mobile / tablet / desktop)
- Source: AGENTS.md → "Custom Rules for Entry Form UI"

### 💰 Billing debt flow

- `AttendanceService.cs` owns debt carry-forward
- `SaveDebtAsync` runs **before** `SaveChanges`; `SaveBillAsync` runs **after**
- Both called from `CreateAsync` during attendance save
- **DebtBF** must be included in running balance / debt calculations
- Private patient = `HRetainership.RetainCode = "0001"` (linked via `Patient.CoyName`), NOT `CoyType`
- Debt carry-forward applies **only** to private patients
- Source: AGENTS.md → "Patient Management"

### 🧾 Billing consultation sub-header

- When multiple `VwhConsultingDetailsForBillingAlt` rows exist per `consultId/billNo`, **iterate all** in a **compact layout** (minimal screen real estate)
- `BillNo` == `consultID`

### 📸 AttendanceSummary = receipt dialog header

- `AttendanceSummaryComponent` is the **sole source of truth** for the receipt dialog header
- Don't add an extra patient-photo lookup from `HPatients` for that flow
- Loading the photo from `HPatients` and passing it into `AttendanceSummaryComponent` IS allowed
- Source: AGENTS.md → "Reusable Components"

### 🔔 Alert banner focus

- Banners/toasts render on the **focused UI layer** (active dialog or parent page)
- Never let them render behind a modal overlay
- Source: AGENTS.md → "Alert Banner Focus Rule"

### 🔐 Auth card / external-login UI

- External-login buttons live **inside the card content** only
- Use `width:100%` / `max-width:100%` / `box-sizing:border-box`
- Keep explicit `border-style`/`border-width`/`border-color` on the card
- ❌ No negative margins / overflow-breaking positioning
- Source: AGENTS.md → "Login/Auth Card UI Rule"

### 🛡️ Authorization pattern (after journal-entries-info 403)

- **Don't use `[Authorize(Roles = "...")]` on controllers** — role strings drift (e.g. `administrator` vs `Admin` vs `Accounting`). Causes 403 / OpenIddict ID2095 "insufficient_access".
- **Use policy-based `[Authorize(Policy = AuthPolicies.XxxPolicy)]`** instead. Patterns:
  - Class-level `[Authorize(Policy = AuthPolicies.ViewXxxPolicy)]` for read endpoints (GET)
  - Per-method `[Authorize(Policy = AuthPolicies.ManageXxxPolicy)]` for write endpoints (POST/PUT/DELETE)
- **Permissions are defined in `ApplicationPermissions.cs`** under the relevant group (`Management Permissions`, `Role Permissions`, etc.) and registered in `AllPermissions`.
- **Policies are defined in `AuthPolicies.cs`** and registered in `Program.cs → AddAuthorizationBuilder()`.
- **Policy assertions should fall back** to:
  - The corresponding `ManageXxx` permission claim (Manage ⇒ can View)
  - `ManageUsers` / `ManageRoles` claims (admins always pass)
  - Role strings (`Admin`, `administrator`, `Xxx`) for legacy/seeded users
- Existing view policies: `ViewUsers`, `ManageUsers`, `ViewRoles`, `ManageRoles`, `AssignRoles`, `ViewRoleByRoleName`, `ViewAuditLogs`
- Existing mgmt policies added: `ViewAccounting`, `ManageAccounting`, `ViewEmployees`, `ManageEmployees`
- Claim type is `CustomClaims.Permission` ("permission")
- Source: this codebase — `Authorization/AuthPolicies.cs`, `Core/Services/Account/ApplicationPermissions.cs`, `Program.cs`

---

## Backend quick rules

- All entities inherit `BaseEntity` (Id, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
- All controllers inherit `BaseApiController`; return `IActionResult`, use AutoMapper, never expose entities
- Services inject `ApplicationDbContext` directly (no repository pattern)
- DTOs in `QuickApp.Server/ViewModels/{Domain}/`, `VM` suffix, DataAnnotations for validation
- ViewModels must NOT include navigation collections
- Service registration: `builder.Services.AddScoped<IXService, XService>();` in `Program.cs`
- Reference impls: `UserAccountController.cs`, `UserRoleController.cs`, `UserVMs.cs`
- New config keys → update **base `appsettings.json`** + env-specific files

## Frontend quick rules

- Standalone components only (no NgModules)
- Use `inject()`, not constructor injection
- API services extend `EndpointBase`, use `@Injectable({ providedIn: 'root' })`, getter for URL, `this.requestHeaders`
- Lazy-load everything in `app.routes.ts` via `loadComponent`, guarded by `AuthGuard`, `path:'**'` last
- New UI strings → add to **all** locale files under `quickapp.client/public/locale/` (en/fr/de/es/pt/zh/ko/ar)
- Cache rows in `rowsCache` for filter/search; show `loadingIndicator`; use `AlertService` for toasts/errors; use `fadeInOut` animation

## Data-access cheat sheet

| Scenario | Use |
|---|---|
| CRUD on owned entities, migrations, change tracking | EF Core |
| Reporting views (`VwhConsultingDetailsForBillingAlt`, etc.) | Dapper |
| Cross-DB queries (Accounting, EMR) | Dapper |
| Raw SQL that's clearer/faster than LINQ | Dapper |
| Always | Parameterized queries, never string-concat |

❌ Never mix EF change-tracking + Dapper writes on the same entity in one tx.

---

## Pointers

- Full rules → `AGENTS.md`
- Real-world patterns → `QuickApp.Server/Controllers/UserAccountController.cs`, `UserRoleController.cs`, `QuickApp.Server/ViewModels/Account/UserVMs.cs`
- Debt carry-forward impl → `QuickApp.Core/Services/.../AttendanceService.cs`
- Patient header UI → `AttendanceSummaryComponent`
- Memory dir: `memory/` (dated session notes)