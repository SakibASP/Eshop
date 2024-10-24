using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoId { get; set; }

        [Required(ErrorMessage = "Please enter a product name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Required]
        //[DisplayName("Sale Price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public double Price { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please specify a category")]
        public int? CategoryId { get; set; }
        [DisplayName("Unit")]
        public int? UnitId { get; set; }
        [DisplayName("Unique Identity")]
        public string? UniqueIdentity { get; set; }

        [NotMapped]
        public string? ImageName { get; set; }  
        [NotMapped]
        public string? ImagePath { get; set; }

        [NotMapped]
        [DisplayName("Stock")]
        public int CurrentStock { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        [ForeignKey("UnitId")]
        public virtual Units? Units { get; set; }
    }
}
