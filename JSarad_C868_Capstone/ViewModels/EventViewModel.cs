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
    }
}