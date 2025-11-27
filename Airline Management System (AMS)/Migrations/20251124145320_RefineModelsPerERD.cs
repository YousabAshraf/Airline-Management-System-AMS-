using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline_Management_System__AMS_.Migrations
{
    /// <inheritdoc />
    public partial class RefineModelsPerERD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "TotalSeats",
                table: "Flights",
                newName: "AvailableSeats");

            migrationBuilder.RenameColumn(
                name: "PassengerName",
                table: "Feedbacks",
                newName: "Comment");

            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "Passengers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FlightNumber",
                table: "Flights",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Feedbacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Feedbacks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TicketPrice",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_PassportNumber",
                table: "Passengers",
                column: "PassportNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightNumber",
                table: "Flights",
                column: "FlightNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_PassportNumber",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Flights_FlightNumber",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "AvailableSeats",
                table: "Flights",
                newName: "TotalSeats");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Feedbacks",
                newName: "PassengerName");

            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FlightNumber",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
