using Eshop.Models.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table("SALE_MASTER")]
    public class SaleMaster : ModelBase
    {
        [Required]
        [DisplayName("Dealer")]
        public int? ClientId { get; set; }

        [Required]
        [DisplayName("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SaleDate { get; set; }

        [Required]
        [DisplayName("Invoice")]
        public string? InvoiceNo { get; set; }
        public string? Remarks { get; set; }
        public double TotalAmount { get; set; } = 0;

        [DisplayName("Discount")]
        public double TotalDiscount { get; set; } = 0;
        public double NetAmount { get { return TotalAmount - TotalDiscount; } }

        [DisplayName("Paid")]
        public double PaidAmount { get; set; } = 0;

        [DisplayName("Due")]
        public double DueAmount { get { return (TotalAmount - TotalDiscount) - PaidAmount; } }
        public string? ChequeNo { get; set; }

        [DisplayName("Payment Method")]
        public int? PaymentTypeId { get; set; }

        [DisplayName("Sub Payment Method")]
        public int? SubPaymentTypeId { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public string? ClientName { get; set; }
        [NotMapped]
        public string? ClientAddr { get; set; }
        [NotMapped]
        public string? ClientPhone { get; set; }
        [NotMapped]
        public string? ClientSecondaryPhone { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual Clients? Clients { get; set; }

        [ForeignKey(nameof(PaymentTypeId))]
        public virtual PaymentType? PaymentType { get; set; }

        [ForeignKey(nameof(SubPaymentTypeId))]
        public virtual PayementTypeSub? PayementTypeSub { get; set; }

    }
}
