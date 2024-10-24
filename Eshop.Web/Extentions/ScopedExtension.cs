using Eshop.Interfaces.Common;
using Eshop.Web.Repositories.Common;
using Eshop.Web.Services;

namespace Eshop.Web.Extentions
{
    public static class ScopedExtension
    {
        public static IServiceCollection AddAllScoped(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBraintreeService, BraintreeService>();
            serviceCollection.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddScoped<IMenuRepo, MenuRepo>();
            return serviceCollection;
        }
    }
}
