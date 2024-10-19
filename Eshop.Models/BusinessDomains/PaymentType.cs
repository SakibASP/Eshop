using Eshop.Models.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table(nameof(PaymentType))]
    public class PaymentType : ModelBase
    {
        [Required]
        [DisplayName("Method Name")]
        public string PaymentMethod { get; set; }
    }
}
