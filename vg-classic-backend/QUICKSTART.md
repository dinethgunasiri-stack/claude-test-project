# 🚀 VG Classic Backend - Quick Start Guide

## Open in Visual Studio (5 minutes)

### Step 1: Open Solution
1. Launch **Visual Studio 2022**
2. Click **"Open a project or solution"**
3. Navigate to: `vg-classic-backend/VGClassic.sln`
4. Click **Open**

### Step 2: Restore Packages (Automatic)
- Visual Studio will automatically restore NuGet packages
- Wait for "Restore complete" message in status bar
- If not automatic, right-click Solution → "Restore NuGet Packages"

### Step 3: Set Startup Project
- In Solution Explorer, right-click **VGClassic.API**
- Select **"Set as Startup Project"**
- The project name should now be bold

### Step 4: Create Database
**Using Package Manager Console:**
1. Tools → NuGet Package Manager → **Package Manager Console**
2. Set dropdown to: **VGClassic.Infrastructure**
3. Run these commands:
```powershell
Add-Migration InitialCreate
Update-Database
```

### Step 5: Add Seed Data
1. Open **VGClassic.API/Program.cs**
2. See **VISUAL-STUDIO-SETUP.md** for complete seed data code
3. Add the seed code after `var app = builder.Build();`
4. Add required using statements at the top

### Step 6: Run! 🎉
- Press **F5** (or click green play button)
- Browser opens automatically to: `https://localhost:7001/swagger`
- Test the API endpoints!

---

## Default Admin Login
```
Email: admin@vgclassic.com
Password: Admin@123
```

---

## Quick Test Checklist
- [ ] Solution opens without errors
- [ ] All 4 projects visible in Solution Explorer
- [ ] Database created successfully
- [ ] Application runs (F5)
- [ ] Swagger UI loads
- [ ] GET /api/products returns data
- [ ] Can login with admin credentials

---

## Common Issues

**"Can't connect to database"**
→ Make sure SQL Server is running

**"Migration already exists"**
→ Delete Migrations folder and run again

**"Port in use"**
→ Close other instances or change port in launchSettings.json

---

## What's Included

✅ **4 Projects** (Domain, Application, Infrastructure, API)
✅ **100+ Files** with complete implementation
✅ **Clean Architecture** with CQRS
✅ **JWT Authentication** with roles
✅ **Entity Framework Core** Code-First
✅ **Swagger Documentation**

---

## Next Steps After Backend is Running

1. Test all API endpoints in Swagger
2. Create test users and products
3. Start building Angular frontend
4. Follow **SETUP-GUIDE.md** for Angular setup

---

## Need Detailed Help?

- **Visual Studio specific**: `VISUAL-STUDIO-SETUP.md`
- **Complete setup**: `SETUP-GUIDE.md`
- **Project overview**: `README.md`
- **Current status**: `PROJECT-STATUS.md`

---

**You're all set! Happy coding! 🎉**
