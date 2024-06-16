using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddParkingSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingTimeAdvanceInMinutes = table.Column<int>(type: "int", nullable: false),
                    BookingDurationInMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ParkingSettings",
                columns: new[] { "Id", "BookingDurationInMinutes", "BookingTimeAdvanceInMinutes" },
                values: new object[] { 1, 20, 20 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2024, 5, 22, 4, 1, 39, 283, DateTimeKind.Utc).AddTicks(9564));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingSettings");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2024, 5, 14, 21, 11, 23, 969, DateTimeKind.Utc).AddTicks(6059));
        }
    }
}
