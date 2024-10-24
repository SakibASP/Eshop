using Eshop.Interfaces.Common;
using Eshop.Models.Menu;
using Eshop.Web.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Web.Repositories.Common
{
    public class MenuRepo(ApplicationDbContext context) : IMenuRepo
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IList<DynamicMenuItem>> GetAllMenuAsync(string? userId)
        {
            return await _context.Database.SqlQueryRaw<DynamicMenuItem>("exec usp_GetMenuData @UserId", new SqlParameter("UserId", userId)).ToListAsync();
        }
    }
}
