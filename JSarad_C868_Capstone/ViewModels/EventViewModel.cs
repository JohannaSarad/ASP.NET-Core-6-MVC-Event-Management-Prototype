using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JSarad_C868_Capstone.ViewModels
{
    public class EventViewModel 
    {
        public string? Title { get; set; }
        public string? Notice { get; set; }
        public Event Event { get; set; }
        [Required]
        public string ClientName { get; set; }
    }
}
       