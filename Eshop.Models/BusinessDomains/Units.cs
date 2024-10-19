using Eshop.Models.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table(nameof(Units))]
    public class Units : ModelBase
    {
        [Required]
        [DisplayName("Unit Name")]
        public string? UnitName { get; set; }
    }
}
