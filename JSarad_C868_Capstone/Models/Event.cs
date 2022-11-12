using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace JSarad_C868_Capstone.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]

        public string EventName { get; set; }
        public string Type { get; set; }
        [Required]
        public string Location { get; set; }

        //[Required]
        //[Remote("IsStartTimeFirst", "Event", AdditionalFields="EndTime", ErrorMessage ="Start Time must be before End Time")]
        //[DataType(DataType.Date)]
        public DateTime EventDate { get; set; }
        //[DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        //[DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        public bool Food { get; set; }
        public bool Bar { get; set; }
        public int Guests { get; set; }
        public string? Notes { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; } // changes when added

        public DateTime LastUpdate { get; set; } = DateTime.Now; //changes when updated
        //string 
        //string sql = "Insert into Capstone (EventName, EventType, StartDate, EndDate) Values ('{EventName}', '{EventType}', 
        
    }
}
