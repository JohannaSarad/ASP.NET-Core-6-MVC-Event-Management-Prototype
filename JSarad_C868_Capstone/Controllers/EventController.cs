using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.Models;
using JSarad_C868_Capstone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JSarad_C868_Capstone.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _db;
        public Employee? SelectedEmployee { get; set; }
        public Event? SelectedEvent { get; set; }
        public List<EmployeeSchedule> tempSchedule { get; set; }
        public EventController(AppDbContext db)
        {
            _db = db;
           
        }

        //Get /Event
        //public IActionResult Index()
        //{
            
        //    var result = from e in _db.Events
        //                 join c in _db.Clients on e.ClientId equals c.Id
        //                 select new EventListDetails
        //                 {
        //                     EventId = e.Id,
        //                     StartDate = e.EventStart,
        //                     EndDate = e.EventEnd,
        //                     Type = e.Type,
        //                     ClientId = e.ClientId,
        //                     ContactName = c.Name,
        //                     Phone = c.Phone
        //                 };//.ToList();
        //    EventListViewModel viewModel = new EventListViewModel();
        //    viewModel.EventList = result;
        //    viewModel.SelectedId = 0;
        //    viewModel.SelectedName = "";
           
        //    return View(viewModel);
        //}

        //public IActionResult Index()
        //{
        //    var events = from e in _db.Events
        //                 join c in _db.Clients on e.ClientId equals c.Id
        //                 select new EventListDetails
        //                 {
        //                     EventId = e.Id,
        //                     StartDate = e.EventStart,
        //                     EndDate = e.EventEnd,
        //                     Type = e.Type,
        //                     ClientId = e.ClientId,
        //                     ContactName = c.Name,
        //                     Phone = c.Phone
        //                 };
        //    return View(events);
        //}

        public IActionResult Index()
        {
            var events = from e in _db.Events
                         join c in _db.Clients on e.ClientId equals c.Id
                         select new EventListDetails
                         {
                             EventId = e.Id,
                             StartDate = e.EventStart,
                             EndDate = e.EventEnd,
                             Type = e.Type,
                             ClientId = e.ClientId,
                             ContactName = c.Name,
                             Phone = c.Phone
                         };
            
            
            return View(events);
        }

        //Get: /Event/Modify/{id}
        [HttpGet]
        public IActionResult Modify(int id)
        {
            EventViewModel viewModel = new EventViewModel();
            if (id == 0)
            {
                viewModel.Event = new Event();
                
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

            //viewModel.Client = _db.Clients.Find(viewModel.Event.ClientId);

            Console.WriteLine(ModelState);
            if (ModelState.IsValid)
            {
                //viewModel.Event.ClientId = viewModel.Client.Id;
                if (viewModel.Event.Id == 0)
                {
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
                               //address = client.Address,
                               //phone = client.Phone,
                               //email = client.Email
                           }).Take(5).ToList();

            return Json(clients);
        }

        [HttpPost]
        public JsonResult Selection(int id)
        {
            SelectedEvent = _db.Events.Find(id);
            
                return Json(SelectedEvent.Type);
            
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
    }

    //public IActionResult Add()
    //{
    //    EventViewModel viewModel = new EventViewModel();
    //    viewModel.Event = new Event();
    //    viewModel.Client = new Client();
    //    viewModel.SelectedEmployee = new Employee();
    //    viewModel.EmployeeSchedule = new EmployeeSchedule();
    //    viewModel.Schedules = new List<EmployeeSchedule>();
    //    viewModel.EmployeeList = GetEmployees();

    //    return View(viewModel);
    //}


    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public IActionResult Add(EventViewModel viewModel)
    //{
    //    viewModel.EmployeeList = GetEmployees();
    //    Console.WriteLine(viewModel.EmployeeList);

    //    if (ModelState.IsValid)
    //    {
    //        viewModel.Schedules = new List<EmployeeSchedule>();
    //        _db.Events.Add(viewModel.Event);


    //        _db.SaveChanges();
    //        //redirecting to the main event page with list of events
    //        return RedirectToAction("Index");
    //    }
    //    else
    //    {
    //        return View(viewModel);
    //    }
    //}

    ////Get /Employee/Edit
    //public IActionResult Edit(int? id)
    //{
    //    if (id == null || id == 0)
    //    {
    //        return NotFound();
    //    }
    //    var selectedEvent = _db.Events.Find(id);

    //    //check if selectedEmployee is null
    //    if (selectedEvent == null)
    //    {
    //        return NotFound();
    //    }

    //    EventViewModel viewModel = new EventViewModel();
    //    viewModel.Event = selectedEvent;

    //    return View(viewModel);

    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public IActionResult Edit(EventViewModel viewModel)
    //{
    //    //viewModel.Employee.Availability = daysToChars;
    //    //validation
    //    if (ModelState.IsValid)
    //    {
    //        //updatng a employee in the database
    //        _db.Events.Update(viewModel.Event);
    //        _db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }
    //    else
    //    {
    //        return View(viewModel);
    //    }
    //}

    //[HttpPost]
    //public IActionResult ScheduleEmployee(EventViewModel viewModel)
    //{

    //    if (viewModel.EmployeeSchedule != null)
    //    {
    //        if (viewModel.Schedules == null)
    //        {
    //            viewModel.Schedules = new List<EmployeeSchedule>();
    //        }
    //        viewModel.Schedules.Add(viewModel.EmployeeSchedule);

    //        //viewModel.EmployeeSchedule.EmployeeId = SelectedEmployee.Id;
    //        //viewModel.EmployeeSchedule.Name = SelectedEmployee.Name;
    //        //viewModel.Schedules.Add(viewModel.EmployeeSchedule);
    //    }
    //    return View("Add", viewModel);
    //}
}
