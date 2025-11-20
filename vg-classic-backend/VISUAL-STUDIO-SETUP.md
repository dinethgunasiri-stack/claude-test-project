# Opening VG Classic Backend in Visual Studio

## Quick Start

1. **Open Visual Studio 2022**

2. **Open the Solution**:
   - File → Open → Project/Solution
   - Navigate to `vg-classic-backend` folder
   - Select `VGClassic.sln`

3. **Restore NuGet Packages**:
   - Right-click on Solution in Solution Explorer
   - Select "Restore NuGet Packages"
   - Wait for packages to download

4. **Set Startup Project**:
   - Right-click on `VGClassic.API` project
   - Select "Set as Startup Project"

5. **Update Connection String** (if needed):
   - Open `VGClassic.API/appsettings.json`
   - Update connection string for your SQL Server instance

## Creating the Database

### Option 1: Using Package Manager Console (Recommended)

1. **Open Package Manager Console**:
   - Tools → NuGet Package Manager → Package Manager Console

2. **Set Default Project**:
   - In the console dropdown, select `VGClassic.Infrastructure`

3. **Create Migration**:
   ```powershell
   Add-Migration InitialCreate
   ```

4. **Update Database**:
   ```powershell
   Update-Database
   ```

### Option 2: Using Command Line

1. **Open Terminal in Visual Studio**:
   - View → Terminal (or Ctrl + `)

2. **Navigate to Infrastructure project**:
   ```bash
   cd VGClassic.Infrastructure
   ```

3. **Run EF Core commands**:
   ```bash
   dotnet ef migrations add InitialCreate --startup-project ../VGClassic.API
   dotnet ef database update --startup-project ../VGClassic.API
   ```

## Adding Seed Data

1. **Open `VGClassic.API/Program.cs`**

2. **Add this code AFTER `var app = builder.Build();`**:

```csharp
// Seed database with roles and admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
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
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
```

3. **Add required using statements** at the top of `Program.cs`:

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VGClassic.Domain.Entities;
using VGClassic.Infrastructure.Identity;
using VGClassic.Infrastructure.Persistence;
```

## Running the Application

1. **Press F5** or click the green play button (IIS Express or VGClassic.API)

2. **Browser should open automatically** to:
   - `https://localhost:7001/swagger`

3. **Test the API**:
   - Try the GET /api/products endpoint
   - Login with: `admin@vgclassic.com` / `Admin@123`

## Solution Structure in Visual Studio

```
VGClassic (Solution)
├── VGClassic.Domain
│   ├── Common/
│   ├── Entities/
│   └── Enums/
├── VGClassic.Application
│   ├── Common/
│   ├── Products/
│   ├── Carts/
│   ├── Orders/
│   └── Authentication/
├── VGClassic.Infrastructure
│   ├── Identity/
│   ├── Persistence/
│   └── Services/
└── VGClassic.API (Startup Project ⭐)
    ├── Controllers/
    ├── Program.cs
    └── appsettings.json
```

## Troubleshooting in Visual Studio

### Issue: "Could not load type"
**Solution**: Clean and rebuild solution
- Build → Clean Solution
- Build → Rebuild Solution

### Issue: NuGet packages not restoring
**Solution**:
- Tools → Options → NuGet Package Manager → Clear All NuGet Cache(s)
- Right-click Solution → Restore NuGet Packages

### Issue: Database connection fails
**Solution**:
1. Make sure SQL Server is running
2. For LocalDB, check Server name in SQL Server Object Explorer
3. Update connection string in `appsettings.json`

### Issue: Migrations folder already exists
**Solution**:
- Delete the `Migrations` folder in `VGClassic.Infrastructure`
- Run `Add-Migration InitialCreate` again

### Issue: Port already in use
**Solution**:
- Right-click `VGClassic.API` → Properties
- Debug → Change the port numbers
- Or kill the process using the port

## Useful Visual Studio Shortcuts

- **F5**: Start with debugging
- **Ctrl+F5**: Start without debugging
- **Ctrl+Shift+B**: Build solution
- **Ctrl+.**: Quick actions (add using, etc.)
- **F12**: Go to definition
- **Shift+F12**: Find all references

## Next Steps

1. ✅ Open solution in Visual Studio
2. ✅ Restore NuGet packages
3. ✅ Create database migration
4. ✅ Update database
5. ✅ Add seed data
6. ✅ Run application (F5)
7. ✅ Test with Swagger
8. 📱 Create Angular frontend

## Need Help?

- Check the main `README.md` for project overview
- See `SETUP-GUIDE.md` for detailed instructions
- View Swagger at `https://localhost:7001/swagger` when running
