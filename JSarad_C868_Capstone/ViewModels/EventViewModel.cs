using JSarad_C868_Capstone.Models;
using Microsoft.AspNetCore.Mvc;

namespace JSarad_C868_Capstone.ViewModels
{
    public class EventViewModel
    {
        public Event Event { get; set; }
        public Client Client { get; set; }
        public Employee SelectedEmployee { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Employee> EmployeeList { get; set; }

    }
    
}