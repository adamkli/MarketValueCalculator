using MarketValueCalculator.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketValueCalculator.Data;
public class MarketValueContext(DbContextOptions<MarketValueContext> options) : DbContext(options)
{
    public DbSet<Price> Prices { get; set; }
    public DbSet<Position> Positions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Position>()
            .HasKey(p => new { p.Date, p.ProductKey });
        modelBuilder.Entity<Position>()
            .HasIndex(p => p.Date);
        modelBuilder.Entity<Position>()
            .HasData(SampleDataGenerator.GenerateSamplePositions());

        modelBuilder.Entity<Price>()
            .HasKey(p => new { p.Date, p.ProductKey });
        modelBuilder.Entity<Price>()
            .HasIndex(p => p.Date);
        modelBuilder.Entity<Price>()
            .HasData(SampleDataGenerator.GenerateSamplePrices());
    }
}
