using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Eshop.Web.Data;
using Eshop.Models.BusinessDomains;
using Eshop.ViewModels.BusinessDomains;
using Eshop.Utils;

namespace Eshop.Web.Common
{
    public static class Utility 
    {
        public static string TruncateDescription(string myString, int maxLength)
        {
            // If the string isn't null or empty
            if (!String.IsNullOrEmpty(myString))
            {
                // Return the appropriate string size
                return (myString.Length <= maxLength) ? myString : myString.Substring(0, maxLength) + "...";
            }
            else
            {
                // Otherwise return the empty string
                return string.Empty;
            }
        }
        public static async Task<List<ProductViewModel>> GetProducts(ApplicationDbContext db,int? p_id, int? CategoryId_, int? price_, string? searchString_)
        {
            var product_id = new SqlParameter("@ProductId", SqlDbType.Int)
            {
                Value = (object?)p_id ?? DBNull.Value
            };

            var CategoryId = new SqlParameter("@CatId", SqlDbType.Int)
            {
                Value = (object?)CategoryId_ ?? DBNull.Value
            };

            var price = new SqlParameter("@Price", SqlDbType.Int)
            {
                Value = (object?)price_ ?? DBNull.Value
            };

            var searchString = new SqlParameter("@SearchString", SqlDbType.VarChar)
            {
                Value = (object?)searchString_ ?? DBNull.Value
            };

            var @params = new[] { product_id, CategoryId, price, searchString };

            var productViewModel = await db.Database.SqlQueryRaw<ProductViewModel>(SP.GetProducts, @params).ToListAsync();
            return productViewModel;
        }
        
        public static async Task<List<Product>> GetTotalProducts(ApplicationDbContext db)
        {          
            var productList = await db.Products.Include(x=>x.Category).ToListAsync();

            return productList;
        }

        public static void GetProductStock(out decimal? SoccerPercentage, out decimal? WatersportsPercentage, out decimal? ChessPercentage, out decimal? CricketPercentage, ApplicationDbContext _context)
        {
            var totalCount = Convert.ToDecimal(_context.Products.Count());
            var soccerCount = Convert.ToDecimal(GetCategoryStock(_context, 1));
            var watersportsCount = Convert.ToDecimal(GetCategoryStock(_context, 2));
            var chessCount = Convert.ToDecimal(GetCategoryStock(_context, 3));
            var cricketCount = Convert.ToDecimal(GetCategoryStock(_context, 4));

            SoccerPercentage = (soccerCount / totalCount) * 100;
            WatersportsPercentage = (watersportsCount / totalCount) * 100;
            ChessPercentage = (chessCount / totalCount) * 100;
            CricketPercentage = (cricketCount / totalCount) * 100;
        }
        public static void GetPendingOrders(ApplicationDbContext _context, out int? order_count)
        {
            var Total_order_pending = _context.ShippingDetails.Where(x => x.IsConfirmed == false).ToList();
            if(Total_order_pending.Count > 0)
            {
                order_count = Total_order_pending.Count;
            }
            else
            {
                order_count = 0;
            }
        }

        public static int GetCategoryStock(ApplicationDbContext _context, int catId)
        {
            return _context.Database.SqlQuery<int>($"SELECT dbo.fnCurrentStockByCategory({catId}) as Value").FirstOrDefault();
        }
    }
}
