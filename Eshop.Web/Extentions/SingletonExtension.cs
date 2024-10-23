using Eshop.Interfaces;

namespace Eshop.Web.Extentions
{
    public static class SingletonExtension
    {
        public static IServiceCollection AddAllSingleton(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return serviceCollection;
        }
    }
}
