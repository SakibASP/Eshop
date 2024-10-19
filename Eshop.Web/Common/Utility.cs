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
        public static async Task<List<ProductViewModel>> GetProducts(ApplicationDbContext db,int? p_id, int? cat_id_, int? price_, string? searchString_)
        {
            var product_id = new SqlParameter("@product_id", SqlDbType.Int)
            {
                Value = (object?)p_id ?? DBNull.Value
            };

            var cat_id = new SqlParameter("@cat_id", SqlDbType.Int)
            {
                Value = (object?)cat_id_ ?? DBNull.Value
            };

            var price = new SqlParameter("@price", SqlDbType.Int)
            {
                Value = (object?)price_ ?? DBNull.Value
            };

            var searchString = new SqlParameter("@searchString", SqlDbType.VarChar)
            {
                Value = (object?)searchString_ ?? DBNull.Value
            };

            var @params = new[] { product_id, cat_id, price, searchString };

            var productViewModel = await db.Database.SqlQueryRaw<ProductViewModel>(SP.GetProducts, @params).ToListAsync();
            return productViewModel;
        }
        
        public static async Task<List<Product>> GetTotalProducts(ApplicationDbContext db)
        {          
            var productList = await db.Products.Include(x=>x.Category1).ToListAsync();

            return productList;
        }

        public static void GetProductStock(out decimal? SoccerPercentage, out decimal? WatersportsPercentage, out decimal? ChessPercentage, out decimal? CricketPercentage, ApplicationDbContext _context)
        {
            var Products = Convert.ToDecimal(_context.Products.Count());
            var Soccer = Convert.ToDecimal(_context.Products.Where(p => p.Cat_Id == 1).Sum(x=>x.CurrentStock));
            var Watersports = Convert.ToDecimal(_context.Products.Where(p => p.Cat_Id == 2).Sum(x => x.CurrentStock));
            var Chess = Convert.ToDecimal(_context.Products.Where(p => p.Cat_Id == 3).Sum(x => x.CurrentStock));
            var Cricket = Convert.ToDecimal(_context.Products.Where(p => p.Cat_Id == 4).Sum(x => x.CurrentStock));

            SoccerPercentage = (Soccer / Products) * 100;
            WatersportsPercentage = (Watersports / Products) * 100;
            ChessPercentage = (Chess / Products) * 100;
            CricketPercentage = (Cricket / Products) * 100;
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

    }
}
