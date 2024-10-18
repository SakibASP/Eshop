using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.Menu
{
    [Table("MenuToRole")]
    public partial class MenuToRole
    {
        [Key]
        public int? Id { get; set; }
        public string RoleId { get; set; }
        public Nullable<int> MenuId { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<bool> IsSelected { get; set; }
    }
}

