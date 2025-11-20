# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

VG Classic is a full-stack e-commerce platform with:
- **Backend**: .NET 8 Web API (`vg-classic-backend/`)
- **Frontend**: Angular 17 SPA (`vg-classic-frontend/`)

Both applications are fully functional and build with 0 errors.

## Build Commands

### Backend (.NET 8 API)
```bash
# Build entire solution
cd vg-classic-backend
dotnet build

# Run API (starts on https://localhost:7001)
cd VGClassic.API
dotnet run

# Create database migration
cd VGClassic.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../VGClassic.API

# Apply migrations
dotnet ef database update --startup-project ../VGClassic.API

# Run from specific project
cd VGClassic.API && dotnet run
```

### Frontend (Angular 17)
```bash
# Install dependencies
cd vg-classic-frontend
npm install

# Development server (http://localhost:4200)
npm start
# or
ng serve

# Production build
npm run build
# Output: dist/vg-classic-frontend/

# Clean Angular cache
ng cache clean
```

## Architecture

### Backend: Clean Architecture with CQRS

**Layer Dependencies** (dependency inversion):
```
API → Infrastructure → Application → Domain
```

**Key Architectural Decisions**:
1. **CQRS with MediatR**: All business logic goes through commands (write) and queries (read)
   - Commands/Queries in: `VGClassic.Application/{Feature}/Commands|Queries/`
   - Handlers implement: `IRequestHandler<TRequest, TResponse>`
   - Pipeline behaviors (validation, logging) automatically applied

2. **Dependency Injection Setup**:
   - Each layer has `DependencyInjection.cs` with extension method
   - `Program.cs` calls: `AddApplication()` and `AddInfrastructure(configuration)`
   - Application layer registers MediatR + FluentValidation
   - Infrastructure layer registers DbContext + Identity + JWT

3. **Entity Framework Core**:
   - Code-First approach
   - DbContext: `ApplicationDbContext` in Infrastructure
   - Entity configurations: `VGClassic.Infrastructure/Persistence/Configurations/`
   - Interface abstraction: `IApplicationDbContext` (no direct EF dependencies in Application)

4. **Authentication Flow**:
   - ASP.NET Core Identity for user management
   - JWT tokens generated via `IJwtTokenGenerator` service
   - Token settings in `appsettings.json` under `JwtSettings`
   - Auth interceptor attaches `Authorization: Bearer {token}` header
   - Password requirements: 8+ chars, uppercase, lowercase, digit, special char

5. **Validation Strategy**:
   - FluentValidation validators in Application layer
   - `ValidationBehaviour<TRequest, TResponse>` intercepts all MediatR requests
   - Validators automatically discovered and executed before handlers
   - Throws `ValidationException` with error details if validation fails

### Frontend: Angular with Lazy Loading

**Module Structure**:
```
app/
├── core/                  # Singleton services, guards, interceptors
│   ├── guards/           # AuthGuard (authenticated), AdminGuard (admin role)
│   ├── interceptors/     # AuthInterceptor (adds JWT to requests)
│   ├── services/         # ApiService, AuthService, CartService
│   └── models/           # TypeScript interfaces
├── features/             # Lazy-loaded feature modules
│   ├── auth/            # Login, Register
│   ├── products/        # Product list, detail
│   ├── cart/            # Shopping cart
│   ├── checkout/        # Order placement
│   └── admin/           # Admin dashboard
└── shared/              # Shared components (Header, Footer, Loading)
```

**Key Architectural Decisions**:
1. **Lazy Loading**: All feature modules loaded on-demand via `app-routing.module.ts`
   - Products: Public, no guard
   - Cart/Checkout: `AuthGuard` required
   - Admin: `AuthGuard` + `AdminGuard` required

2. **State Management**:
   - Auth state: `AuthService.currentUser$` (BehaviorSubject)
   - Cart state: `CartService.cart$` (BehaviorSubject)
   - Token storage: localStorage (`vg_token`, `vg_user`)

3. **HTTP Communication**:
   - `ApiService`: Generic wrapper for HttpClient
   - Base URL: `environment.apiUrl` (`https://localhost:7001/api`)
   - All requests automatically include JWT via `AuthInterceptor`
   - 401 responses trigger automatic logout

4. **Form Strategy**:
   - Reactive Forms (`FormBuilder`, `FormGroup`)
   - Validators: Built-in + custom (e.g., password match)
   - Error display: Conditional with `submitted` flag

5. **Routing Guards**:
   - `AuthGuard`: Checks `authService.isAuthenticated`, redirects to `/auth/login` with `returnUrl`
   - `AdminGuard`: Checks `authService.isAdmin`, redirects to `/products`
   - Applied in route configuration: `canActivate: [AuthGuard, AdminGuard]`

## Critical Configuration

### Backend Connection String
Location: `vg-classic-backend/VGClassic.API/appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=VGClassicDb;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

### Frontend API URL
Location: `vg-classic-frontend/src/environments/environment.ts`
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7001/api'
};
```

### CORS Configuration
Location: `vg-classic-backend/VGClassic.API/Program.cs`
- Policy name: `AllowAngularApp`
- Allowed origins: `http://localhost:4200`, `http://localhost:4201`
- Already configured - frontend and backend can communicate

## Common Development Workflows

### Adding a New Feature (Backend)

1. **Domain Entity** (if needed): `VGClassic.Domain/Entities/EntityName.cs`
2. **Application Layer**:
   - Create folder: `VGClassic.Application/{FeatureName}/`
   - Add command: `Commands/{CommandName}/{CommandName}Command.cs`
   - Add handler: `Commands/{CommandName}/{CommandName}CommandHandler.cs`
   - Add validator: `Commands/{CommandName}/{CommandName}CommandValidator.cs`
   - Add DTO: `Common/Models/{FeatureName}Dto.cs`
3. **Infrastructure** (if DB changes): Add configuration in `Persistence/Configurations/`
4. **API Controller**: `VGClassic.API/Controllers/{FeatureName}Controller.cs`
   - Inject `IMediator`
   - Send commands: `await _mediator.Send(new CommandName(...))`
5. **Migration**: `cd VGClassic.Infrastructure && dotnet ef migrations add FeatureName --startup-project ../VGClassic.API`

### Adding a New Feature (Frontend)

1. **Create Module**: `ng generate module features/feature-name --routing`
2. **Create Component**: `ng generate component features/feature-name/component-name`
3. **Add Model**: Create interface in `core/models/feature.model.ts`
4. **Update Service**: Add methods to `ApiService` or create new service
5. **Configure Routing**:
   - Add route in feature routing module
   - Add lazy route in `app-routing.module.ts`
6. **Apply Guards**: Add `canActivate: [AuthGuard]` if protected route

### Testing the Full Stack

1. Start backend: `cd vg-classic-backend/VGClassic.API && dotnet run`
2. Verify Swagger: Open `https://localhost:7001/swagger`
3. Start frontend: `cd vg-classic-frontend && npm start`
4. Open browser: `http://localhost:4200`
5. Test flow:
   - Register user at `/auth/register`
   - Login at `/auth/login`
   - Browse products at `/products`
   - Add to cart (requires authentication)
   - Checkout at `/checkout`

### Seeding Initial Data

Add to `Program.cs` after `var app = builder.Build();`:
```csharp
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Create roles
    string[] roles = { "Customer", "Admin", "SuperAdmin" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Create admin
    var adminEmail = "admin@vgclassic.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User",
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(admin, "Admin@123");
        await userManager.AddToRoleAsync(admin, "SuperAdmin");
    }
}
```

## Important Implementation Patterns

### Backend: Command/Query Pattern
```csharp
// Command (write operation)
public record CreateProductCommand : IRequest<Result<int>>
{
    public string Name { get; init; }
    // ... other properties
}

// Handler
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Business logic
        await _context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(entity.Id);
    }
}
```

### Frontend: Service with Observables
```typescript
export class FeatureService {
  private stateSubject: BehaviorSubject<State | null>;
  public state$: Observable<State | null>;

  constructor(private apiService: ApiService) {
    this.stateSubject = new BehaviorSubject<State | null>(null);
    this.state$ = this.stateSubject.asObservable();
  }

  loadData(): Observable<ApiResponse<State>> {
    return this.apiService.get<ApiResponse<State>>('endpoint')
      .pipe(tap(response => {
        if (response.isSuccess && response.value) {
          this.stateSubject.next(response.value);
        }
      }));
  }
}
```

### Result Pattern (Backend)
All operations return `Result<T>` or `Result`:
```csharp
// Success
return Result<T>.Success(value);

// Failure
return Result<T>.Failure("Error message");
return Result<T>.Failure(new[] { "Error 1", "Error 2" });
```

Frontend handles:
```typescript
response.isSuccess // boolean
response.value     // T or undefined
response.errors    // string[] or undefined
```

## Database Schema Notes

**Key Relationships**:
- `Product` → `ProductImages` (one-to-many)
- `Product` → `ProductVariants` (one-to-many, for size/color options)
- `Product` → `Category` (many-to-one)
- `Cart` → `CartItems` → `Product` (shopping cart structure)
- `Order` → `OrderItems` → `Product` (order history)
- `ApplicationUser` → `Cart` (one-to-one)
- `ApplicationUser` → `Orders` (one-to-many)

**Audit Fields** (all entities inherit from `BaseEntity`):
- `Id`, `CreatedDate`, `CreatedBy`, `UpdatedDate`, `UpdatedBy`

## Design System (Frontend)

**Futuristic Theme**:
- Primary color: `#00f0ff` (cyan) - neon glow effects
- Secondary: `#ff00ff` (magenta) - accents
- Background: Dark gradients (`#0a0e27`, `#050814`)
- Fonts: Orbitron (headers), Rajdhani (body)
- Effects: Glass morphism, neon glow, smooth animations

**Component Classes**:
- `.neon-text` - Cyan glowing text
- `.text-gradient` - Gradient text effect
- `.glass-effect` - Glass morphism with backdrop blur
- `.card` - Styled with borders and shadow effects
- `.btn-primary` - Cyan button with glow on hover

## Troubleshooting

**Backend won't build**:
- Check all project references in solution
- Restore NuGet packages: `dotnet restore`
- Verify .NET 8 SDK installed: `dotnet --version`

**Frontend won't build**:
- Delete `node_modules` and reinstall: `rm -rf node_modules && npm install`
- Clear Angular cache: `ng cache clean`
- Check TypeScript version compatibility

**CORS errors**:
- Verify backend `Program.cs` has `AllowAngularApp` policy configured
- Check frontend origin matches allowed origins
- Ensure `app.UseCors("AllowAngularApp")` is before `app.UseAuthorization()`

**401 Unauthorized**:
- Check JWT token in browser DevTools → Application → Local Storage
- Verify token format in Network tab (should be `Bearer {token}`)
- Check token expiration (default 1 hour)
- Verify JWT settings match between `appsettings.json` and token generation

**Database migration issues**:
- Always specify `--startup-project ../VGClassic.API` from Infrastructure folder
- Check connection string in `appsettings.json`
- Verify SQL Server is running
- Use `dotnet ef database drop` to reset (WARNING: deletes all data)
