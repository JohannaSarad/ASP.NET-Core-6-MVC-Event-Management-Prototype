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
        public Event selectedEvent { get; set; }
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
                             EventName = e.EventName,
                             EventDate = e.EventDate,
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
                                  EventName = e.EventName,
                                  EventDate = e.EventDate,
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
                viewModel.Event.EventDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                viewModel.Event.StartTime = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                viewModel.Event.EndTime = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                viewModel.Title = "Add Event";
            }
            else
            {
                viewModel.Event = _db.Events.Find(id);
                var client = _db.Clients.Find(viewModel.Event.ClientId);
                viewModel.ClientName = client.Name;
                viewModel.Title = "Edit Event";
                viewModel.Notice = "Notice: making changes to event dates will remove any associated employee schedules for this event. " +
                    "Making changes to event times may cause incorrect employee schedule times. Please double check employee schedule times after making" +
                    " changes to the event times.";
            }
            return PartialView("_ModifyEventModalPartial", viewModel); ;
        }

        //Post: /Event/Modify/{id}
        /*adds and edits and event. Code behind for ~Views/Event/_ModifyEventModalPartial.
          Called from site.js Modify Object Function*/
        [HttpPost]
        public IActionResult Modify(EventViewModel viewModel)
        {
            int eventId = viewModel.Event.Id;
            var eventSchedule = from es in _db.EventSchedules where es.EventId == eventId select es;
            //Event originalEvent = _db.Events.Find(eventId);
            //DateTime originalStartTime = originalEvent.StartTime;
            //DateTime originalEndTime = originalEvent.EndTime;

            viewModel.Event.StartTime = viewModel.Event.EventDate.Date + viewModel.Event.StartTime.TimeOfDay;
            viewModel.Event.EndTime = viewModel.Event.EventDate.Date + viewModel.Event.EndTime.TimeOfDay;
            TimeSpan open = new TimeSpan(06, 00, 00);
            TimeSpan close = new TimeSpan(23, 00, 00);

            if (viewModel.Event.StartTime > viewModel.Event.EndTime)
            {
                ModelState.AddModelError("Event.EndTime", "* Event start time must be before Event end time");
            }

            if ((viewModel.Event.EndTime.TimeOfDay > close) || (viewModel.Event.StartTime.TimeOfDay < open)
                || (viewModel.Event.EndTime.TimeOfDay < open) || (viewModel.Event.StartTime.TimeOfDay > close))
            {
                ModelState.AddModelError("Event.EndTime", "* Events must be scheduled during hours of operation between 6:00 am and 11:00 pm");
            }

            if (ModelState.IsValid)
            {
                //viewModel.Event.ClientId = viewModel.Client.Id;
                if (eventId == 0)
                {
                    _db.Events.Add(viewModel.Event);
                    //_db.SaveChanges();
                }
                else
                {
                    if (eventSchedule.Any())
                    {
                        bool canUpdate = IsScheduleConflictRemoved(eventId, viewModel.Event.StartTime);
                        if (canUpdate) 
                        {
                            _db.Events.Update(viewModel.Event);
                        }
                    }
                }
                _db.SaveChanges();
                return Ok(true);
            }
            //check if date was changed and if there was an employee schedule associated
            
            return PartialView("_ModifyEventModalPartial", viewModel);
        }

        [HttpPost]
        public IActionResult DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                var selectedEvent = _db.Events.Find(id);
                if (selectedEvent == null)
                {
                    return NotFound();
                }
                _db.Events.Remove(selectedEvent);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddSchedule(int id)
        {
            EventScheduleViewModel viewModel = new EventScheduleViewModel();


            viewModel.Event = _db.Events.Find(id);
            selectedEvent = viewModel.Event;
            viewModel.Client = _db.Clients.Find(viewModel.Event.ClientId);
            viewModel.EmployeeSchedule = new ScheduleDisplayDetails();

            viewModel.EmployeeSchedule.StartTime = viewModel.Event.StartTime;
            viewModel.EmployeeSchedule.EndTime = viewModel.Event.EndTime;
            viewModel.EmployeeList = _db.Employees.ToList();
            //viewModel.Employee = new Employee();
            //viewModel.Includes = Included(viewModel.Event);

            //viewModel.EmployeeSchedule = new Schedule();
            viewModel.Schedules = (from es in _db.EventSchedules
                                   join s in _db.Schedules on es.ScheduleId equals s.Id
                                   join e in _db.Employees on s.EmployeeId equals e.Id
                                   where es.EventId == viewModel.Event.Id
                                   select new ScheduleDisplayDetails
                                   {
                                       ScheduleId = s.Id,
                                       EmployeeId = s.EmployeeId,
                                       EmployeeName = e.Name,
                                       StartTime = s.StartTime,
                                       EndTime = s.EndTime
                                   }).ToList();
            return View("AddSchedule", viewModel); ;
        }

        [HttpPost]
        public IActionResult AddSchedule(EventScheduleViewModel viewModel)
        {

            ModelState.Clear();
            viewModel.Event = _db.Events.Find(viewModel.Event.Id);
            viewModel.Client = _db.Clients.Find(viewModel.Client.Id);
            var employees = _db.Employees;
            var schedules = _db.Schedules;
            
            DateTime start = viewModel.Event.StartTime.Date + viewModel.EmployeeSchedule.StartTime.TimeOfDay;
            DateTime end = viewModel.Event.StartTime.Date + viewModel.EmployeeSchedule.EndTime.TimeOfDay;
            
            Employee selectedEmployee = _db.Employees.Find(viewModel.EmployeeSchedule.EmployeeId);

            TimeSpan open = new TimeSpan(06, 00, 00);
            TimeSpan close = new TimeSpan(23, 00, 00);

            //FIX ME!!! long list of validations starts here try to move to their own methods

            bool saveEnabled = false;

            //validate employee selected for schedule
            if (viewModel.EmployeeSchedule.EmployeeId == 0)
            {
                TempData["Error"] = "Please select the employee you would like to add to schedule";
                return View("AddSchedule", viewModel);
            }

            //validate employee schedule is within operation hours
            if ((viewModel.Event.EndTime.TimeOfDay > close) || (viewModel.Event.StartTime.TimeOfDay < open) || 
                (viewModel.Event.EndTime.TimeOfDay < open) || (viewModel.Event.StartTime.TimeOfDay > close))
            {
                TempData["Error"] = "Employees must be scheduled during hours of operation between 6:00 am and 11:00 pm";
                return View("AddSchedule", viewModel);
            }

            //validate employee availability for event
            if ((viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Monday && !selectedEmployee.Availability.Contains("M"))
            || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Tuesday && !selectedEmployee.Availability.Contains("T"))
            || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Wednesday && !selectedEmployee.Availability.Contains("W"))
            || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Thursday && !selectedEmployee.Availability.Contains("R"))
            || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Friday && !selectedEmployee.Availability.Contains("F"))
            || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Saturday && !selectedEmployee.Availability.Contains("S"))
            || (viewModel.Event.StartTime.DayOfWeek == DayOfWeek.Sunday && !selectedEmployee.Availability.Contains("U")))
            {
                TempData["Error"] = ($"{selectedEmployee.Name}'s availability is not open for the event on {viewModel.Event.StartTime.ToLongDateString()}");
                return View("AddSchedule", viewModel);
            }

            if (schedules.Any())
            {
                
                    
                foreach (Schedule schedule in schedules)
                {
                    if (schedule.EmployeeId == selectedEmployee.Id)
                    {
                        // validate employee does not have an overlapping schedule
                        if ((schedule.StartTime < start && schedule.EndTime > start) ||
                            (schedule.StartTime > start && schedule.EndTime < end) ||
                            (schedule.StartTime < end && schedule.EndTime > end) ||
                            (schedule.StartTime == start) || (schedule.StartTime == end) ||
                            (schedule.EndTime == start) || (schedule.EndTime == end))
                        {
                            TempData["Error"] = ($"{selectedEmployee.Name} is already working {viewModel.Event.EventName} on  {viewModel.Event.StartTime.ToLongDateString()}" +
                                $"from {schedule.StartTime.ToShortTimeString()} to {schedule.EndTime.ToShortTimeString()}");
                            return View("AddSchedule", viewModel);
                            //valid = false;
                        }
                    }
                }
            }
            
            Schedule scheduleToAdd = new Schedule() 
            {  
                EmployeeId = viewModel.EmployeeSchedule.EmployeeId,
                StartTime = viewModel.EmployeeSchedule.StartTime,
                EndTime = viewModel.EmployeeSchedule.EndTime
            };
            _db.Schedules.Add(scheduleToAdd);
            _db.SaveChanges();

            EventSchedule eventScheule = new EventSchedule()
            {
                EventId = viewModel.Event.Id,
                ScheduleId = scheduleToAdd.Id
            };
            _db.EventSchedules.Add(eventScheule);
            _db.SaveChanges();

            ScheduleDisplayDetails employeeSchedule = new ScheduleDisplayDetails()
            {
                ScheduleId = scheduleToAdd.Id,
                EmployeeId = viewModel.EmployeeSchedule.EmployeeId,
                EmployeeName = viewModel.EmployeeSchedule.EmployeeName,
                StartTime = viewModel.EmployeeSchedule.StartTime,
                EndTime = viewModel.EmployeeSchedule.EndTime
            };

            if (viewModel.Schedules == null)
            {
                viewModel.Schedules = new List<ScheduleDisplayDetails>();
                viewModel.Schedules.Add(employeeSchedule);
            }
            else
            {
                viewModel.Schedules.Add(employeeSchedule);
            }
                
            
            return View("AddSchedule", viewModel);
        }

            

        [HttpPost]
        public IActionResult CheckOT(EventScheduleViewModel viewModel)
        {
            ModelState.Clear();
            List<string> employeeOvertime = new List<string>();
            List<int> idsInEvent = new List<int>();
            viewModel.Event = _db.Events.Find(viewModel.Event.Id);
            viewModel.Client = _db.Clients.Find(viewModel.Client.Id);
            Employee employees = _db.Employees.Find(viewModel.EmployeeSchedule.EmployeeId);
            var schedules = _db.Schedules;

            DateTime start = viewModel.Event.StartTime.Date + viewModel.EmployeeSchedule.StartTime.TimeOfDay;
            DateTime end = viewModel.Event.StartTime.Date + viewModel.EmployeeSchedule.EndTime.TimeOfDay;
            //get approval for overtime
            

            DayOfWeek weekStart = DayOfWeek.Sunday;
            int offset = weekStart - viewModel.Event.StartTime.DayOfWeek;
            DateTime weekStartDate = viewModel.Event.StartTime.AddDays(offset);
            DateTime weekEndDate = weekStartDate.AddDays(6);
            //var timeAndAHalf = shiftLength - 8;
            //var doubleTime = shiftLength - 12;
            string overTimeMessage = "";
            
            if (viewModel.Schedules != null) 
            {
                idsInEvent = viewModel.Schedules.Select(s => s.EmployeeId).Distinct().ToList();
                Employee employee = new Employee();
                
                if (idsInEvent.Count > 0) {
                    float shiftLength = 0;
                    float weeklyHours = 0;
                    foreach (int employeeId in idsInEvent)
                    {
                        employee = _db.Employees.Find(employeeId);
                        foreach (Schedule schedule in schedules)
                        {
                            if ((start.Date == schedule.StartTime.Date) && (schedule.EmployeeId == employeeId))
                            {
                                shiftLength += (schedule.EndTime.TimeOfDay.Hours - schedule.StartTime.TimeOfDay.Hours);
                            }

                            //calculate hours in the week of the event
                            if ((schedule.StartTime >= weekStartDate && schedule.EndTime <= weekEndDate) && (schedule.EmployeeId == employeeId))
                            {
                                weeklyHours += (schedule.EndTime.TimeOfDay.Hours - schedule.StartTime.TimeOfDay.Hours);
                            }
                        }
                    }

                    if (shiftLength > 8 || weeklyHours > 40)
                    {
                        string ot = ($"{employee.Name} is scheduled for");
                        if (shiftLength > 8)
                        {
                            ot += ($"{shiftLength} hours on {viewModel.Event.StartTime.ToLongDateString()}");
                        }

                        if (weeklyHours > 40)
                        {
                            ot += ((shiftLength > 8 ? "and" : " ") + $"{weeklyHours} hours for the week starting {weekStartDate.ToLongDateString()}" +
                                $"and ending on {weekEndDate.ToLongDateString()}.");
                        }

                        employeeOvertime.Add(ot);
                        shiftLength = 0;
                        weeklyHours = 0;
                    }
                }
                    
                foreach(string message in employeeOvertime)
                {
                    overTimeMessage += ($"{message} \n\n");
                }
                TempData["Error"] = ($"California State Law requires employees recieve overtime at a rate of time and a half for any hours in excess " +
                                     $"of 40 per week or 8 per day, and a rate of double time for any hours in excess of 12 per day. \n") + overTimeMessage;
                return View("AddSchedule", viewModel);
            }
            TempData["Error"] = "No overtime hours were found for this event";
            return View("AddSchedule", viewModel);
        }
        
        [HttpPost]
        public IActionResult RemoveSchedule(int? id)
        {
            Schedule selectedSchedule = _db.Schedules.Find(id);
            EventSchedule eventSchedule = _db.EventSchedules.Where(es => es.ScheduleId == id).FirstOrDefault();
            var eventId = eventSchedule.EventId;
            if (selectedSchedule != null)
            {
                _db.Schedules.Remove(selectedSchedule);
                _db.EventSchedules.Remove(eventSchedule);
                _db.SaveChanges();
            }

            //recreate model
            EventScheduleViewModel viewModel = new EventScheduleViewModel();
            viewModel.Event = _db.Events.Find(eventId);
            viewModel.Client = _db.Clients.Find(viewModel.Event.ClientId);
            viewModel.EmployeeSchedule = new ScheduleDisplayDetails();

            viewModel.EmployeeSchedule.StartTime = viewModel.Event.StartTime;
            viewModel.EmployeeSchedule.EndTime = viewModel.Event.EndTime;
            viewModel.EmployeeList = _db.Employees.ToList();

            return RedirectToAction("AddSchedule", new { id = eventId });
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

        public bool IsScheduleConflictRemoved(int id, DateTime start)
        {
            var selectedEvent= _db.Events.Find(id);
            if (start.Date != selectedEvent.StartTime.Date)
            {
                var eventSchedules = _db.EventSchedules.Where(e => e.EventId == id).ToList();
                foreach (var eventSchedule in eventSchedules)
                {
                    Schedule employeeSchedule = _db.Schedules.Where(s => s.Id == eventSchedule.ScheduleId).FirstOrDefault();
                    _db.Schedules.Remove(employeeSchedule);
                    _db.EventSchedules.Remove(eventSchedule);
                }
            _db.SaveChanges();
            }
            return true;
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

//To test for ScheduleEmployee











