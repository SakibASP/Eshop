using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Eshop.Web.Models;
using Eshop.Models.Menu;
using Eshop.Models.BusinessDomains;
using Eshop.ViewModels.BusinessDomains;
using Eshop.Models.Common;
using Eshop.Web.Common;

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

        //menu items
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<MenuToRole> MenuToRole { get; set; }

        public virtual DbSet<RequestCounts> RequestCounts { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<ClientTypes> ClientTypes { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<PaymentType> PaymentType { get; set; }
        public virtual DbSet<PayementTypeSub> PayementTypeSub { get; set; }
        public virtual DbSet<DamageMaster> DamageMaster { get; set; }
        public virtual DbSet<DamageDetail> DamageDetail { get; set; }
        public virtual DbSet<SaleMaster> SaleMaster { get; set; }
        public virtual DbSet<SaleDetail> SaleDetail { get; set; }
        public virtual DbSet<PurchaseMaster> PurchaseMaster { get; set; }
        public virtual DbSet<PurchaseDetail> PurchaseDetail { get; set; }

        //For storing users actions in Audit Table
        public virtual DbSet<Audit> Audit { get; set; }
        private AuditTrailFactory? auditFactory = null;
        private readonly List<Audit> auditList = [];
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            auditList.Clear();
            auditFactory = new AuditTrailFactory(this);

            var entityList = ChangeTracker.Entries().Where((p => p.State == EntityState.Deleted || p.State == EntityState.Modified));  //p.State == EntityState.Added || for insert statement
            foreach (var entity in entityList)
            {
                Audit? audit = await auditFactory.GetAudit(entity);
                bool isValid = true;

                var tableName = audit?.TableName ?? string.Empty;
                if (audit is null || string.IsNullOrEmpty(tableName))
                {
                    isValid = false;
                }
                else if (entity.State == EntityState.Modified && string.IsNullOrWhiteSpace(audit?.NewData) && string.IsNullOrWhiteSpace(audit?.OldData))
                {
                    isValid = false;
                }
                else if (tableName.Contains("RequestCount", StringComparison.CurrentCultureIgnoreCase) || tableName.Contains("AspNet", StringComparison.CurrentCultureIgnoreCase))
                {
                    isValid = false;
                }

                if (isValid)
                {
                    auditList.Add(audit!);
                }
            }

            var retVal = await base.SaveChangesAsync(cancellationToken);
            if (auditList.Count > 0)
            {
                await Audit.AddRangeAsync(auditList, cancellationToken);
                await base.SaveChangesAsync(cancellationToken);
            }

            return retVal;
        }

    }
}