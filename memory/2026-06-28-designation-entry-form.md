# Designation entry form — Employees module

Built full CRUD entry form for designations, mirroring the legacy VB.NET `genIDNo()` generator
and the QuickApp employee-info pattern.

## Backend

- `AestheticEMR.Core/Services/Employees/Interfaces/IDesignationService.cs` — interface
- `AestheticEMR.Core/Services/Employees/DesignationService.cs` — implementation
- `AestheticEMR.Server/Controllers/DesignationController.cs` — `api/designation`, all `[Authorize]`
- `AestheticEMR.Server/ViewModels/Employees/EmployeeVMs.cs` — extended `DesignationVM` with
  validation + `InUseCount` (lookup-only, ignored on create/update payloads)
- `AestheticEMR.Server/Configuration/MappingProfile.cs` — added `DesignationVM ↔ Designation` reverse map
- `AestheticEMR.Server/Program.cs` — registered `IDesignationService`

## ID generation (mirrors VB.NET genIDNo)

- Uses the shared `IDgen` table with `DestName = "Designation"` (same pattern as Employee)
- 2-digit zero-padded format (`"01"`, `"10"`, `"99"`)
- Atomic increment inside a transaction; rollback on collision
- Capped at 99 — instead of silent truncation like the VB version, throws `InvalidOperationException`
  which the controller surfaces as `400 Bad Request`

## Delete safety

- Refuses to delete a designation still referenced by `HrEmployees.Designation` — service
  throws `InvalidOperationException`, controller returns `409 Conflict`
- `IsInUseAsync` (single-row check) + `GetInUseCountsAsync` (bulk grouped counts for the list)
- Frontend also disables the delete button when `inUseCount > 0` (defence in depth)

## Frontend

- `AestheticEMR.client/src/app/services/designation-endpoint.service.ts` — `DesignationEndpoint`
- `AestheticEMR.client/src/app/models/employee.model.ts` — added `inUseCount?` to `Designation`
- `AestheticEMR.client/src/app/features/employees/designation/designation.component.ts` —
  `DesignationComponent` (list) + `DesignationDialogComponent` (new/edit, one reusable dialog)
- `AestheticEMR.client/src/app/features/employees/employees.routes.ts` — added `designation` route
- i18n: added `designations` block to all 8 locale files (en/fr/de/es/pt/zh/ko/ar), English copy
- `AestheticEMR.client/public/assets/navigation.json` — added Designation subItem under Employees
  (`workspace_premium` icon, path `designation`). Sidebar template reads this JSON at runtime
  via `MainLayoutComponent.ngOnInit()`, so no template/scss changes were needed.

## UI features

- List page: search by id/name, Material table, page size 10, Add button
- Dialog: auto-gen id (read-only), required name field, X icon + Cancel button to close
- "In Use" badge (orange pill with employee count) on rows currently referenced
- Delete button auto-disabled when in use, with tooltip explanation
- Responsive (mobile/tablet/desktop), fadeInOut animation, AlertService toasts

## Files NOT touched

- `Designation.cs` entity — already correct, uses legacy lowercase columns `desID`/`desName`
- `ApplicationDbContext` mapping for `Designation` — already correct (PK = `desID`, table = `Designation`)
- `EmployeeVM` — no breaking changes; existing `DesignationId`/`DesignationName` fields preserved
- `EmployeeController.designations` endpoint — still works, returns same shape (plus `inUseCount` is `null`)

## Build status

✅ `dotnet build` → 0 errors, 39 pre-existing warnings (none from new code)
✅ `ng build --configuration=development` → exit 0, only pre-existing login/change-password warnings
✅ All 8 locale JSON files parse cleanly

## Save error — debugging session

**Symptom:** "error saving new record" persisted across rebuilds.

**Root causes identified:**
1. **Stale build** — Visual Studio and an old `AestheticEMR.Server.exe` (pid 9400) were
   holding the project DLLs, so `dotnet build` could not overwrite them. User was testing
   against the previous in-memory build that lacked our fixes.
2. **`required string DesignationId`** on the VM — when the frontend's auto-generated id
   hadn't arrived yet (race), the POST body was missing the field, so ASP.NET Core's
   required-modifier validation rejected the request with a deserialization error.
3. **`DbUpdateException` unhandled** — the controller only caught `InvalidOperationException`,
   so any SQL-level error (unique constraint, FK violation) escaped to a generic 500 with no
   actionable message.
4. **IDgen seed collision** — first-ever designation would start at id=1, colliding with any
   legacy `Designation` row already in the table (seed data, VB.NET imports).

**Fixes shipped:**
- Killed old AestheticEMR.Server process before rebuilding.
- `DesignationVM.DesignationId` → `string?` (optional on POST, server is source of truth).
- Constructor in `DesignationDialogComponent` now tracks `loadingId`; Save button disables
  itself and shows "Preparing..." until the id arrives. `save()` also defends against the
  race with explicit warnings.
- Controller `Create` and `Update` now catch `DbUpdateException` and surface the inner
  exception message as a 400 (instead of 500).
- `DesignationService.CreateAsync` seeds the IDgen counter from `max(existing.desID) + 1`
  when no IDgen row exists, so legacy rows never collide.
- Dropped `context.HrIdgens.Update(idgen)` — the entity is already tracked from the query,
  so reattaching as Modified was redundant (and would break if it weren't tracked).
- Always overwrite client-supplied id in the service (`designation.desID = nextId.ToString(...)`)
  as defence in depth.