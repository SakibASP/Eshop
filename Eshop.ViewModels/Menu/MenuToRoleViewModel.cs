namespace Eshop.ViewModels.Menu
{
    public class MenuToRoleViewModel
    {
       // public List<int> MenuParentIds { set; get; }
        public List<MenuSelection> MenuSelections { set; get; }
        public string RoleId { set; get; }

        public List<int> MenuParentIds { get; internal set; }
        public MenuToRoleViewModel()
        {
            MenuSelections = new List<MenuSelection>();
        }
    }
}