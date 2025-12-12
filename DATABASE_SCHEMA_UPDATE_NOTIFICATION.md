# ?? Database Schema Changes - Team Notification

## Summary
One new column was added to the `Seats` table to fix the Flight Management feature.

---

## ?? CRITICAL UPDATE NEEDED

### Migration Applied: `AddSeatPrice` (20251213000000)

**Changed Table:** `Seats`

| Change Type | Column Name | Data Type | Default Value | Nullable |
|------------|------------|-----------|----------------|----------|
| ? **ADDED** | `SeatPrice` | `int` | 100 | No |

---

## What This Means

### For Your Database:
- A new integer column `SeatPrice` was added to the `Seats` table
- All existing seat records will automatically get a default price of **100** (currency unit)
- This column stores the price for each individual seat class (Economy, Business, First Class)

### Seat Price Structure:
```
- Economy Class: 100
- Business Class: 250
- First Class: 500
```

### Impact:
- ? **NO DATA LOSS** - Existing seat data is preserved
- ? **NO BREAKING CHANGES** - Default values applied automatically
- ? **BACKWARD COMPATIBLE** - Old queries still work

---

## How to Update Your Database

### Option 1: Automatic Update (Recommended)
When team members start the application, the migration will apply automatically:
1. Clone/pull the latest code
2. Run the application
3. The migration runs automatically on startup
4. ? Database is updated

**No manual action needed!**

### Option 2: Manual Update via Package Manager Console (PowerShell)
If your team prefers manual control:
```powershell
cd "path\to\project"
dotnet ef database update
```

### Option 3: Check Current Migration Status
```powershell
dotnet ef migrations list
```

---

## What Your Team Needs to Do

1. ? **Pull the latest code** from the repository
2. ? **No manual database migration needed** - it's automatic
3. ? **Test Flight Management** to confirm it works:
   - Go to Admin Dashboard ? Manage Flights
   - Create a new flight
   - View flight details and seat pricing

---

## Code Changes Reference

### Files Modified:
- `Migrations/20251213000000_AddSeatPrice.cs` (New migration file)
- `Migrations/20251213000000_AddSeatPrice.Designer.cs` (New designer file)
- `Migrations/ApplicationDbContextModelSnapshot.cs` (Updated)
- `Program.cs` (Added automatic migration on startup)

### Models Affected:
- `Models/Seat.cs` - Already had the property, now synced with database

---

## Questions Your Team Might Have

**Q: Will this affect existing data?**
A: No. Existing seats will be assigned the default price of 100. No data loss.

**Q: Do we need to run migrations manually?**
A: No. The application automatically applies pending migrations on startup.

**Q: What if the migration fails?**
A: The application will log the error but still run. Check the logs and run `dotnet ef database update` manually.

**Q: Can we rollback if needed?**
A: Yes. Run: `dotnet ef database update <PreviousMigrationName>` or drop and recreate the database.

---

## Verification Checklist

- [ ] Team has pulled latest code
- [ ] Application starts successfully
- [ ] No error logs in console about migrations
- [ ] Flight Management page loads
- [ ] Can create a new flight (with seats having prices)
- [ ] Seat pricing displays correctly in views

---

## Contact

If anyone has issues:
1. Check application logs for migration errors
2. Ensure SQL Server is running
3. Verify connection string in `appsettings.json`
4. Run `dotnet ef database update` manually if needed

**Last Updated:** December 13, 2025
**Migration Name:** AddSeatPrice
**Status:** ? Applied and tested
