using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.Models;
using JSarad_C868_Capstone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace JSarad_C868_Capstone.Controllers
{
    [Authorize]
    public class ScheduleEmployees : Controller
    {
        private readonly AppDbContext _db;
        public ScheduleEmployees(AppDbContext db)
        {
            _db = db;

        }
        public IActionResult Index(int? id)
        {
            EventScheduleViewModel viewModel = new EventScheduleViewModel();
            viewModel.Event = _db.Events.Find(id);
            viewModel.Client = _db.Clients.Find(viewModel.Event.ClientId);
            viewModel.EmployeeList = _db.Employees.ToList();
            viewModel.EmployeeSchedule = new ScheduleDisplayDetails();
            viewModel.EmployeeSchedule.StartTime = viewModel.Event.StartTime;
            viewModel.EmployeeSchedule.EndTime = viewModel.Event.EndTime;
            //selectedEvent = viewModel.Event;
            //viewModel.Includes = Included(viewModel.Event);
            
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
            return View(viewModel);
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

                if (idsInEvent.Count > 0)
                {
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

                foreach (string message in employeeOvertime)
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
            try
            {
                Schedule selectedSchedule = _db.Schedules.Find(id);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            //EventSchedule eventSchedule = _db.EventSchedules.Where(es => es.ScheduleId == id).FirstOrDefault();
            //var eventId = eventSchedule.EventId;
            //if (selectedSchedule != null)
            //{
            //    _db.Schedules.Remove(selectedSchedule);
            //    _db.EventSchedules.Remove(eventSchedule);
            //    _db.SaveChanges();
            //}

            //recreate model
            EventScheduleViewModel viewModel = new EventScheduleViewModel();
            //viewModel.Event = selectedEvent;
            viewModel.Client = _db.Clients.Find(viewModel.Event.ClientId);
            viewModel.EmployeeSchedule = new ScheduleDisplayDetails();

            viewModel.EmployeeSchedule.StartTime = viewModel.Event.StartTime;
            viewModel.EmployeeSchedule.EndTime = viewModel.Event.EndTime;
            viewModel.EmployeeList = _db.Employees.ToList();

            return RedirectToAction("AddSchedule");
        }
    }
}
