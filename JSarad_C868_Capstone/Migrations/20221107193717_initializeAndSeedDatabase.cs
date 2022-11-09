using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JSarad_C868_Capstone.Migrations
{
    public partial class initializeAndSeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Availability = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Food = table.Column<bool>(type: "bit", nullable: false),
                    Bar = table.Column<bool>(type: "bit", nullable: false),
                    Guests = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "Email", "LastUpdate", "Name", "Phone" },
                values: new object[] { 1, "123 Country Road", "eledford@email.com", new DateTime(2022, 11, 7, 11, 37, 17, 454, DateTimeKind.Local).AddTicks(3066), "Edwin Ledford", "6613332222" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Address", "Availability", "Email", "LastUpdate", "Name", "Phone", "Role" },
                values: new object[,]
                {
                    { 1, "2414 Loma Linda Dr", "MTWRFSU", "jsarad2@wgu.edu", new DateTime(2022, 11, 7, 11, 37, 17, 454, DateTimeKind.Local).AddTicks(3105), "Johanna Sarad", "6614444763", "Bartender" },
                    { 2, "345 Mullberry Way", "TRFSU", "rcrocker@email.com", new DateTime(2022, 11, 7, 11, 37, 17, 454, DateTimeKind.Local).AddTicks(3109), "Rebecca Crocker", "6613332211", "Server" },
                    { 3, "765 Atlantic St", "MWF", "iward@email.com", new DateTime(2022, 11, 7, 11, 37, 17, 454, DateTimeKind.Local).AddTicks(3111), "Ian Ward", "8057778899", "Server" }
                });

            migrationBuilder.InsertData(
                table: "EventSchedules",
                columns: new[] { "Id", "EventId", "ScheduleId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Bar", "ClientId", "EndTime", "Food", "Guests", "LastUpdate", "Location", "Notes", "StartTime", "Type" },
                values: new object[] { 1, true, 1, new DateTime(2022, 11, 10, 20, 0, 0, 0, DateTimeKind.Unspecified), true, 50, new DateTime(2022, 11, 7, 11, 37, 17, 454, DateTimeKind.Local).AddTicks(3123), "888 Corporate Way", "", new DateTime(2022, 11, 10, 16, 0, 0, 0, DateTimeKind.Unspecified), "Corporate Event" });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "EmployeeId", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 11, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 11, 10, 4, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, new DateTime(2022, 11, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 11, 10, 4, 30, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "Test", "Admin" },
                    { 2, "password", "Planner" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "EventSchedules");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
