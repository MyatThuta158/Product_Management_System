using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Product_Management.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Management.Models
{
    public class Product
    {
        [Key]
        public int StockID { get; set; }

        [Required]
        public string StockNo { get; set; }

        [Required]
        public string StockName { get; set; }


        public string ProductImage { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        [NotMapped]
 
        public IFormFile ProductImgFile { get; set; }


        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public bool Active { get; set; } = true;
        public DateTime CreatedDate { get; set; }
    }
}
