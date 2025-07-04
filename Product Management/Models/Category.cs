using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryNo { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public bool Active { get; set; } = true;

        public DateTime CreatedDate { get; set; }

       
    }
}
