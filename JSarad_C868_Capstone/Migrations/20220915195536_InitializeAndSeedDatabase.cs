using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JSarad_C868_Capstone.Migrations
{
    public partial class InitializeAndSeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Availability = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Food = table.Column<bool>(type: "bit", nullable: false),
                    Bar = table.Column<bool>(type: "bit", nullable: false),
                    Guests = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSchedules", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Administrators",
                columns: new[] { "Id", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "Test", "Admin" },
                    { 2, "password", "Planner" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "Email", "LastUpdate", "Name", "Phone" },
                values: new object[] { 1, "123 Country Road", "eledford@email.com", new DateTime(2022, 9, 15, 12, 55, 35, 892, DateTimeKind.Local).AddTicks(1850), "Edwin Ledford", "6613332222" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Address", "Availability", "Email", "LastUpdate", "Name", "Phone", "Role" },
                values: new object[,]
                {
                    { 1, "2414 Loma Linda Dr", "MTWRFSU", "jsarad2@wgu.edu", new DateTime(2022, 9, 15, 12, 55, 35, 892, DateTimeKind.Local).AddTicks(1898), "Johanna Sarad", "6614444763", "Bartender" },
                    { 2, "345 Mullberry Way", "TRFSU", "rcrocker@email.com", new DateTime(2022, 9, 15, 12, 55, 35, 892, DateTimeKind.Local).AddTicks(1902), "Rebecca Crocker", "66133322111", "Server" },
                    { 3, "765 Atlantic St", "MWF", "iward@email.com", new DateTime(2022, 9, 15, 12, 55, 35, 892, DateTimeKind.Local).AddTicks(1904), "Ian Ward", "8057778899", "Server" }
                });

            migrationBuilder.InsertData(
                table: "EventSchedules",
                columns: new[] { "Id", "EmployeeId", "EventId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Bar", "ClientId", "EventEnd", "EventStart", "Food", "Guests", "LastUpdate", "Location", "Type" },
                values: new object[] { 1, true, 1, new DateTime(2022, 11, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 11, 10, 4, 30, 0, 0, DateTimeKind.Unspecified), true, 50, new DateTime(2022, 9, 15, 12, 55, 35, 892, DateTimeKind.Local).AddTicks(1915), "888 Corporate Way", "Corporate Event" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "EventSchedules");
        }
    }
}
