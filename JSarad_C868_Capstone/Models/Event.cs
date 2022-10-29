using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required]
        //[Display(Name = "Start Date")]
        public DateTime EventStart { get; set; }
        [Required]
        public DateTime EventEnd { get; set; }
        public bool Food { get; set; }
        public bool Bar { get; set; }
        public int Guests { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;

    }
}
