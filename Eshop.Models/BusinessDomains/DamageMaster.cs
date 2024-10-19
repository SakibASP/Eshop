using Eshop.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table(nameof(DamageMaster))]
    public class DamageMaster : ModelBase
    {
        [Required]
        [DisplayName("Damage Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DamageDate { get; set; }
        [Required]
        public string? DamageNo { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }
}
