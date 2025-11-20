# VG Classic - Angular Frontend

## Overview
Modern Angular 17 frontend application for VG Classic fashion e-commerce platform with futuristic design.

## Features

### User Features
- 🔐 User Authentication (Login/Register)
- 🛍️ Product Catalog with Filters & Search
- 🔍 Product Detail Pages
- 🛒 Shopping Cart Management
- 💳 Checkout Process
- 📦 Order History

### Admin Features
- 📊 Admin Dashboard
- ➕ Product Management (Create/Update/Delete)
- 📋 Order Management
- 👥 User Management

## Tech Stack
- **Framework**: Angular 17
- **Styling**: Bootstrap 5 + Custom SCSS
- **State Management**: Services + RxJS
- **HTTP**: HttpClient with JWT Interceptor
- **Routing**: Angular Router with Guards
- **Forms**: Reactive Forms

## Project Structure
```
src/
├── app/
│   ├── core/                 # Core services, guards, interceptors
│   │   ├── guards/
│   │   ├── interceptors/
│   │   ├── services/
│   │   └── models/
│   ├── features/
│   │   ├── auth/            # Authentication module
│   │   ├── products/        # Product catalog
│   │   ├── cart/            # Shopping cart
│   │   ├── checkout/        # Checkout process
│   │   └── admin/           # Admin panel
│   ├── shared/              # Shared components
│   │   ├── components/
│   │   └── pipes/
│   └── layout/              # Layout components
│       ├── header/
│       ├── footer/
│       └── sidebar/
├── assets/                   # Static assets
├── environments/             # Environment configs
└── styles.scss              # Global styles

## Setup Instructions

### Prerequisites
- Node.js (v18 or higher)
- npm or yarn
- Angular CLI (`npm install -g @angular/cli`)

### Installation

1. **Install Dependencies**:
   ```bash
   npm install
   ```

2. **Configure API URL**:
   - Update `src/environments/environment.ts` with your backend API URL
   - Default: `https://localhost:7001/api`

3. **Run Development Server**:
   ```bash
   npm start
   # or
   ng serve
   ```

4. **Open Browser**:
   - Navigate to `http://localhost:4200`

### Build for Production

```bash
npm run build
# Output will be in dist/vg-classic-frontend/
```

## Configuration

### API Endpoints
The application connects to the following backend endpoints:
- `/Authentication/login` - User login
- `/Authentication/register` - User registration
- `/Products` - Product management
- `/Carts` - Shopping cart operations
- `/Orders` - Order management

### Authentication
- JWT tokens stored in localStorage
- Token automatically attached to requests via HTTP interceptor
- Auth guard protects routes requiring authentication
- Admin guard restricts access to admin-only routes

## Design Theme

### Color Palette
- Primary: Cyan (`#00f0ff`) - Futuristic neon glow
- Secondary: Magenta (`#ff00ff`) - Accent highlights
- Background: Dark blue gradients (`#0a0e27`, `#050814`)
- Accent: Purple (`#9d4edd`) and Green (`#06ffa5`)

### Typography
- Headers: **Orbitron** - Bold, futuristic font
- Body: **Rajdhani** - Clean, modern sans-serif

### UI Elements
- Glass morphism effects with backdrop blur
- Neon glow on interactive elements
- Smooth animations and transitions
- Responsive design for all screen sizes

## Key Components

### Authentication
- Login page with email/password
- Registration with validation
- JWT token management
- Role-based access control

### Product Catalog
- Grid layout with product cards
- Category filtering
- Price range filtering
- Search functionality
- Pagination

### Shopping Cart
- Add/remove items
- Quantity adjustment
- Real-time price calculation
- Persist cart in backend

### Admin Dashboard
- Product CRUD operations
- Order management
- User management
- Statistics and analytics

## Development

### Code Style
- TypeScript strict mode enabled
- Component-based architecture
- Service layer for business logic
- Reactive programming with RxJS

### Testing
```bash
npm test
```

### Linting
```bash
npm run lint
```

## Troubleshooting

### CORS Issues
- Ensure backend CORS is configured to allow `http://localhost:4200`
- Check `VGClassic.API/Program.cs` for CORS settings

### API Connection
- Verify backend is running on `https://localhost:7001`
- Check browser console for API errors
- Verify SSL certificate is trusted

### Build Errors
- Clear node_modules and reinstall: `rm -rf node_modules && npm install`
- Clear Angular cache: `ng cache clean`

## Browser Support
- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## License
Private - VG Classic © 2025
