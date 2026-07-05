using Microsoft.EntityFrameworkCore;
using Proizvodi.Api.Models;

namespace Proizvodi.Api.Data;

public class ProizvodiContext(DbContextOptions<ProizvodiContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<UserFavorite> UserFavorites => Set<UserFavorite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserFavorite>()
            .HasKey(uf => new { uf.UserId, uf.ProductId });

        modelBuilder.Entity<UserFavorite>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(uf => uf.UserId);

        modelBuilder.Entity<UserFavorite>()
            .HasOne(uf => uf.Product)
            .WithMany(p => p.FavoritedBy)
            .HasForeignKey(uf => uf.ProductId);
    }
}