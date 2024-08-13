using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public class UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : DbContext(options)
    {
        public DbSet<Url> Urls { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>().HasKey(u => new { u.Id, u.UserId });
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Url>().HasOne<User>().WithMany().HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Url>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Token>().HasKey(t => t.Value);
            base.OnModelCreating(modelBuilder);
        }
    }
}