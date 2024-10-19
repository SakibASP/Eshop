using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Eshop.Web.Binder;
using Eshop.Utils;
using Eshop.Web.Data;
using Eshop.Web.Interfaces;
using Eshop.Web.Models;
using Eshop.Web.Services;
using System.Security.Claims;
using Eshop.Web.Repositories;
using Microsoft.AspNetCore.Mvc.Razor;
using Serilog.Events;
using Serilog;

//Creating Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("AspNetCore", LogEventLevel.Warning)
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
        .WriteTo.File("Logs/InfoLogs/logs-.txt", rollingInterval: RollingInterval.Day)
    )
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
        .WriteTo.File("Logs/FatalLogs/logs-.txt", rollingInterval: RollingInterval.Day)
    )
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
        .WriteTo.File("Logs/ErrorLogs/logs-.txt", rollingInterval: RollingInterval.Day)
    )
    .Filter.ByExcluding(e => e.Properties.ContainsKey("SourceContext") &&
                              (e.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore")
                              || e.MessageTemplate.Text.Contains("HTTP")
    ))
    .CreateLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();


    builder.Services.Configure<IdentityOptions>(options =>
        options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
    builder.Services.AddControllersWithViews();
    builder.Services.AddControllersWithViews().AddNewtonsoftJson();

    //Mail shipping
    EmailSettings EmailSettings = new EmailSettings
    {
        WriteAsFile = bool.Parse(builder.Configuration.GetSection("AppSettings:Email.WriteAsFile").Value ?? "false")
    };
    //builder.Services.AddTransient<IOrderProcessor, EmailOrderProcessor>();
    builder.Services.AddTransient<IOrderProcessor>(provider =>
         new EmailOrderProcessor(EmailSettings));

    //Payment Services
    builder.Services.AddTransient<IBraintreeService, BraintreeService>();

    //Model Binding
    builder.Services.AddMvc(o =>
    {
        // adds custom binder at first place
        o.ModelBinderProviders.Insert(0, new CartModelBinderProvider());
    }).AddRazorRuntimeCompilation();
    builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

    builder.Services.AddSession(options =>
    {
        options.Cookie.Name = ".eshop.Web.Session";
        options.IdleTimeout = TimeSpan.FromMinutes(5);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });
    builder.Services.AddDistributedMemoryCache();

    builder.Services.Configure<RazorViewEngineOptions>(o =>
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

    var app = builder.Build();

    // Configuring User Roles
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);
            await ContextSeed.SeedRolesAsync(userManager, roleManager);
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseMigrationsEndPoint();
    //}
    //else
    //{
    //    app.UseExceptionHandler("/Home/Error");
    //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //    app.UseHsts();
    //}

    app.UseDeveloperExceptionPage();
    //app.UseDatabaseErrorPage();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    //enable session before MVC
    app.UseSession();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}