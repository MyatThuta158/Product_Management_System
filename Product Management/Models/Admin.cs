using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models
{
    public class Admin:BaseUserClass
    {

        [Required]
        public string Role { get; set; }

        [Required]
        public string Department { get; set; }
    }
}
