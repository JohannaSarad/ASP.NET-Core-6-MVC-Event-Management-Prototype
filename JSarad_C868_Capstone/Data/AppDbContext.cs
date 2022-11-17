using JSarad_C868_Capstone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JSarad_C868_Capstone.Data
{
    public class AppDbContext : DbContext
    {
        //establish connection with Entity Framework
        public static User currentUser { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //create category table with the name of categories
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventSchedule> EventSchedules { get; set; }
        public DbSet<Schedule> Schedules { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EventSchedule>().HasKey(e => new { e.EventId, e.ScheduleId });
            
            builder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "Admin",
                    Password = "Test"
                },
                new User
                {
                    Id = 2,
                    Username = "Planner",
                    Password = "password"
                });

            builder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    Name = "Edwin Ledford",
                    Phone = "6613332222",
                    Email = "eledford@email.com",
                    Address = "123 Country Road"
                });

            builder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Johanna Sarad",
                    Phone = "6614444763",
                    Email = "jsarad2@wgu.edu",
                    Address = "2414 Loma Linda Dr",
                    Role = "Bartender",
                    Availability = "MTWRFSU"
                },
                new Employee
                {
                    Id = 2,
                    Name = "Rebecca Crocker",
                    Phone = "6613332211",
                    Email = "rcrocker@email.com",
                    Address = "345 Mullberry Way",
                    Role = "Server",
                    Availability = "TRFSU"
                },
                new Employee
                {
                    Id = 3,
                    Name = "Ian Ward",
                    Phone = "8057778899",
                    Email = "iward@email.com",
                    Address = "765 Atlantic St",
                    Role = "Server",
                    Availability = "MWF"
                });

            builder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    EventName = "Ledford LLC. Luncheon",
                    Type = "Corporate",
                    Location = "888 Corporate Way",
                    EventDate = new DateTime(2022, 11, 10, 16, 00, 00),
                    StartTime = new DateTime(2022, 11, 10, 16, 00, 00),
                    EndTime = new DateTime(2022, 11, 10, 20, 00, 00),
                    Food = true,
                    Bar = true,
                    Guests = 50,
                    ClientId = 1,
                    CreatedBy = 1,
                    CreatedOn = new DateTime(2022, 10, 21, 16, 30, 00),
                    Notes = ""
                }); 
            builder.Entity<EventSchedule>().HasData(
                new EventSchedule
                {
                    //Id = 1,
                    EventId = 1,
                    ScheduleId = 1
                },
                new EventSchedule
                {
                    //Id = 2,
                    EventId = 1,
                    ScheduleId = 2
                });
            builder.Entity<Schedule>().HasData(
                new Schedule
                {
                    Id = 1,
                    EmployeeId = 1,
                    StartTime = new DateTime(2022, 11, 10, 16, 00, 00),
                    EndTime = new DateTime(2022, 11, 10, 20, 00, 00)
                },
                new Schedule
                {
                    Id = 2,
                    EmployeeId = 2,
                    StartTime = new DateTime(2022, 11, 10, 16, 00, 00),
                    EndTime = new DateTime(2022, 11, 10, 20, 00, 00)
                });
        }
    }
}
