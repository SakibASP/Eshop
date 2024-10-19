using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int AutoId { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please specify a category")]
        public string? CategoryName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
