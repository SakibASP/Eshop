namespace Eshop.Utils
{
    public static class Constant
    {
        public const string CART = "Cart";
        public const string CartTotal = "CartSummary";
        public const string SessionTest = "MySession";
        public const string Menu = "_Menu";

        public const string PRODUCTS_LIST = "_PRODUCTS_LIST";
        public const string TOTAL_PRODUCTS_LIST = "_TOTAL_PRODUCTS_LIST";
        public const string SOCCER = "_SOCCER";
        public const string WATERSPORTS = "_WATERSPORTS";
        public const string CHESS = "_CHESS";
        public const string CRICKET = "_CRICKET";
        public const string PENDING_ORDERS = "_PENDING_ORDERS";
        public const string Notifications = "Notifications";

        //redirectio  link
        public const string LoginPage = "/Identity/Account/Login";

        //Roles
        public const string Admin = "Admin";
        public const string SuperAdmin = "SuperAdmin";

        //Messages
        public const string Error = "Error";
        public const string ErrorMessage = "Failed! Something went wrong!";
        public const string Success = "Success";
        public const string SuccessMessage = "Saved successfully!";

        //Folder names
        public const string ImageFolderName = "ProductImages";
        public const string EmailFolderName = "Emails";
    }
    public static class SP
    {
        public const string GetProducts = "EXEC GetProducts @ProductId=@ProductId, @CatId=@CatId,@Price=@Price,@SearchString=@SearchString";
    }
}
