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
        public Employee? SelectedEmployee { get; set; }
        public Event? SelectedEvent { get; set; }
        //public List<EmployeeSchedule> tempSchedule { get; set; }
        public EventController(AppDbContext db)
        {
            _db = db;
           
        }


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
            viewModel.EmployeeList = _db.Employees.ToList();
            viewModel.Employee = new Employee();
            viewModel.EmployeeSchedule = new ScheduleDisplayDetails();
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
            if (viewModel.Schedules == null)
                {
                    viewModel.Schedules = new List<ScheduleDisplayDetails>();
                }
                viewModel.EmployeeSchedule.EmployeeId = SelectedEmployee.Id;
                viewModel.EmployeeSchedule.EmployeeName = SelectedEmployee.Name;
                viewModel.Schedules.Add(viewModel.EmployeeSchedule);
                viewModel.Schedules.Add(viewModel.EmployeeSchedule);

            return View("Add", viewModel);
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
            SelectedEmployee = _db.Employees.Find(id);
            
                return Json(SelectedEmployee.Name);
            
        }

        public List<Employee> GetEmployees()
        {
            var employeeList = new List<Employee>();
            if (_db.Employees.Any())
            {
                employeeList = _db.Employees.ToList();
               
            }
            return employeeList.ToList();
            
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsStartTimeFirst(DateTime StartTime, DateTime EndTime)
        {

            if (StartTime > EndTime)
            {
                return Json(data: false);
            }
            return Json(data: true);
        }


    }

}
    

    


    

   
