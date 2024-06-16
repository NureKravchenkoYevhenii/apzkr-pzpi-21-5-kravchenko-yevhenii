using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMembershipModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_DurationInTimeUnits_Memberships",
                table: "Membership");

            migrationBuilder.DropColumn(
                name: "DurationInTimeUnits",
                table: "Membership");

            migrationBuilder.RenameColumn(
                name: "TimeUnitValue",
                table: "Membership",
                newName: "DurationInDays");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2024, 5, 14, 21, 11, 23, 969, DateTimeKind.Utc).AddTicks(6059));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationInDays",
                table: "Membership",
                newName: "TimeUnitValue");

            migrationBuilder.AddColumn<int>(
                name: "DurationInTimeUnits",
                table: "Membership",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2024, 5, 11, 10, 1, 59, 220, DateTimeKind.Utc).AddTicks(5547));

            migrationBuilder.AddCheckConstraint(
                name: "CHK_DurationInTimeUnits_Memberships",
                table: "Membership",
                sql: "[DurationInTimeUnits] >= 0");
        }
    }
}
