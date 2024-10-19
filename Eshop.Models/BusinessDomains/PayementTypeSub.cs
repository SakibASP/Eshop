using Eshop.Models.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table(nameof(PayementTypeSub))]
    public class PayementTypeSub : ModelBase
    {
        [Required]
        [DisplayName("Payment Type")]
        public int? TypeId { get; set; }
        [Required]
        [DisplayName("Sub Type Name")]
        public string? TypeName { get; set; }
        [DisplayName("Account Number")]
        public string? AccountNumber { get; set; }
        [ForeignKey(nameof(TypeId))]
        public virtual PaymentType? PaymentType { get; set;}
    }
}
