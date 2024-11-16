using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaContable.API.Database.Configuration;
using SistemaContable.API.Database.Entities;

namespace SistemaContable.API.Database
{
    public class LogsContext : DbContext
    {

        public LogsContext(
            DbContextOptions options
            ) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.HasDefaultSchema("security");

        //    modelBuilder.ApplyConfiguration(new LogsConfiguration());

        //}

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified
                ));

            foreach (var entry in entries)
            {
                var entity = entry.Entity as BaseEntity;
                if (entity != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = "f3055da8-3c3a-4926-8d40-0728800f9c9c";
                        entity.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        entity.UpdatedBy = "f3055da8-3c3a-4926-8d40-0728800f9c9c";
                        entity.UpdatedDate = DateTime.Now;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<LogsEntity> Logs { get; set; }

    }
}
