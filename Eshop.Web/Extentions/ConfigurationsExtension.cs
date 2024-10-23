using Eshop.Web.Binder;
using Eshop.Web.Data;
using Eshop.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Claims;

namespace Eshop.Web.Extentions
{
    public static class ConfigurationsExtension
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection)
        {

            serviceCollection.Configure<IdentityOptions>(options =>
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);
            serviceCollection.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultUI()
                        .AddDefaultTokenProviders();
            serviceCollection.AddControllersWithViews();
            serviceCollection.AddControllersWithViews().AddNewtonsoftJson();

            //Model Binding
            serviceCollection.AddMvc(o =>
            {
                // adds custom binder at first place
                o.ModelBinderProviders.Insert(0, new CartModelBinderProvider());
            }).AddRazorRuntimeCompilation();

            serviceCollection.AddSession(options =>
            {
                options.Cookie.Name = ".eshop.Web.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            serviceCollection.AddDistributedMemoryCache();

            serviceCollection.Configure<RazorViewEngineOptions>(o =>
            {
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("~/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("~/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("~/Views/Common/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("~/Views/Users/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("~/Views/BusinessDomains/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("~/Views/ApiIntegration/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("~/Views/Report/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("~/Views/BackgroundTask/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("~/Views/Menu/{1}/{0}" + RazorViewEngine.ViewExtension);
            });

            return serviceCollection;
        }
    }
}
