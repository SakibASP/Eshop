namespace Eshop.ViewModels.Menu
{
    public class MenuItemViewModel
    {
        public MenuItemViewModel()
        {
            MenuSelections = new List<MenuSelection>();
        }
        //public int MenuId { get; set; }
        public string RoleId { get; set; }
        //public string MenuUrl { get; set; }
        //public Nullable<int> MenuParentId { get; set; }
        //public Nullable<bool> Active { get; set; }
        public IList<MenuSelection> MenuSelections { set; get; }
    }
}