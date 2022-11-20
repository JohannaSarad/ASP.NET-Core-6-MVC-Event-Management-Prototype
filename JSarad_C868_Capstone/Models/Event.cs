using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace JSarad_C868_Capstone.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required, StringLength(100), DisplayName("Event Name")]
        public string EventName { get; set; }

        [Required,StringLength(50)]
        public string Type { get; set; }
        
        [Required, StringLength(200)]
        public string Location { get; set; }

        public DateTime EventDate { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        [DisplayName("Includes Food")]
        public bool Food { get; set; }
        
        [DisplayName("Includes Bar")]
        public bool Bar { get; set; }

        [DisplayName("Number of Guests")]
        public int Guests { get; set; }
        public string? Notes { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } // changes when added

        public DateTime LastUpdate { get; set; } = DateTime.Now; //changes when updated
        //string 
        //string sql = "Insert into Capstone (EventName, EventType, StartDate, EndDate) Values ('{EventName}', '{EventType}', 
        
    }
}
