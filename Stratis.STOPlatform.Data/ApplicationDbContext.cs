using Microsoft.EntityFrameworkCore;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Data.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Stratis.STOPlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Document> Documents { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Address).IsUnique();
            modelBuilder.Entity<Deposit>().HasIndex(u => new { u.TransactionId, u.UserId }).IsUnique();
            modelBuilder.Entity<Document>().HasIndex(u => u.Key).IsUnique();
            InitialData.Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateDocuments();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateDocuments();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void UpdateDocuments()
        {
            foreach (var entry in ChangeTracker.Entries<Document>().Where(e => e.State == EntityState.Unchanged))
            {
                entry.Entity.Update();
            }
        }
    }
}
