using System.ComponentModel.DataAnnotations;

namespace JSarad_C868_Capstone.Models
{
    public class Administrator
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
