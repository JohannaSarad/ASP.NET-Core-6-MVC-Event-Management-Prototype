﻿using JSarad_C868_Capstone.Data;
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
            viewModel.Event.StartTime = viewModel.Event.EventDate.Date + viewModel.Event.StartTime.TimeOfDay;
            viewModel.Event.EndTime = viewModel.Event.EventDate.Date + viewModel.Event.EndTime.TimeOfDay;
           
            if (viewModel.Event.StartTime < viewModel.Event.EndTime)
            {
                TimeSpan open = new TimeSpan(06, 00, 00);
                TimeSpan close = new TimeSpan(23, 00, 00);

                if ((viewModel.Event.EndTime.TimeOfDay < close) && (viewModel.Event.StartTime.TimeOfDay > open))
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
                            //check if date was changed and if there was an employee schedule associated 
                            var eventSchedule = _db.EventSchedules.Where(es => es.EventId == viewModel.Event.Id);
                            if (eventSchedule.Any())
                            {
                                var oldStart = _db.Events.Find(viewModel.Event.Id).StartTime;
                                var oldEnd = _db.Events.Find(viewModel.Event.Id).EndTime;
                                if ((oldStart != viewModel.Event.StartTime) || (oldEnd != viewModel.Event.EndTime))
                                {
                                    //remove eventschedules with event id and schedules where 
                                    //List<Schedule> employeeSchedules = new List<Schedule>();
                                    foreach (EventSchedule schedule in eventSchedule)
                                    {
                                        var scheduleToRemove = _db.Schedules.Find(schedule.ScheduleId);
                                        _db.Schedules.Remove(scheduleToRemove);
                                        _db.EventSchedules.Remove(schedule);
                                    }
                                    _db.SaveChanges();
                                }
                            }

                            _db.Events.Update(viewModel.Event);
                            _db.SaveChanges();
                        }
                        return Ok(true);
                    }
                    //return Ok(false);
                }
                ModelState.AddModelError("Event.EndTime", "* Events must be scheduled during hours of operation between 6:00 am and 11:00 pm");
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
            //viewModel.Includes = Included(viewModel.Event);
            
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
            if ((viewModel.Event.EndTime.TimeOfDay < close) && (viewModel.Event.StartTime.TimeOfDay > open))
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
            
            
            ScheduleDisplayDetails employeeSchedule = new ScheduleDisplayDetails()
            {
                EmployeeId = viewModel.EmployeeSchedule.EmployeeId,
                EmployeeName = viewModel.EmployeeSchedule.EmployeeName,
                StartTime = viewModel.EmployeeSchedule.StartTime,
                EndTime = viewModel.EmployeeSchedule.EndTime
            };

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
                foreach (ScheduleDisplayDetails scheduleDetails in viewModel.Schedules)
                {
                    //Employee employee = _db.Employees.Find(scheduleDetails.EmployeeId);
                    if (schedules.Any()) {
                        int shiftLength = 0;
                        int weeklyHours = 0;
                        foreach (Schedule schedule in schedules)
                        {
                            if ((start.Date == schedule.StartTime.Date) && (schedule.EmployeeId == scheduleDetails.EmployeeId))
                            {
                                shiftLength += (schedule.EndTime.TimeOfDay.Hours - schedule.StartTime.TimeOfDay.Hours);
                            }

                            //calculate hours in the week of the event
                            if ((schedule.StartTime >= weekStartDate && schedule.EndTime <= weekEndDate) && (schedule.EmployeeId == scheduleDetails.EmployeeId))
                            {
                                weeklyHours += (schedule.EndTime.TimeOfDay.Hours - schedule.StartTime.TimeOfDay.Hours);
                            }
                        }

                        if (shiftLength > 8 || weeklyHours > 40)
                        {
                            string ot = ($"{scheduleDetails.EmployeeName} is scheduled for");
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
                }
                foreach(string message in employeeOvertime)
                {
                    overTimeMessage += message + "\n";
                }
                TempData["Error"] = ($"California State Law requires employees recieve overtime at a rate of time and a half for any hours in excess " +
                                     $"of 40 per week or 8 per day, and a rate of double time for any hours in excess of 12 per day.") + "\n" + overTimeMessage;
                return View("AddSchedule", viewModel);
            }
            TempData["Error"] = "No overtime hours were found for this event";
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

//To test for ScheduleEmployee











