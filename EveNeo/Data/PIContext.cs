using EveNeo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EveNeo.Data
{
    public class PIContext : DbContext
    {
        public PIContext(DbContextOptions<PIContext> options, IConfiguration configuration) : base(options)
        {
        }

        public IConfiguration Configuration { get; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Schematic> Schematics { get; set; }
        public DbSet<SchematicTypeMap> SchematicTypeMaps { get; set; }
        public DbSet<PlanetResourceMap> PlanetResourceMaps { get; set; }

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
                entity.Property(e => e.Capacity).HasColumnType("System.Double");
            });
            
            modelBuilder.Entity<Schematic>().ToTable("Schematic");

            modelBuilder.Entity<SchematicTypeMap>().ToTable("SchematicTypeMap");
            modelBuilder.Entity<SchematicTypeMap>().HasKey(e => new { e.SchematicID, e.TypeID });

            modelBuilder.Entity<PlanetResourceMap>().ToTable("PlanetResourceMap");
            modelBuilder.Entity<PlanetResourceMap>().HasKey(e => new { e.PlanetTypeID, e.ResourceTypeID });
        }
    }
}
