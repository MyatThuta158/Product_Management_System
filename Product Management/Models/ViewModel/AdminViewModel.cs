using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.ViewModel
{
    public class AdminViewModel
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


        [Compare("Password", ErrorMessage = "The confirm password and actual password do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Department { get; set; }
    }
}
