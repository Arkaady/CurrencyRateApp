using CurrencyRateApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CurrencyRateApp.Context
{
    public class EFContext : DbContext
    {
        public DbSet<AuthorizationKey> AuthorizationKeys { get; set; }

        public EFContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AuthorizationKey(modelBuilder);
        }

        private void AuthorizationKey(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorizationKey>()
                .HasKey(ak => ak.Id);

            modelBuilder.Entity<AuthorizationKey>()
                .HasIndex(ak => ak.ApiKeyHash)
                .IsUnique();

            modelBuilder.Entity<AuthorizationKey>()
                .Property(ak => ak.ApiKeyHash)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
