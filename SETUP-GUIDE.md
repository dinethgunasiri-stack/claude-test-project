# VG Classic - Complete Setup Guide

This guide will walk you through setting up both the backend and frontend from scratch.

## ⚠️ Important Notes

The .NET SDK is not installed in the current environment, so you'll need to run these commands on a machine with .NET 8 SDK installed.

## 📋 Prerequisites Checklist

### For Backend
- [ ] .NET 8 SDK installed (`dotnet --version` should show 8.x)
- [ ] SQL Server (LocalDB or Express) installed
- [ ] Visual Studio 2022 OR VS Code with C# extension

### For Frontend
- [ ] Node.js 18+ installed (`node --version`)
- [ ] npm installed (`npm --version`)
- [ ] Angular CLI installed globally (`ng --version`)

## 🔧 Step-by-Step Setup

### Part 1: Backend Setup (30-45 minutes)

#### Step 1: Verify .NET Installation

```bash
dotnet --version
# Should output: 8.x.x
```

If not installed, download from: https://dotnet.microsoft.com/download/dotnet/8.0

#### Step 2: Navigate and Restore Packages

```bash
cd vg-classic-backend
dotnet restore
```

#### Step 3: Build the Solution

```bash
dotnet build
```

#### Step 4: Update Connection String

Open `vg-classic-backend/VGClassic.API/appsettings.json` and update if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=VGClassicDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

**For SQL Server Express:**
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=VGClassicDb;Trusted_Connection=true;MultipleActiveResultSets=true"
```

#### Step 5: Install EF Core Tools

```bash
dotnet tool install --global dotnet-ef
# Or update if already installed
dotnet tool update --global dotnet-ef
```

#### Step 6: Create Database Migration

```bash
cd VGClassic.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../VGClassic.API --context ApplicationDbContext
```

#### Step 7: Apply Migration to Database

```bash
dotnet ef database update --startup-project ../VGClassic.API
```

This creates the database and all tables.

#### Step 8: Add Seed Data Code

Open `vg-classic-backend/VGClassic.API/Program.cs` and add this code AFTER `var app = builder.Build();`:

```csharp
// Seed database with roles and admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Seed roles
    string[] roles = { "Customer", "Admin", "SuperAdmin" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed admin user
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

    // Seed categories
    if (!context.Categories.Any())
    {
        var categories = new[]
        {
            new Category { Name = "Men's Clothing", Slug = "mens-clothing", Description = "Stylish clothing for men", IsActive = true, DisplayOrder = 1, CreatedDate = DateTime.UtcNow },
            new Category { Name = "Women's Clothing", Slug = "womens-clothing", Description = "Elegant clothing for women", IsActive = true, DisplayOrder = 2, CreatedDate = DateTime.UtcNow },
            new Category { Name = "Accessories", Slug = "accessories", Description = "Fashion accessories", IsActive = true, DisplayOrder = 3, CreatedDate = DateTime.UtcNow }
        };
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();
    }

    // Seed sample products
    if (!context.Products.Any())
    {
        var mensCategory = context.Categories.First(c => c.Slug == "mens-clothing");
        var womensCategory = context.Categories.First(c => c.Slug == "womens-clothing");

        var products = new[]
        {
            new Product
            {
                Name = "Classic White T-Shirt",
                Description = "Premium cotton t-shirt with modern fit",
                DetailedDescription = "This premium t-shirt is crafted from 100% organic cotton with a modern, comfortable fit. Perfect for any casual occasion.",
                Price = 29.99m,
                CompareAtPrice = 39.99m,
                CategoryId = mensCategory.Id,
                Brand = "VG Classic",
                SKU = "VG-TS-001",
                StockQuantity = 100,
                IsActive = true,
                IsFeatured = true,
                PublishedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { Url = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab", Alt = "White T-Shirt", DisplayOrder = 0, IsPrimary = true, CreatedDate = DateTime.UtcNow }
                },
                Variants = new List<ProductVariant>
                {
                    new ProductVariant { Size = "S", Color = "White", ColorHex = "#FFFFFF", SKU = "VG-TS-001-S-WHT", StockQuantity = 25, IsActive = true, CreatedDate = DateTime.UtcNow },
                    new ProductVariant { Size = "M", Color = "White", ColorHex = "#FFFFFF", SKU = "VG-TS-001-M-WHT", StockQuantity = 30, IsActive = true, CreatedDate = DateTime.UtcNow },
                    new ProductVariant { Size = "L", Color = "White", ColorHex = "#FFFFFF", SKU = "VG-TS-001-L-WHT", StockQuantity = 25, IsActive = true, CreatedDate = DateTime.UtcNow },
                    new ProductVariant { Size = "XL", Color = "White", ColorHex = "#FFFFFF", SKU = "VG-TS-001-XL-WHT", StockQuantity = 20, IsActive = true, CreatedDate = DateTime.UtcNow }
                }
            },
            new Product
            {
                Name = "Elegant Black Dress",
                Description = "Sophisticated evening dress",
                DetailedDescription = "An elegant black dress perfect for any formal occasion. Features a flattering silhouette and premium fabric.",
                Price = 89.99m,
                CompareAtPrice = 129.99m,
                CategoryId = womensCategory.Id,
                Brand = "VG Classic",
                SKU = "VG-DR-001",
                StockQuantity = 50,
                IsActive = true,
                IsFeatured = true,
                PublishedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { Url = "https://images.unsplash.com/photo-1595777457583-95e059d581b8", Alt = "Black Dress", DisplayOrder = 0, IsPrimary = true, CreatedDate = DateTime.UtcNow }
                },
                Variants = new List<ProductVariant>
                {
                    new ProductVariant { Size = "S", Color = "Black", ColorHex = "#000000", SKU = "VG-DR-001-S-BLK", StockQuantity = 15, IsActive = true, CreatedDate = DateTime.UtcNow },
                    new ProductVariant { Size = "M", Color = "Black", ColorHex = "#000000", SKU = "VG-DR-001-M-BLK", StockQuantity = 20, IsActive = true, CreatedDate = DateTime.UtcNow },
                    new ProductVariant { Size = "L", Color = "Black", ColorHex = "#000000", SKU = "VG-DR-001-L-BLK", StockQuantity = 15, IsActive = true, CreatedDate = DateTime.UtcNow }
                }
            }
        };
        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}
```

Don't forget to add the required using statements at the top:

```csharp
using Microsoft.AspNetCore.Identity;
using VGClassic.Infrastructure.Identity;
using VGClassic.Infrastructure.Persistence;
using VGClassic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
```

#### Step 9: Run the Backend

```bash
cd VGClassic.API
dotnet run
```

✅ **Backend should now be running at:**
- API: https://localhost:7001
- Swagger: https://localhost:7001/swagger

#### Step 10: Test the API

Visit `https://localhost:7001/swagger` and test:
1. **Register** a new user
2. **Login** with credentials
3. **Get products** (should see seeded products)
4. **Test admin login** with `admin@vgclassic.com` / `Admin@123`

---

### Part 2: Frontend Setup (20-30 minutes)

#### Step 1: Install Angular CLI

```bash
npm install -g @angular/cli
```

#### Step 2: Create Angular App

```bash
# From the project root directory
ng new vg-classic-frontend --routing --style=scss --skip-git
cd vg-classic-frontend
```

#### Step 3: Install Dependencies

```bash
npm install bootstrap @popperjs/core
npm install @auth0/angular-jwt
```

#### Step 4: Configure Bootstrap

Update `angular.json` - find the `"styles"` array and update it:

```json
"styles": [
  "node_modules/bootstrap/dist/css/bootstrap.min.css",
  "src/styles.scss"
],
"scripts": [
  "node_modules/@popperjs/core/dist/umd/popper.min.js",
  "node_modules/bootstrap/dist/js/bootstrap.min.js"
]
```

#### Step 5: Create Environment Configuration

Create/update `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7001/api'
};
```

Create/update `src/environments/environment.prod.ts`:

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://your-production-api.com/api'
};
```

#### Step 6: Generate Core Structure

```bash
# Generate core module
ng generate module core
ng generate service core/services/auth
ng generate service core/services/product
ng generate service core/services/cart
ng generate guard core/guards/auth
ng generate guard core/guards/admin
ng generate interceptor core/interceptors/auth

# Generate shared module
ng generate module shared
ng generate component shared/components/header
ng generate component shared/components/footer
ng generate component shared/components/product-card

# Generate feature modules
ng generate module features/auth --routing
ng generate component features/auth/login
ng generate component features/auth/register

ng generate module features/shop --routing
ng generate component features/shop/product-list
ng generate component features/shop/product-detail

ng generate module features/cart --routing
ng generate component features/cart/cart-view

ng generate module features/admin --routing
ng generate component features/admin/dashboard
ng generate component features/admin/product-management
```

#### Step 7: Run the Frontend

```bash
ng serve
```

✅ **Frontend should now be running at:**
- App: http://localhost:4200

---

## 🧪 Testing the Complete System

### Test Flow:

1. **Start Backend** (`dotnet run` from VGClassic.API)
2. **Start Frontend** (`ng serve` from vg-classic-frontend)
3. **Open Browser**: http://localhost:4200
4. **Register** a new user account
5. **Login** with your credentials
6. **Browse Products** - see the seeded products
7. **Add to Cart** - add items to cart
8. **View Cart** - see cart items
9. **Checkout** - place an order
10. **Admin Login** - use admin credentials to access admin panel
11. **Manage Products** - create/edit/delete products as admin

## 📝 Common Issues & Solutions

### Backend Issues

**Issue**: Can't connect to SQL Server
- **Solution**: Make sure SQL Server is running. For LocalDB: `sqllocaldb start mssqllocaldb`

**Issue**: Migration fails
- **Solution**: Delete Migrations folder, drop database, and recreate migration

**Issue**: Port already in use
- **Solution**: Change port in `launchSettings.json` or kill the process using the port

### Frontend Issues

**Issue**: CORS error
- **Solution**: Make sure backend CORS is configured for `http://localhost:4200`

**Issue**: API calls failing
- **Solution**: Verify `environment.ts` has correct API URL

**Issue**: Bootstrap not working
- **Solution**: Restart `ng serve` after updating `angular.json`

## 🎯 Next Steps After Setup

1. **Customize Design** - Update styles for futuristic look
2. **Add More Products** - Use admin panel or seed more data
3. **Implement Features** - Refer to the main README for feature list
4. **Deploy** - Follow deployment guides for production

## 📞 Need Help?

- Check the main `README.md` for detailed documentation
- Review backend `vg-classic-backend/README.md` for API details
- Check Swagger documentation at https://localhost:7001/swagger

---

**Happy Coding! 🚀**
