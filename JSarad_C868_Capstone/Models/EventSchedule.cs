using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JSarad_C868_Capstone.Models
{
    public class EventSchedule
    {
        public int EventId { get; set; }
       
        public int ScheduleId { get; set; }
    }
}
