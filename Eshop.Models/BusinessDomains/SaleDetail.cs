using Eshop.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table("SALE_DETAIL")]
    public class SaleDetail : ModelBase
    {
        public int? SaleMasterId { get; set; }

        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }

        public int Quantity { get; set; } = 0;

        public double Rate { get; set; } = 0;

        public double Amount { get { return Quantity * Rate; } }

        public double Discount { get; set; } = 0;

        public double TotalAmount { get { return (Quantity * Rate)-Discount; } }
        [NotMapped]
        public string? UnitName { get; set; }
        [NotMapped]
        public int? CurrentStock { get; set; }

        [ForeignKey(nameof(SaleMasterId))]
        public virtual SaleMaster? SALE_MASTER { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }
    }
}
