using EveNeo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EveNeo.Data
{
    public class UpdateContext : DbContext
    {
        public UpdateContext(DbContextOptions<UpdateContext> options, IConfiguration configuration) : base(options)
        {
        }

        public IConfiguration Configuration { get; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { 
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.ID).ValueGeneratedNever();
            });

            modelBuilder.Entity<Group>().ToTable("Group");
            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.ID).ValueGeneratedNever();
            });

            modelBuilder.Entity<Item>().ToTable("Item");
            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ID).ValueGeneratedNever();
            });
        }
    }
}
