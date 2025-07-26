using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models
{
    public class Customer:BaseUserClass
    {
        [Required]
        public string Address { get; set; }


        [Required]
        public string City { get; set; }
    }
}
