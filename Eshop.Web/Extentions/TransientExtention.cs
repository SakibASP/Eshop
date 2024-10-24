using Eshop.Interfaces;
using Eshop.Interfaces.BusinessDetails;
using Eshop.Utils;
using Eshop.Web.Repositories.BusinessDomains;

namespace Eshop.Web.Extentions
{
    public static class TransientExtention
    {
        public static IServiceCollection AddAllTransients(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //Mail shipping
            EmailSettings EmailSettings = new()
            {
                WriteAsFile = bool.Parse(configuration.GetSection("AppSettings:Email.WriteAsFile").Value ?? "false")
            };
            serviceCollection.AddScoped<IOrderProcessor>(provider =>
                new EmailOrderProcessor(EmailSettings, provider.GetRequiredService<IWebHostEnvironment>()));
            return serviceCollection;
        }

    }
}
