using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.Models;
using Microsoft.AspNetCore.Mvc;


namespace JSarad_C868_Capstone.ViewModels
{
    public class EventViewModel 
    {
        [BindProperty]
        public Event? Event { get; set; }
        [BindProperty]
        public Client? Client { get; set; }
        [BindProperty]
        public Employee? SelectedEmployee { get; set; }
        //public DateTime StartTime { get; set; }
        //public DateTime EndTime { get; set; }
        [BindProperty]
        public EmployeeSchedule? EmployeeSchedule { get; set; }
        [BindProperty]
        public List<Employee>? EmployeeList { get; set; }
        [BindProperty]
        public List<EmployeeSchedule>? Schedules { get; set; } 

    }
    
}