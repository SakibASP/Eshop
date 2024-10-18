using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table("ShipmentOrders")]
    public class ShipmentOrders
    {
        [Key]
        public int? AutoId { get; set; }

        public int? ShippingDetailsId { get; set; }
        public string? ProductName { get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("ShippingDetailsId")]
        public virtual ShippingDetails? _ShippingDetails  { get; set; }

    }
}
