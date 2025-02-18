using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<StatusTypes> StatusTypes { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<StatusTypes>()
                .HasData(
                    new StatusTypes { Id = 1, StatusName = "Not started" },
                    new StatusTypes { Id = 2, StatusName = "In Progress" },
                    new StatusTypes { Id = 3, StatusName = "Completed" }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
