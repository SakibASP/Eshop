using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.Menu
{
    [Table("DynamicMenuItem")]
    public class DynamicMenuItem
    {
        public int? MID { get; set; }
        public string? MenuName { get; set; }
        public string? MenuURL { get; set; }
        public int? MenuParentID { get; set; }
        public string? FaIcon { get; set; }
    }
}
