using Microsoft.EntityFrameworkCore;
using models;

namespace data;

public class CarAppContext : DbContext
{
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Ownership> Ownerships { get; set; }

    public CarAppContext(DbContextOptions<CarAppContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Brand>().ToTable("Brands");
        modelBuilder.Entity<Owner>().ToTable("Owners");

        modelBuilder.Entity<Model>().ToTable("Models");
        modelBuilder.Entity<Model>().HasOne<Brand>().WithMany().HasForeignKey(c => c.BrandId);

        modelBuilder.Entity<Car>().ToTable("Cars");
        modelBuilder.Entity<Car>().HasOne(c => c.Brand).WithMany().HasForeignKey(c => c.BrandId);
        modelBuilder.Entity<Car>().HasOne(c => c.Model).WithMany().HasForeignKey(c => c.ModelId);

        modelBuilder.Entity<Category>().ToTable("Categories");

        modelBuilder.Entity<Ownership>().ToTable("Ownerships");
        modelBuilder.Entity<Ownership>().HasOne(o => o.Car).WithOne().HasForeignKey<Ownership>(o => o.CarId);
        modelBuilder.Entity<Ownership>().HasOne(o => o.Owner).WithMany().HasForeignKey(o => o.OwnerId);
    }
}
