using JSarad_C868_Capstone.Models;

namespace JSarad_C868_Capstone.ViewModels
{
    public class EventViewModel
    {
        public Event Event { get; set; }
        public Client Client { get; set; }
        public List<Employee> EmployeeList { get; set; }
        
    }
}
