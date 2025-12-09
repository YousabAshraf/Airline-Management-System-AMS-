using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline_Management_System__AMS_.Migrations
{
    /// <inheritdoc />
    public partial class AddingAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "admin-role-id-0001", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmationCode", "EmailConfirmed", "FirstName", "LastName", "LastVerificationEmailSent", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName", "VerificationResendCount" },
                values: new object[] { "admin-user-id-0001", 0, "7eed2b1b-24cf-44b0-a80f-73602742e0f3", "admin@example.com", "ae7330c6-1e90-408c-aac6-53736aa1fc80", true, "System", "Admin", new DateTime(2025, 12, 9, 23, 30, 5, 350, DateTimeKind.Utc).AddTicks(4912), false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEO3NdLIchZm9VHOxcunJcP+Pv5FqZ58/bjveJXhTRiPWBjwseZCA1USkffZ9jemxQQ==", null, false, "Admin", "", false, "admin", 0 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "admin-role-id-0001", "admin-user-id-0001" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "admin-role-id-0001", "admin-user-id-0001" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id-0001");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-0001");
        }
    }
}
