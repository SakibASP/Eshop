using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Models.Common
{
    [Table(nameof(RequestCounts))]
    public class RequestCounts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoId { get; set; }
        public int GetCount { get; set; }
        public int PostCount { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
