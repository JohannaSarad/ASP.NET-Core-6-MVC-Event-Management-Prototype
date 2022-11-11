using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.Models;
using JSarad_C868_Capstone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace JSarad_C868_Capstone.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly AppDbContext _db;
        //public Employee? SelectedEmployee { get; set; }
        //public Event? SelectedEvent { get; set; }
        //public List<EmployeeSchedule> tempSchedule { get; set; }
        public EventController(AppDbContext db)
        {
            _db = db;
           
        }

        // /Event
        //Returns Event List View
        public IActionResult Index()
        {
            var events = from e in _db.Events
                         join c in _db.Clients on e.ClientId equals c.Id
                         select new EventListDetails
                         {
                             EventId = e.Id,
                             StartTime = e.StartTime,
                             EndTime = e.EndTime,
                             Type = e.Type,
                             ClientId = e.ClientId,
                             ContactName = c.Name,
                             Phone = c.Phone
                         };
            
            
            return View(events);
        }

        //Event Table Search (returns Events by type)
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            ViewData["GetEvent"] = search;
            var searchQuery = from e in _db.Events
                              join c in _db.Clients on e.ClientId equals c.Id
                              select new EventListDetails
                              {
                                  EventId = e.Id,
                                  StartTime = e.StartTime,
                                  EndTime = e.EndTime,
                                  Type = e.Type,
                                  ClientId = e.ClientId,
                                  ContactName = c.Name,
                                  Phone = c.Phone
                              };
            
            if (!string.IsNullOrEmpty(search))
            {
                searchQuery = searchQuery.Where(e => e.Type.Contains(search));
            }
            return View(await searchQuery.AsNoTracking().ToListAsync());
        }

        //Get: /Event/Modify/{id}

        [HttpGet]
        public IActionResult Modify(int id)
        {
            EventViewModel viewModel = new EventViewModel();
            if (id == 0)
            {
                viewModel.Event = new Event();
                viewModel.Event.StartTime = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                viewModel.Event.EndTime = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            }
            else
            {
                viewModel.Event = _db.Events.Find(id);
                var client = _db.Clients.Find(viewModel.Event.ClientId);
                viewModel.ClientName = client.Name;
            }
            return PartialView("_ModifyEventModalPartial", viewModel); ;
        }

        //Post: /Event/Modify/{id}
        /*adds and edits and event. Code behind for ~Views/Event/_ModifyEventModalPartial.
          Called from site.js Modify Object Function*/
        [HttpPost]
        public IActionResult Modify(EventViewModel viewModel)
        {
            if (viewModel.Event.StartTime < viewModel.Event.EndTime)
            {
                if (ModelState.IsValid)
                {
                    //viewModel.Event.ClientId = viewModel.Client.Id;
                    if (viewModel.Event.Id == 0)
                    {
                        Console.WriteLine(viewModel);
                        _db.Events.Add(viewModel.Event);
                        _db.SaveChanges();
                    }
                    else
                    {

                        _db.Events.Update(viewModel.Event);
                        _db.SaveChanges();
                    }
                    return Ok(true);
                }
            }
            ModelState.AddModelError("Event.EndTime", "* Start Time must be before End Time");

            return PartialView("_ModifyEventModalPartial", viewModel);
        }

        [HttpPost]
        public IActionResult DeletePOST(int? id)
        {
            var selectedEvent = _db.Events.Find(id);
            if (selectedEvent == null)
            {
                return NotFound();
            }
            _db.Events.Remove(selectedEvent);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult AddSchedule(int id)
        {
            EventScheduleViewModel viewModel = new EventScheduleViewModel();
            
            viewModel.Event = _db.Events.Find(id);
            viewModel.Client = _db.Clients.Find(viewModel.Event.ClientId);
            viewModel.EmployeeSchedule = new ScheduleDisplayDetails();
            viewModel.EmployeeSchedule.StartTime = viewModel.Event.StartTime;
            viewModel.EmployeeSchedule.EndTime = viewModel.Event.EndTime;
            viewModel.EmployeeList = _db.Employees.ToList();
            //viewModel.Employee = new Employee();
            viewModel.Includes = Included(viewModel.Event);
            
            


            //viewModel.EmployeeSchedule = new Schedule();
            viewModel.Schedules = (from es in _db.EventSchedules
                                   join s in _db.Schedules on es.ScheduleId equals s.Id
                                   join e in _db.Employees on s.EmployeeId equals e.Id
                                   where es.EventId == viewModel.Event.Id
                                   select new ScheduleDisplayDetails
                                   {
                                       EmployeeId = e.Id,
                                       EmployeeName = e.Name,
                                       StartTime = s.StartTime,
                                       EndTime = s.EndTime
                                   }).ToList();
            return View(viewModel);
        }




        [HttpPost]
        public IActionResult ScheduleEmployee(EventScheduleViewModel viewModel)
        {
            var schedules = _db.Schedules;
            Employee employee = _db.Employees.Find(viewModel.EmployeeSchedule.EmployeeId);
            DateTime start = viewModel.EmployeeSchedule.StartTime;
            DateTime end = viewModel.EmployeeSchedule.EndTime;
            ModelState.Clear();
            bool valid = true;
            if (viewModel.EmployeeSchedule.EmployeeId == 0)
            {
                ViewData["Error"] = "Please Select an employee for this Schedule";
                valid = false;
            }
            //validate employee availability for event
            else if ((viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Monday && !employee.Availability.Contains("M"))
                || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Tuesday && !employee.Availability.Contains("T"))
                || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Wednesday && !employee.Availability.Contains("W"))
                || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Thursday && !employee.Availability.Contains("R"))
                || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Friday && !employee.Availability.Contains("F"))
                || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Saturday && !employee.Availability.Contains("S"))
                || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Sunday && !employee.Availability.Contains("U")))
            {
                ViewData["Error"] = ($"{employee.Name}'s availability is not open for the event on {viewModel.Event.StartTime.ToLongDateString()}");
                valid = false;
            }
            foreach (var schedule in schedules)
            {
                // validate employee does not have an overlapping schedule
                if ((schedule.StartTime < start && schedule.EndTime > start) || 
                    (schedule.StartTime > start && schedule.EndTime < end) ||
                    (schedule.StartTime < end && schedule.EndTime > end) ||
                    (schedule.StartTime == start) || (schedule.StartTime == end) ||
                    (schedule.EndTime == start) || (schedule.EndTime == end))
                {
                    ViewData["Error"] = ($"{employee.Name} is already working (give events a name) on  {viewModel.Event.StartTime.ToLongDateString()}" +
                        $"from {schedule.StartTime.ToShortTimeString()} to {schedule.EndTime.ToShortTimeString()}");
                    valid = false;
                }
            }

            ScheduleDisplayDetails employeeSchedule = new ScheduleDisplayDetails()
            {
                EmployeeId = viewModel.EmployeeSchedule.EmployeeId,
                EmployeeName= viewModel.EmployeeSchedule.EmployeeName,
                StartTime = viewModel.EmployeeSchedule.StartTime,
                EndTime = viewModel.EmployeeSchedule.EndTime
            };

            if (valid)
            {
                viewModel.Schedules.Add(employeeSchedule);
            }
            
            return View("AddSchedule", viewModel);
        }
            
            

            
             

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var clients =  (from client in _db.Clients
                           where client.Name.StartsWith(prefix)
                           select new
                           {
                               label = client.Name,
                               val = client.Name,
                               id = client.Id,
                           }).Take(5).ToList();

            return Json(clients);
        }

        [HttpPost]
        public JsonResult Selection(int id)
        {
            var selectedEvent = _db.Events.Find(id);
            
                return Json(selectedEvent.Type);
            
        }

        public JsonResult EmployeeSelection(int id)
        {
            var selectedEmployee = _db.Employees.Find(id);

            return Json(selectedEmployee.Name);

        }

        //concats string for displaying included services in Event
        public string Included(Event modelEvent)
        {
            string includes = "";
            if (modelEvent.Food == true)
            {
                includes = "Food";
                if (modelEvent.Bar == true)
                {
                    includes += " & Bar";
                }
            }
            else if (modelEvent.Bar == true)
            {
                includes = "Bar";
            }
            return includes;

        }

        //public List<Employee> GetEmployees()
        //{
        //    var employeeList = new List<Employee>();
        //    if (_db.Employees.Any())
        //    {
        //        employeeList = _db.Employees.ToList();
               
        //    }
        //    return employeeList.ToList();
            
        //}

    //    [AcceptVerbs("Get", "Post")]
    //    public JsonResult IsStartTimeFirst(DateTime StartTime, DateTime EndTime)
    //    {

    //        if (StartTime > EndTime)
    //        {
    //            return Json(data: false);
    //        }
    //        return Json(data: true);
    //    }


    }

}
    

    


    

   
