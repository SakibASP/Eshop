using Eshop.Models.BusinessDomains;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table("ProductImages")]
    public class ProductImages
    {
        [Key]
        public int AutoId { get; set; }
        public int? ProductID { get; set; }
        public int? IsCover { get; set; }
        public string? ImageName { get; set; }
        public string? ImagePath  { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set;}

        [ForeignKey("ProductID")]
        public virtual Product? Product_ { get; set; }
    }
}
