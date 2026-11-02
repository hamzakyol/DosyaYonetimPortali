using Microsoft.EntityFrameworkCore;
using DosyaYonetimPortali.Models.Entities;

namespace DosyaYonetimPortali.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FileItem> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // İlişkilerde döngüsel silme engeli (Restrict cascade delete)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FileItem>()
                .HasOne(f => f.AppUser)
                .WithMany(u => u.Files)
                .HasForeignKey(f => f.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FileItem>()
                .HasOne(f => f.Category)
                .WithMany(c => c.Files)
                .HasForeignKey(f => f.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}