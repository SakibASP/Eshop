using Eshop.Models.Menu;

namespace Eshop.Interfaces.Common
{
    public interface IMenuRepo
    {
        /// <summary>
        /// Getting menu list by userid from database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IList<DynamicMenuItem>> GetAllMenuAsync(string? userId);
    }
}
