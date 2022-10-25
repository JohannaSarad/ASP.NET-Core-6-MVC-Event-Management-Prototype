using JSarad_C868_Capstone.Models;
using Microsoft.AspNetCore.Mvc;

namespace JSarad_C868_Capstone.ViewModels
{
    public class EmployeeListViewModel
    {
        [BindProperty]
        public int SelectedId { get; set; }
        [BindProperty]
        //public string SelectedName { get; set; }
        //[BindProperty]
        public IEnumerable<Employee>? EmployeeList { get; set; }
    }
}
