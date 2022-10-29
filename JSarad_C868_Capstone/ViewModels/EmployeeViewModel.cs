using JSarad_C868_Capstone.Models;
using Microsoft.Build.Framework;

namespace JSarad_C868_Capstone.ViewModels
{
    public class EmployeeViewModel
    {
        public bool Monday { get; set; } = false;
        public bool Tuesday { get; set; } = false;
        public bool Wednesday { get; set; } = false;
        public bool Thursday { get; set; } = false;
        public bool Friday { get; set; } = false;
        public bool Saturday { get; set; } = false;
        public bool Sunday { get; set; } = false;
        
        public Employee Employee { get; set; } 
    }
}
