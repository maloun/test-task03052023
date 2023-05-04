using demo.Models.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace demo.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<HouseConsumersTable> HouseConsumers { get; set; }
        public DbSet<HouseConsumptionsTable> HouseConsumptions { get; set; }

        public DbSet<PlantsConsumersTable> PlantsConsumers { get; set; }
        public DbSet<PlantsConsumptionsTable> PlantsConsumptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HouseConsumptionsTable>()
                .HasOne(h => h.Consumer)
                .WithMany(h => h.Consumptions)
                .HasForeignKey(h => h.ConsumerId);

            modelBuilder.Entity<PlantsConsumptionsTable>()
                .HasOne(p => p.Consumer)
                .WithMany(p => p.Consumptions)
                .HasForeignKey(p => p.ConsumerId);
        }
    }
}
