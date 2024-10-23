using Eshop.ViewModels.BusinessDomains;

namespace Eshop.ViewModels.BusinessDomains
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
        public string? Nonce { get; set; }
    }
}