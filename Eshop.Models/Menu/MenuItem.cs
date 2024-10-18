using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.Menu
{

    [Table("MenuItem")]
    public partial class MenuItem
    {
        [Key]
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public Nullable<int> MenuParentId { get; set; }
        public Nullable<bool> Active { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
        [NotMapped]
        public virtual List<MenuItem> Children { get; set; }
    }
}

