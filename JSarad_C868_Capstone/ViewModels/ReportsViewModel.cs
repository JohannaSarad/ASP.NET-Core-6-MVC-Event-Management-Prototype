using JSarad_C868_Capstone.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace JSarad_C868_Capstone.ViewModels
{
    public class ReportsViewModel
    {
        public List<Event> EventList { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SelectedUser { get; set; }
       
        public List<SelectListItem> UserSelectList { get; set; }
        public bool OrderByPlanner { get; set; } = false;
        public bool OrderByDates { get; set; } = false;
        
    }
}
