using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline_Management_System__AMS_.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeatPrice",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatPrice",
                table: "Seats");
        }
    }
}
