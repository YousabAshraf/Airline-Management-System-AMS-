using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline_Management_System__AMS_.Migrations
{
    /// <inheritdoc />
    public partial class AddExtraFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "Passengers",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-0001",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmationCode", "LastVerificationEmailSent", "NationalId", "PassportNumber", "PasswordHash", "PhoneNumber" },
                values: new object[] { "210cdb8b-b094-48fe-80cf-5d7738baf6c7", "35ae3aee-9a22-45dc-9667-025b7a912306", new DateTime(2025, 12, 10, 3, 12, 24, 132, DateTimeKind.Utc).AddTicks(4234), "00000000000000", "TEMP", "AQAAAAIAAYagAAAAEEO8TklIFiKvQV6JZntuYmM+txJNvWPgZSJXWMVeXaoCYmR5R0ydkhgHeG+D/HDitQ==", "0123456789" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "Passengers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-0001",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmationCode", "LastVerificationEmailSent", "PasswordHash", "PhoneNumber" },
                values: new object[] { "619f86f9-6eed-446a-8279-5b0172df64c6", "fd7a0c62-fa4f-4e2c-a4de-d7e5e13da30d", new DateTime(2025, 12, 10, 0, 59, 22, 825, DateTimeKind.Utc).AddTicks(1751), "AQAAAAIAAYagAAAAEAc8PPmUY2y9zhjd3Ke3Iv2XpTCBZY9S47G2sRlsGY3XHJUxZoOEJGyp7FMoGxaSxQ==", null });
        }
    }
}
