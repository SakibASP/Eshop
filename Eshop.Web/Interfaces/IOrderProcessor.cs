using Eshop.Models.BusinessDomains;
using Eshop.Web.Models;

namespace Eshop.Web.Interfaces
{
    public interface IOrderProcessor
    {
        Task ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
