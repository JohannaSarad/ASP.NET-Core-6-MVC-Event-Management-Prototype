using JSarad_C868_Capstone.Models;

namespace JSarad_C868_Capstone.ViewModels
{
    public class EventScheduleViewModel
    {
        //public EventSchedule EventSchedule { get; set; }

        public Event Event { get; set; }
        public Client Client { get; set; }
        public Employee Employee { get; set; }
        public List<Employee> EmployeeList { get; set; }
        public ScheduleDisplayDetails EmployeeSchedule { get; set; }
        public List<ScheduleDisplayDetails> Schedules { get; set; }
    }
}
