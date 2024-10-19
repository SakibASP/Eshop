using Eshop.Models.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table("PURCHASE_MASTER")]
    public class PurchaseMaster : ModelBase
    {
        [Required]
        [DisplayName("Supplier")]
        public int? ClientId { get; set; }

        [Required]
        [DisplayName("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }
        [Required]
        public string? PurchaseNo { get; set; }
        public string? Remarks { get; set; }
        public double TotalAmount { get; set; } = 0;
        public string? ChequeNo { get; set; }
       
        [DisplayName("Paid")]
        public double PaidAmount { get; set; } = 0;
        [DisplayName("Due")]
        public double DueAmount { get { return (TotalAmount - PaidAmount); } }
        [DisplayName("Payment Method")]
        public int? PaymentTypeId { get; set; }
        [DisplayName("Sub Payment Method")]
        public int? SubPaymentTypeId { get; set; }
        public bool IsActive { get; set; }
        [DisplayName("Bill of entry")]
        public string? BillNo { get; set; }
        [ForeignKey(nameof(ClientId))]
        public virtual Clients? Clients { get; set; }
        [ForeignKey(nameof(PaymentTypeId))]
        public virtual PaymentType? PaymentType { get; set; }
        [ForeignKey(nameof(SubPaymentTypeId))]
        public virtual PayementTypeSub? PayementTypeSub { get; set; }
    }
}
