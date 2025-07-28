using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.ViewModel
{
    public class CustomerViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password do not match.")]
        public string ConfirmPassword { get; set; }


        [Required]
        public string Address { get; set; }


        [Required]
        public string City { get; set; }

    }
}
