# VG Classic E-Commerce Backend

A modern e-commerce backend built with .NET 8, Clean Architecture, CQRS with MediatR, and Entity Framework Core.

## Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
├── VGClassic.Domain       # Enterprise business rules and entities
├── VGClassic.Application  # Application business rules (CQRS, MediatR)
├── VGClassic.Infrastructure # External concerns (EF Core, Identity, JWT)
└── VGClassic.API           # Web API controllers and configuration
```

## Features

✅ **Clean Architecture** with dependency inversion
✅ **CQRS Pattern** with MediatR for command/query separation
✅ **Entity Framework Core** with Code-First approach
✅ **JWT Authentication** with role-based authorization (Admin/Customer)
✅ **ASP.NET Core Identity** for user management
✅ **FluentValidation** for request validation
✅ **SQL Server** database
✅ **Swagger/OpenAPI** documentation

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code with C# extension

## Getting Started

### 1. Restore Dependencies

```bash
cd vg-classic-backend
dotnet restore
```

### 2. Update Database Connection

Edit `VGClassic.API/appsettings.json` and update the connection string if needed:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=VGClassicDb;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

### 3. Create Database

Run migrations to create the database:

```bash
cd VGClassic.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../VGClassic.API
dotnet ef database update --startup-project ../VGClassic.API
```

### 4. Seed Initial Data

You'll need to create roles and an admin user. Add this code to `Program.cs` after `var app = builder.Build();`:

```csharp
// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Create roles
    string[] roles = { "Customer", "Admin", "SuperAdmin" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create admin user
    var adminEmail = "admin@vgclassic.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User",
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser, "Admin@123");
        await userManager.AddToRoleAsync(adminUser, "SuperAdmin");
    }
}
```

### 5. Run the API

```bash
cd VGClassic.API
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5001`
- Swagger UI: `https://localhost:7001/swagger`

## API Endpoints

### Authentication
- `POST /api/authentication/register` - Register new user
- `POST /api/authentication/login` - Login user

### Products (Public)
- `GET /api/products` - Get all products (with filtering, pagination)
- `GET /api/products/{id}` - Get product by ID

### Products (Admin Only)
- `POST /api/products` - Create product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

### Cart (Authenticated)
- `GET /api/carts` - Get user's cart
- `POST /api/carts/items` - Add item to cart
- `DELETE /api/carts/items/{itemId}` - Remove item from cart

### Orders (Authenticated)
- `GET /api/orders` - Get user's orders
- `POST /api/orders` - Create order from cart

## Database Schema

### Main Tables
- **Products** - Product catalog
- **ProductImages** - Product images
- **ProductVariants** - Size/color variations
- **Categories** - Product categories
- **Carts** - User shopping carts
- **CartItems** - Items in carts
- **Orders** - Customer orders
- **OrderItems** - Order line items
- **Payments** - Payment information
- **Reviews** - Product reviews
- **Addresses** - User addresses

## Testing with Swagger

1. Navigate to `https://localhost:7001/swagger`
2. Register a user or login with admin credentials:
   - Email: `admin@vgclassic.com`
   - Password: `Admin@123`
3. Copy the access token from the response
4. Click "Authorize" button in Swagger UI
5. Enter: `Bearer {your-token}`
6. Test the endpoints!

## Project Structure

### Domain Layer
- **Entities**: Core business entities (Product, Order, Cart, etc.)
- **Enums**: Business enumerations (OrderStatus, PaymentStatus, etc.)
- **Common**: Base classes and interfaces

### Application Layer
- **Commands**: Write operations (Create, Update, Delete)
- **Queries**: Read operations (Get, List)
- **DTOs**: Data transfer objects
- **Validators**: FluentValidation validators
- **Behaviours**: MediatR pipeline behaviors (Validation, Logging)

### Infrastructure Layer
- **Persistence**: EF Core DbContext and configurations
- **Identity**: ASP.NET Identity and JWT implementation
- **Services**: Infrastructure services

### API Layer
- **Controllers**: RESTful API controllers
- **Program.cs**: Application configuration

## Technologies Used

- **.NET 8** - Framework
- **Entity Framework Core 8** - ORM
- **MediatR** - CQRS implementation
- **FluentValidation** - Validation
- **ASP.NET Core Identity** - Authentication
- **JWT Bearer** - Authorization
- **Swashbuckle** - API documentation
- **SQL Server** - Database

## Design Patterns

- **Clean Architecture** - Separation of concerns
- **CQRS** - Command Query Responsibility Segregation
- **Repository Pattern** - Data access abstraction
- **Mediator Pattern** - Decoupled request handling
- **Pipeline Behavior** - Cross-cutting concerns

## Next Steps

1. **Add Categories**: Create category management endpoints
2. **Seed Products**: Add sample products with images
3. **Payment Integration**: Integrate Stripe/PayPal
4. **Email Service**: Add email notifications
5. **File Upload**: Implement image upload
6. **Admin Dashboard**: Additional admin endpoints

## Contributing

This is a project for VG Classic clothing brand. Contact the team for contribution guidelines.

## License

Proprietary - VG Classic © 2024
