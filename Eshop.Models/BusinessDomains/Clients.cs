using Eshop.Models.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table("Clients")]
    public class Clients : ModelBase
    {
        [Required] 
        [DisplayName("Type")]
        public int? TypeId { get; set; }
        [Required]
        [DisplayName("Name")]
        public string? ClientName { get; set; }
        [Required]
        [DisplayName("Code")]
        public string? ClientCode { get; set; }
        [Required]
        [DisplayName("Address")]
        public string? ClientAddr { get; set; }
        [Required]
        [DisplayName("Mobile")]
        public string? ClientPhone { get; set; }
        [DisplayName("Secondary Mobile")]
        public string? SecondaryPhone { get; set; }
        [ForeignKey(nameof(TypeId))]
        public virtual ClientTypes? ClientTypes { get; set; }
    }
}
