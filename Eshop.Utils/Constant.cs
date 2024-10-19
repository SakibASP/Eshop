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
        public const string ImageFolderName = "ProductImages";
        public const string Notifications = "Notifications";
        public const string Admin = "Admin";
        public const string SuperAdmin = "SuperAdmin";
        public const string Error = "Error";
        public const string Success = "Success";
    }
    public static class SP
    {
        public const string GetProducts = "EXEC GetProducts @product_id=@product_id, @cat_id=@cat_id,@price=@price,@searchString=@searchString";
        public const string rawProducts = "SELECT * FROM PRODUCTS";
    }
}
