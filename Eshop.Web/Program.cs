using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Eshop.Web.Binder;
using Eshop.Utils;
using Eshop.Web.Data;
using Eshop.Interfaces;
using Eshop.Web.Models;
using Eshop.Web.Services;
using System.Security.Claims;
using Eshop.Web.Repositories;
using Microsoft.AspNetCore.Mvc.Razor;
using Serilog.Events;
using Serilog;
using Eshop.Web.Extentions;

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

    //Registering all extented DIs
    builder.Services.AddConfigurations();
    builder.Services.AddAllScoped();
    builder.Services.AddAllTransients(builder.Configuration);
    builder.Services.AddAllSingleton();

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