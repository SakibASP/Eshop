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

        [DisplayName("Stock")]
        [Range(0,50, ErrorMessage = "Please enter between 1 to 50")]
        public int CurrentStock { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please specify a category")]
        public int? Cat_Id { get; set; }
        public bool IsAvailabe { get; set; }

        [NotMapped]
        public string? ImageName { get; set; }  
        [NotMapped]
        public string? ImagePath { get; set; }

        [NotMapped]
        public double TotalPrice { get { return (CurrentStock == 0 ? 1 : CurrentStock) * Price; } }

        [ForeignKey("Cat_Id")]
        [DisplayName("Category")]
        public virtual Category? Category1 { get; set; }


    }
}
