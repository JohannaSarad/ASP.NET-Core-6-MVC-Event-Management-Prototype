using System.ComponentModel.DataAnnotations;

namespace JSarad_C868_Capstone.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Phone]
        public string Phone { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Availability { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
    }
}
