using JSarad_C868_Capstone.Data;
using JSarad_C868_Capstone.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JSarad_C868_Capstone.ViewModels
{
    public class EventViewModel 
    {
        public Event Event { get; set; }
        public string ClientName { get; set; }
    }
}
       