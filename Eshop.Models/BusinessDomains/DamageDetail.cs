using Eshop.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table(nameof(DamageDetail))]
    public class DamageDetail : ModelBase
    {
        public int? DamageMasterId { get; set; }

        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }

        public int Quantity { get; set; }

        [NotMapped]
        public string? UnitName { get; set; }
        [NotMapped]
        public int? CurrentStock { get; set; }
        [ForeignKey(nameof(DamageMasterId))]
        public virtual DamageMaster? DAMAGE_MASTER { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }
        
    }
}
