using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Airline_Management_System__AMS_.Models;

namespace Airline_Management_System__AMS_.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Unique constraints
            builder.Entity<Flight>()
                .HasIndex(f => f.FlightNumber)
                .IsUnique();

            builder.Entity<Passenger>()
                .HasIndex(p => p.PassportNumber)
                .IsUnique();
            var hasher = new PasswordHasher<ApplicationUser>();
            var adminRoleId = "admin-role-id-0001";
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
            var adminUserId = "admin-user-id-0001";
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = string.Empty,

                
                FirstName = "System",
                LastName = "Admin",
                Role = "Admin",
                EmailConfirmationCode = Guid.NewGuid().ToString(),
                LastVerificationEmailSent = DateTime.UtcNow,
                VerificationResendCount = 0
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@123"); 

            builder.Entity<ApplicationUser>().HasData(adminUser);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Seat> Seats { get; set; }
    }
}
