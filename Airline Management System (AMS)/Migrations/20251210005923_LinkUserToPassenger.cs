using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline_Management_System__AMS_.Migrations
{
    /// <inheritdoc />
    public partial class LinkUserToPassenger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Passengers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-0001",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmationCode", "LastVerificationEmailSent", "PasswordHash" },
                values: new object[] { "619f86f9-6eed-446a-8279-5b0172df64c6", "fd7a0c62-fa4f-4e2c-a4de-d7e5e13da30d", new DateTime(2025, 12, 10, 0, 59, 22, 825, DateTimeKind.Utc).AddTicks(1751), "AQAAAAIAAYagAAAAEAc8PPmUY2y9zhjd3Ke3Iv2XpTCBZY9S47G2sRlsGY3XHJUxZoOEJGyp7FMoGxaSxQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_UserId",
                table: "Passengers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_AspNetUsers_UserId",
                table: "Passengers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_AspNetUsers_UserId",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_UserId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Passengers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-0001",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmationCode", "LastVerificationEmailSent", "PasswordHash" },
                values: new object[] { "7eed2b1b-24cf-44b0-a80f-73602742e0f3", "ae7330c6-1e90-408c-aac6-53736aa1fc80", new DateTime(2025, 12, 9, 23, 30, 5, 350, DateTimeKind.Utc).AddTicks(4912), "AQAAAAIAAYagAAAAEO3NdLIchZm9VHOxcunJcP+Pv5FqZ58/bjveJXhTRiPWBjwseZCA1USkffZ9jemxQQ==" });
        }
    }
}
