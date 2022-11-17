using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JSarad_C868_Capstone.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required, StringLength(50)]
        public string Name { get; set; }
        
        [Required, Phone, StringLength(15)]
        public string Phone { get; set; }
        
        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }
        
        [Required, StringLength(200)]
        public string Address { get; set; }
        
        public DateTime LastUpdate { get; set; } = DateTime.Now;
    }
}
