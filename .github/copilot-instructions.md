# QuickApp AI Development Rules

**Reference this file in your AI prompts when extending QuickApp.**

## Quick Start

When asking AI to extend QuickApp, include this in your prompt:

```
Reference the AI rules file: ai-rules/AI_RULES.md

Follow the exact patterns and conventions documented in this file.
Reference UserAccountController, UserRoleController, and UserVMs.cs for real implementation examples.
```

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

```
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
```

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

**Service Registration** (in `Program.cs`):

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

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

1. **Policy-based authorization** (attribute):

```csharp
[HttpGet("users")]
[Authorize(AuthPolicies.ViewAllUsersPolicy)]
public async Task<IActionResult> GetUsers() { ... }
```

2. **Inline authorization checks** (for resource-based authorization):

```csharp
[HttpGet("users/{id}")]
public async Task<IActionResult> GetUserById(string id)
{
    if (!(await _authorizationService.AuthorizeAsync(User, id,
        UserAccountManagementOperations.ReadOperationRequirement)).Succeeded)
        return new ChallengeResult();
    // ... rest of method
}
```

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

**Error Handling Pattern:**

```csharp
[HttpDelete("{id}")]
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
```

**Error Handling Rules:**

1. ✅ **Use custom exceptions** for domain-specific errors
2. ✅ **Log errors** using `_logger.LogError()`
3. ✅ **Return appropriate HTTP status codes**
4. ✅ **Use `AddModelError()`** from BaseApiController for validation errors
5. ❌ **DO NOT expose internal exceptions** to API responses

---

## Frontend Patterns (Angular)

### Project Structure

```
quickapp.client/src/app/
  ├── components/       # Feature components (lazy-loaded)
  ├── services/         # API services (extend EndpointBase)
  ├── models/           # TypeScript interfaces
  └── app.routes.ts     # Route configuration
```

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
4. ✅ **Use `path: '**'`\*\* for 404 route (must be last)
5. ❌ **DO NOT use eager loading** - always lazy load feature components

**Example:**

```typescript
{
  path: 'products',
  loadComponent: () => import('./components/products/products.component').then(m => m.ProductsComponent),
  canActivate: [AuthGuard],
  title: 'Products'
}
```

### Translation Files

**Important**: When adding new UI text, add translation keys to all locale files.

**Location**: `quickapp.client/public/locale/`
**Files**: `en.json`, `fr.json`, `de.json`, `es.json`, `pt.json`, `zh.json`, `ko.json`, `ar.json`

**Usage in Templates:**

```html
<h4>{{ 'Products' | translate }}</h4>
<p>{{ 'Description' | translate }}</p>
```

**Translation Rules:**

1. ✅ **Add keys to `en.json` first** (primary language)
2. ✅ **Add same keys to all other locale files** (can use English as placeholder)
3. ✅ **Use nested structure** for organization (e.g., `pageHeader.Products`, `mainMenu.Customers`)
4. ✅ **Use `translate` pipe** in templates
5. ❌ **DO NOT hardcode strings** in templates or components

### Error Handling

**Error Handling Pattern:**

```typescript
loadData(): void {
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
          MessageSeverity.error,
          error
        );
      }
    });
}
```

**Error Handling Rules:**

1. ✅ **Use `AlertService.startLoadingMessage()`** before async operations
2. ✅ **Use `AlertService.stopLoadingMessage()`** after operations complete
3. ✅ **Use `AlertService.showStickyMessage()`** for errors
4. ✅ **Use `MessageSeverity` enum** (error, warn, info, success)
5. ❌ **DO NOT show raw error objects** to users

---

## Reference Implementations

When implementing new features, reference these real implementations:

### Backend

- **UserAccountController.cs** - Authorization patterns (inline checks, policies), CRUD operations
- **UserRoleController.cs** - CRUD patterns with authorization
- **UserVMs.cs** - ViewModel validation with DataAnnotations
- **UserAccountService.cs** - Service implementation with traditional constructor
- **CustomerService.cs** - Service implementation with primary constructor

### Frontend

- Reference existing components in `quickapp.client/src/app/components/`
- Reference endpoint services in `quickapp.client/src/app/services/`

---

## Naming Conventions

### Backend

- **Entities**: PascalCase, singular (e.g., `Product`, `Customer`)
- **ViewModels**: PascalCase with `VM` suffix (e.g., `ProductVM`, `UserVM`)
- **Services**: PascalCase with `Service` suffix (e.g., `ProductService`)
- **Controllers**: PascalCase with `Controller` suffix (e.g., `ProductController`)
- **Files**: Match class name (e.g., `Product.cs`, `ProductVM.cs`)

### Frontend

- **Components**: PascalCase with `Component` suffix (e.g., `ProductsComponent`)
- **Services**: PascalCase with `Service` or `Endpoint` suffix (e.g., `ProductEndpoint`)
- **Models**: PascalCase interface (e.g., `Product`)
- **Files**: `{feature}.component.ts`, `{feature}-endpoint.service.ts`, `{feature}.model.ts`

---

## Quick Checklist

When creating a new entity with full CRUD:

**Backend:**

- [ ] Create entity in `QuickApp.Core/Models/{Domain}/{EntityName}.cs` inheriting `BaseEntity`
- [ ] Create ViewModel in `QuickApp.Server/ViewModels/{Domain}/{EntityName}VM.cs` with DataAnnotations validation
- [ ] Create interface in `QuickApp.Core/Services/{Domain}/Interfaces/I{EntityName}Service.cs`
- [ ] Create service in `QuickApp.Core/Services/{Domain}/{EntityName}Service.cs`
- [ ] Register service in `Program.cs`
- [ ] Add AutoMapper mappings in `MappingProfile.cs`
- [ ] Create controller in `QuickApp.Server/Controllers/{EntityName}Controller.cs` inheriting `BaseApiController`
- [ ] Add authorization (policies or inline checks)
- [ ] Update `ApplicationDbContext` if new DbSet needed
- [ ] Create migration: `dotnet ef migrations add Add{EntityName}`

**Frontend:**

- [ ] Create model interface in `models/{feature}.model.ts`
- [ ] Create endpoint service in `services/{feature}-endpoint.service.ts` extending `EndpointBase`
- [ ] Create component in `components/{feature}/{feature}.component.ts`
- [ ] Create template in `components/{feature}/{feature}.component.html`
- [ ] Create styles in `components/{feature}/{feature}.component.scss`
- [ ] Add route in `app.routes.ts` with `AuthGuard`
- [ ] Add navigation link in `app.component.html` nav section (with permission check if needed)
- [ ] Add permission getter in `app.component.ts` if permission-based navigation
- [ ] For admin features: Add tab in `settings.component.html` (with permission check)
- [ ] Add translation keys to `public/locale/*.json` files
- [ ] Use `AlertService` for loading states and errors
- [ ] Wrap main content in `<div [@fadeInOut]>` animation
- [ ] Import `TranslateModule` for i18n

---

## Critical Rules

### ❌ Never Do This

- Don't create new authentication mechanisms
- Don't bypass BaseApiController
- Don't access DbContext from controllers
- Don't put business logic in controllers
- Don't create new error handling patterns
- Don't skip authorization
- Don't use eager loading in Angular
- Don't make HTTP calls without EndpointBase
- Don't hardcode UI strings (use translations)

### ✅ Always Do This

- Inherit from BaseEntity for entities
- Inherit from BaseApiController for controllers
- Extend EndpointBase for API services
- Use AuthGuard on protected routes
- Use AlertService for loading/errors
- Use AutoMapper for entity ↔ ViewModel
- Use DataAnnotations for ViewModel validation
- Protect all endpoints with authorization
- Add translation keys for new UI text
- Follow naming conventions exactly

### Additional Guidelines for the frontEnd:

- Use Bash for scripting and automation tasks (e.g., build scripts, deployment scripts)
- Use Angular signals for state management
- Henceforth Use Angular material and material icons for UI consistency

---

**Remember**: QuickApp provides the foundation. AI fills in features following established patterns. Reference real implementations (UserAccountController, UserRoleController, UserVMs) for concrete examples.
