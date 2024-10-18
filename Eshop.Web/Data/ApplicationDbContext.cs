using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Eshop.Web.Models;
using Eshop.Web.Models.ViewModels;
using Eshop.Models.Menu;
using Eshop.Models.BusinessDomains;
using Eshop.ViewModels.BusinessDomains;

namespace Eshop.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductViewModel>().HasNoKey();
        }

        //repositories
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<ProductImages> ProductImages { get; set; }
        public virtual DbSet<ShippingDetails> ShippingDetails { get; set; }
        public virtual DbSet<ShipmentOrders> ShipmentOrders { get; set; }

        //view models
        public virtual DbSet<ProductViewModel> ProductViewModel { get; set; }

        //menu items
        public virtual DbSet<DynamicMenuItem> DynamicMenuItem { get; set; }
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<MenuToRole> MenuToRole { get; set; }

    }
}