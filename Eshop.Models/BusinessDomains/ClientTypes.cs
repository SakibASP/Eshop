using Eshop.Models.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table("ClientTypes")]
    public class ClientTypes : ModelBase
    {
        [Required]
        [DisplayName("Type Name")]
        public string? TypeName { get; set; }
    }
}
