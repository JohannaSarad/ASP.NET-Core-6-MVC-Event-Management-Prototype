using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JSarad_C868_Capstone.Models
{
    public class EventSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Event")]
        public int EventId { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        
        public DateTime StartDate { get; set; }
        [BindProperty, DataType(DataType.Time), DisplayFormat(DataFormatString = "{HH:mm aa}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

    }
}
