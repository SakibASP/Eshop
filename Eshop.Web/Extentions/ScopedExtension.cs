using Eshop.Interfaces;
using Eshop.Web.Services;

namespace Eshop.Web.Extentions
{
    public static class ScopedExtension
    {
        public static IServiceCollection AddAllScoped(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBraintreeService, BraintreeService>();
            serviceCollection.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            return serviceCollection;
        }
    }
}
