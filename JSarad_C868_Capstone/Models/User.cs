using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JSarad_C868_Capstone.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        [Required, StringLength(70), DisplayName("User Name")]
        public string Username { get; set; }
        
        [Required, StringLength(40)]
        public string Password { get; set; }
    }
}
