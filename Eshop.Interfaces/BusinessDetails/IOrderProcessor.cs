using Eshop.Models.BusinessDomains;
using Eshop.ViewModels.BusinessDomains;

namespace Eshop.Interfaces.BusinessDetails
{
    public interface IOrderProcessor
    {
        Task ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
