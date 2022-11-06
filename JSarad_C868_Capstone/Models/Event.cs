using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JSarad_C868_Capstone.Controllers;

namespace JSarad_C868_Capstone.Models
{
    public class Event
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Location { get; set; }

        //[Required]
        //[Remote("IsStartTimeFirst", "Event", AdditionalFields="EndTime", ErrorMessage ="Start Time must be before End Time")]

        public DateTime StartTime { get; set; }
       
        public DateTime EndTime { get; set; }

        public bool Food { get; set; }
        public bool Bar { get; set; }
        public int Guests { get; set; }
        public string? Notes { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public DateTime LastUpdate { get; set; } = DateTime.Now;
        //string 
        //string sql = "Insert into Capstone (EventName, EventType, StartDate, EndDate) Values ('{EventName}', '{EventType}', 
        
    }
}
