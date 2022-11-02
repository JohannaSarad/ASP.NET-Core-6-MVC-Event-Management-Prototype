//using JSarad_C868_Capstone.Models;
//using Microsoft.EntityFrameworkCore;

//namespace JSarad_C868_Capstone.Data
//{
//    public class DbInitializer
//    {
//        private readonly AppDbContext _context;
//        public int TempClientId { get; set; }
//        public int TempEmployeeId { get; set; }
//        public int TempEmployeeId2 { get; set; }
//        public int TempEmployeeId3 { get; set; }
//        public int TempEventId { get; set; }
      
//        public DbInitializer(AppDbContext context)
//        {
//            _context = context;
//        }

//        public void Run()
//        {
//            //create Database and tables of not exist
//            _context.Database.Migrate();

//            //seed database
//            if (!_context.Administrators.Any()) 
//            {
//                var seedAdmin = new Administrator()
//                {
//                    Username = "Admin",
//                    Password = "Test"
//                };
//                var seedAdmin2 = new Administrator()
//                {
//                    Username = "Planner",
//                    Password = "password"
//                };
//                _context.Administrators.Add(seedAdmin);
//                _context.Administrators.Add(seedAdmin2);
//            }
//            if (!_context.Clients.Any() && !_context.Employees.Any() && !_context.Events.Any() && !_context.EventSchedules.Any())
//            {
//                var seedClient = new Client()
//                {
//                    Name = "Edwin Ledford",
//                    Phone = "6613332222",
//                    Email = "eledford@email.com",
//                    Address = "123 Country Road"
//                };
//                _context.Clients.Add(seedClient);
//                TempClientId = seedClient.Id;

//                var seedEmployee = new Employee() 
//                {
//                    Name = "Johanna Sarad",
//                    Phone = "6614444763",
//                    Email = "jsarad2@wgu.edu",
//                    Address = "2414 Loma Linda Dr",
//                    Role = "Bartender",
//                    Availability = "MTWRFSU"
//                };
//                var seedEmployee2 = new Employee()
//                {
//                    Name = "Rebecca Crocker",
//                    Phone = "66133322111",
//                    Email = "rcrocker@email.com",
//                    Address = "345 Mullberry Way",
//                    Role = "Server",
//                    Availability = "TRFSU"
//                };
//                var seedEmployee3 = new Employee()
//                {
//                    Name = "Ian Ward",
//                    Phone = "8057778899",
//                    Email = "iward@email.com",
//                    Role = "Server",
//                    Availability = "MWF"
//                };
//                _context.Employees.Add(seedEmployee);
//                TempEmployeeId = seedEmployee.Id;
//                _context.Employees.Add(seedEmployee2);
//                TempEmployeeId2 = seedEmployee2.Id;
//                _context.Employees.Add(seedEmployee3);
//                TempEmployeeId3 = seedEmployee3.Id;

//                var seedEvent = new Event()
//                {
//                    Type = "Corporate Event",
//                    Location = "888 Corporate Way",
//                    EventStart = new DateTime(2022, 11, 10, 4, 30, 00),
//                    EventEnd = new DateTime(2022, 11, 10, 10, 00, 00),
//                    Food = true,
//                    Bar = true,
//                    Guests = 50,
//                    ClientId = TempClientId,
//                };
//                var seedEventSchedule = new EventSchedule()
//                {
//                    EventId = TempEventId,
//                    EmployeeId = TempEmployeeId,
//                };
//                var seedEventSchedule2 = new EventSchedule()
//                {
//                    EventId = TempEventId,
//                    EmployeeId = TempEmployeeId2,
//                };
//                var seedEventSchedule3 = new EventSchedule()
//                {
//                    EventId = TempEventId,
//                    EmployeeId = TempEmployeeId3,
//                };
//                _context.EventSchedules.Add(seedEventSchedule);
//                _context.EventSchedules.Add(seedEventSchedule2);
//                _context.EventSchedules.Add(seedEventSchedule3);
//            }
//        }
//    }
//}
