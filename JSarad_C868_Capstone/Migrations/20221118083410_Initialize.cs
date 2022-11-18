using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JSarad_C868_Capstone.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Food = table.Column<bool>(type: "bit", nullable: false),
                    Bar = table.Column<bool>(type: "bit", nullable: false),
                    Guests = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    EventId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSchedules", x => new { x.EventId, x.ScheduleId });
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
                    Username = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "Email", "LastUpdate", "Name", "Phone" },
                values: new object[] { 1, "123 Country Road", "eledford@email.com", new DateTime(2022, 11, 18, 0, 34, 9, 832, DateTimeKind.Local).AddTicks(6690), "Edwin Ledford", "6613332222" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Address", "Availability", "Email", "LastUpdate", "Name", "Phone", "Role" },
                values: new object[,]
                {
                    { 1, "2414 Loma Linda Dr", "MTWRFSU", "jsarad2@wgu.edu", new DateTime(2022, 11, 18, 0, 34, 9, 832, DateTimeKind.Local).AddTicks(6727), "Johanna Sarad", "6614444763", "Bartender" },
                    { 2, "345 Mullberry Way", "TRFSU", "rcrocker@email.com", new DateTime(2022, 11, 18, 0, 34, 9, 832, DateTimeKind.Local).AddTicks(6730), "Rebecca Crocker", "6613332211", "Server" },
                    { 3, "765 Atlantic St", "MWF", "iward@email.com", new DateTime(2022, 11, 18, 0, 34, 9, 832, DateTimeKind.Local).AddTicks(6732), "Ian Ward", "8057778899", "Server" }
                });

            migrationBuilder.InsertData(
                table: "EventSchedules",
                columns: new[] { "EventId", "ScheduleId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Bar", "ClientId", "CreatedBy", "CreatedOn", "EndTime", "EventDate", "EventName", "Food", "Guests", "LastUpdate", "Location", "Notes", "StartTime", "Type" },
                values: new object[] { 1, true, 1, 1, new DateTime(2022, 10, 21, 16, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 11, 10, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 11, 10, 16, 0, 0, 0, DateTimeKind.Unspecified), "Ledford LLC. Luncheon", true, 50, new DateTime(2022, 11, 18, 0, 34, 9, 832, DateTimeKind.Local).AddTicks(6744), "888 Corporate Way", "", new DateTime(2022, 11, 10, 16, 0, 0, 0, DateTimeKind.Unspecified), "Corporate" });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "EmployeeId", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 11, 10, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 11, 10, 16, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, new DateTime(2022, 11, 10, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 11, 10, 16, 0, 0, 0, DateTimeKind.Unspecified) }
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
