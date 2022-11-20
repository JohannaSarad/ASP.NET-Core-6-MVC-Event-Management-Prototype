using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JSarad_C868_Capstone.Migrations
{
    public partial class CreatedOnNullibleFalse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2022, 11, 19, 18, 12, 55, 49, DateTimeKind.Local).AddTicks(2021));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2022, 11, 19, 18, 12, 55, 49, DateTimeKind.Local).AddTicks(2056));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2022, 11, 19, 18, 12, 55, 49, DateTimeKind.Local).AddTicks(2059));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2022, 11, 19, 18, 12, 55, 49, DateTimeKind.Local).AddTicks(2061));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2022, 11, 19, 18, 12, 55, 49, DateTimeKind.Local).AddTicks(2073));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Events",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2022, 11, 19, 8, 53, 11, 21, DateTimeKind.Local).AddTicks(1764));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2022, 11, 19, 8, 53, 11, 21, DateTimeKind.Local).AddTicks(1805));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2022, 11, 19, 8, 53, 11, 21, DateTimeKind.Local).AddTicks(1808));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2022, 11, 19, 8, 53, 11, 21, DateTimeKind.Local).AddTicks(1810));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2022, 11, 19, 8, 53, 11, 21, DateTimeKind.Local).AddTicks(1821));
        }
    }
}
