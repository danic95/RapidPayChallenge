using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using RapidPayChallenge.Domain.Helper;
using RapidPayChallenge.Domain.Models;

namespace RapidPayChallenge.Data
{
    public class RapidPayDbContext : DbContext
    {
        public RapidPayDbContext(DbContextOptions<RapidPayDbContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Transac> Transacs { get; set; }

        public DbSet<PaymFee> PaymFees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<Account>()
                .Property(k => k.Email).IsRequired();
            modelBuilder.Entity<Account>()
                .Property(k => k.PasswordHash).IsRequired();

            modelBuilder.Entity<Card>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<Card>()
                .Property(k => k.Number)
                .HasMaxLength(16);
            modelBuilder.Entity<Card>()
                .Property(k => k.Number)
                .IsRequired();
            modelBuilder.Entity<Card>()
                .Property(k => k.Balance)
                .IsRequired();

            modelBuilder.Entity<Transac>()
                .HasKey(k => k.Id);
            modelBuilder.Entity<Transac>()
                .Property(k => k.Amount)
                .IsRequired();

            modelBuilder.Entity<PaymFee>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Account>()
              .HasMany(k => k.Cards)
              .WithOne(k => k.Account)
              .HasForeignKey(k => k.AccountId)
              .HasPrincipalKey(k => k.Id);

            modelBuilder.Entity<Card>()
              .HasMany(k => k.Transactions)
              .WithOne(k => k.Card)
              .HasForeignKey(k => k.CardId)
              .HasPrincipalKey(k => k.Id);

            var demoAccountId = Guid.NewGuid();
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Admin",
                    LastName = "RapidPay",
                    Email = "admin@rapidpay.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("RapidPay!2024")
                },
                new Account
                {
                    Id = demoAccountId.ToString(),
                    FirstName = "Daniel",
                    LastName = "Carias",
                    Email = "dcarias@rapidpay.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Daniel!2024")
                }
            );

            modelBuilder.Entity<Card>().HasData(
               new Card
               {
                   Id = Guid.NewGuid(),
                   Number = "4228567262279934",
                   ExpMonth = 12,
                   ExpYear = 2026,
                   CVC = "481",
                   Balance = 10000,
                   AccountId = demoAccountId.ToString()
               });

            modelBuilder.Entity<PaymFee>().HasData(
              new PaymFee
              {
                  Id = Guid.NewGuid(),
                  Fee = PaymFeeGen.GenPaymFee()
              });
        }
    }
}