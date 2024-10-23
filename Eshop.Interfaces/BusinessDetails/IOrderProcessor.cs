using Eshop.Models.BusinessDomains;
using Eshop.ViewModels.BusinessDomains;

namespace Eshop.Interfaces
{
    public interface IOrderProcessor
    {
        Task ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
