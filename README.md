# VG Classic E-Commerce Platform

A modern, full-stack e-commerce platform for VG Classic clothing brand featuring a C# .NET Core backend with Clean Architecture and an Angular frontend with Bootstrap.

## 🚀 Project Overview

**VG Classic** is a comprehensive e-commerce solution designed for selling clothing with a futuristic, modern design. The platform includes user authentication, shopping cart functionality, order management, and a separate admin panel for managing products and orders.

## 📁 Project Structure

```
/
├── vg-classic-backend/    # .NET Core 8 Backend (Clean Architecture)
│   ├── VGClassic.Domain/         # Entities, Enums, Business Rules
│   ├── VGClassic.Application/    # CQRS, MediatR, Use Cases
│   ├── VGClassic.Infrastructure/ # EF Core, Identity, Services
│   └── VGClassic.API/            # Web API, Controllers
│
└── vg-classic-frontend/   # Angular 17+ Frontend
    ├── src/app/
    │   ├── core/          # Auth, Guards, Interceptors
    │   ├── shared/        # Shared Components, Pipes
    │   ├── features/      # Feature Modules
    │   │   ├── auth/      # Login, Register
    │   │   ├── shop/      # Product Listing, Details
    │   │   ├── cart/      # Shopping Cart
    │   │   ├── checkout/  # Checkout Flow
    │   │   └── admin/     # Admin Panel
    │   └── layouts/       # Main & Admin Layouts
    └── ...
```

## ✨ Features

### Backend Features
- ✅ Clean Architecture with CQRS pattern
- ✅ MediatR for command/query handling
- ✅ Entity Framework Core Code-First approach
- ✅ JWT-based authentication
- ✅ Role-based authorization (Admin/Customer)
- ✅ SQL Server database
- ✅ FluentValidation for request validation
- ✅ Swagger/OpenAPI documentation

### Frontend Features
- ✅ Angular with Reactive Forms
- ✅ Bootstrap for responsive design
- ✅ JWT authentication with token refresh
- ✅ Route guards for auth/admin protection
- ✅ HTTP interceptors for auth tokens
- ✅ Shopping cart with persistence
- ✅ Product browsing with filters
- ✅ Checkout process
- ✅ Admin panel for product/order management
- ✅ User profile and order history

### Key Functionalities
- 🛍️ **Product Catalog**: Browse products with filtering and search
- 🛒 **Shopping Cart**: Add/remove items, persistent cart
- 💳 **Checkout**: Multi-step checkout with shipping info
- 👤 **User Authentication**: Register, login, profile management
- 📦 **Order Management**: View order history and status
- 👨‍💼 **Admin Panel**: Manage products, orders, and customers
- 📱 **Responsive Design**: Mobile-first Bootstrap UI

## 🛠️ Technology Stack

### Backend
- **.NET 8** - Application framework
- **C#** - Programming language
- **Entity Framework Core** - ORM for database access
- **SQL Server** - Relational database
- **ASP.NET Core Identity** - User management
- **JWT Bearer** - Token-based authentication
- **MediatR** - Mediator pattern implementation
- **FluentValidation** - Input validation
- **Swashbuckle** - API documentation

### Frontend
- **Angular 17+** - Frontend framework
- **TypeScript** - Programming language
- **Bootstrap 5** - CSS framework
- **RxJS** - Reactive programming
- **Angular Reactive Forms** - Form handling
- **HttpClient** - API communication

## 🚦 Getting Started

### Prerequisites

#### Backend
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or LocalDB)
- Visual Studio 2022 or VS Code with C# extension

#### Frontend
- [Node.js](https://nodejs.org/) (v18 or later)
- [Angular CLI](https://angular.io/cli): `npm install -g @angular/cli`

### Backend Setup

1. **Navigate to backend directory**:
   ```bash
   cd vg-classic-backend
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Update database connection string** in `VGClassic.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=VGClassicDb;Trusted_Connection=true;MultipleActiveResultSets=true"
   }
   ```

4. **Create database**:
   ```bash
   cd VGClassic.Infrastructure
   dotnet ef migrations add InitialCreate --startup-project ../VGClassic.API
   dotnet ef database update --startup-project ../VGClassic.API
   ```

5. **Run the API**:
   ```bash
   cd ../VGClassic.API
   dotnet run
   ```

   The API will be available at:
   - API: `https://localhost:7001`
   - Swagger: `https://localhost:7001/swagger`

### Frontend Setup

1. **Create Angular application**:
   ```bash
   ng new vg-classic-frontend --routing --style=scss
   cd vg-classic-frontend
   ```

2. **Install dependencies**:
   ```bash
   npm install bootstrap @ng-bootstrap/ng-bootstrap
   npm install @auth0/angular-jwt
   ```

3. **Update `angular.json`** to include Bootstrap:
   ```json
   "styles": [
     "node_modules/bootstrap/dist/css/bootstrap.min.css",
     "src/styles.scss"
   ]
   ```

4. **Update `src/environments/environment.ts`**:
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'https://localhost:7001/api'
   };
   ```

5. **Run the application**:
   ```bash
   ng serve
   ```

   The app will be available at `http://localhost:4200`

## 📚 API Documentation

Once the backend is running, visit `https://localhost:7001/swagger` for interactive API documentation.

### Key Endpoints

#### Authentication
- `POST /api/authentication/register` - Register new user
- `POST /api/authentication/login` - Login user

#### Products
- `GET /api/products` - Get all products (with filtering)
- `GET /api/products/{id}` - Get product details
- `POST /api/products` - Create product (Admin only)
- `PUT /api/products/{id}` - Update product (Admin only)
- `DELETE /api/products/{id}` - Delete product (Admin only)

#### Cart
- `GET /api/carts` - Get user's cart
- `POST /api/carts/items` - Add item to cart
- `DELETE /api/carts/items/{itemId}` - Remove from cart

#### Orders
- `GET /api/orders` - Get user's orders
- `POST /api/orders` - Create order

## 👤 Default Admin Credentials

After seeding the database:
- **Email**: `admin@vgclassic.com`
- **Password**: `Admin@123`

## 🎨 Design Guidelines

### UI/UX Principles
- **Futuristic Design**: Modern, clean aesthetic
- **Mobile-First**: Responsive on all devices
- **Easy Navigation**: Intuitive user flows
- **Performance**: Fast loading, optimized images

### Color Scheme
- Primary: Modern teal/cyan tones
- Secondary: Dark grays and blacks
- Accent: Bright highlights for CTAs
- Background: Light grays and whites

## 📦 Database Schema

### Main Entities
- **Product** - Clothing items with variants
- **ProductVariant** - Size and color options
- **Category** - Product categorization
- **Cart** - User shopping cart
- **Order** - Customer orders
- **User** - Customer and admin accounts

## 🔐 Authentication Flow

1. **User Registration** → Creates account with Customer role
2. **Login** → Receives JWT access token + refresh token
3. **Protected Routes** → JWT token sent in Authorization header
4. **Token Refresh** → Refresh token extends session
5. **Role-Based Access** → Admin routes require Admin/SuperAdmin role

## 📱 Frontend Architecture

### Core Module
- Authentication service
- HTTP interceptors (auth, error)
- Guards (AuthGuard, AdminGuard)

### Feature Modules
- **Auth**: Login, Register
- **Shop**: Product listing, product details
- **Cart**: Shopping cart management
- **Checkout**: Order placement
- **Admin**: Product/Order management

### Shared Module
- Reusable components (header, footer, product card)
- Pipes (currency formatting)
- Directives

## 🚀 Deployment

### Backend
- Deploy to Azure App Service or IIS
- Configure SQL Server connection
- Set up environment variables
- Enable HTTPS

### Frontend
- Build for production: `ng build --configuration production`
- Deploy to Azure Static Web Apps, Netlify, or Vercel
- Configure environment variables
- Set up CDN for static assets

## 📝 Next Steps

### High Priority
1. **Seed Sample Data** - Add products, categories, images
2. **Category Management** - Admin CRUD for categories
3. **Image Upload** - Allow admins to upload product images
4. **Enhanced Filters** - More product filtering options
5. **Payment Integration** - Integrate Stripe or PayPal

### Medium Priority
6. **Product Reviews** - Customer review system
7. **Wishlist** - Save products for later
8. **Email Notifications** - Order confirmations
9. **Search Functionality** - Full-text product search
10. **Analytics Dashboard** - Sales and customer analytics

### Nice to Have
11. **Social Login** - OAuth with Google/Facebook
12. **Discount Codes** - Promo code system
13. **Product Recommendations** - AI-powered suggestions
14. **Live Chat** - Customer support
15. **Multi-language** - i18n support

## 🤝 Contributing

This is a proprietary project for VG Classic. Please contact the development team for contribution guidelines.

## 📄 License

Proprietary - VG Classic © 2024

## 📞 Support

For issues or questions, please contact the VG Classic development team.

---

**Built with ❤️ for VG Classic**
