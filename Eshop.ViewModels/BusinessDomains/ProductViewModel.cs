using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Eshop.ViewModels.BusinessDomains
{
    public class ProductViewModel
    {
        public int? ProductID { get; set; }
        public int? ProductImageID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public double Price { get; set; }

        [DisplayName("Name")]
        public string? ImageName { get; set; }

        [DisplayName("Image")]
        public string? ImagePath { get; set; }
        public string? CreatedBy { get; set; }
        public string? ShortDesc { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? CurrentStock { get; set; }

        [DisplayName("Category")]
        public int? Cat_Id { get; set; }
        public string? Category { get; set; }
        public int? IsCover { get; set; }
        public bool IsAvailabe { get; set; }
    }
}
