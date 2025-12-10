using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline_Management_System__AMS_.Migrations
{
    /// <inheritdoc />
    public partial class NationalIdandRemovePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Flights");

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "Passengers",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-0001",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmationCode", "LastVerificationEmailSent", "PasswordHash" },
                values: new object[] { "b21c83c3-c532-49e5-b42b-f5cce211292f", "00b23cf3-716f-4612-8eb6-ca3ec22a40b5", new DateTime(2025, 12, 10, 18, 20, 10, 598, DateTimeKind.Utc).AddTicks(3712), "AQAAAAIAAYagAAAAEN3YosHkjT0l/l61PkK54IqalRzYDCGscvVSHoLX+hWiNV0A82h6nuj79Hl67Y2vzQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "Passengers",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Flights",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-0001",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmationCode", "LastVerificationEmailSent", "PasswordHash" },
                values: new object[] { "210cdb8b-b094-48fe-80cf-5d7738baf6c7", "35ae3aee-9a22-45dc-9667-025b7a912306", new DateTime(2025, 12, 10, 3, 12, 24, 132, DateTimeKind.Utc).AddTicks(4234), "AQAAAAIAAYagAAAAEEO8TklIFiKvQV6JZntuYmM+txJNvWPgZSJXWMVeXaoCYmR5R0ydkhgHeG+D/HDitQ==" });
        }
    }
}
