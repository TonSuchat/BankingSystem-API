using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DBModels
{
    public class BankingSystemContext : DbContext
    {
        public BankingSystemContext(DbContextOptions<BankingSystemContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MasterIBAN> MasterIBANs { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Account
            modelBuilder.Entity<Account>().Property(a => a.CreatedOn).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Account>().Property(a => a.IsActive).HasDefaultValue(true);
            #endregion

            #region Customer
            modelBuilder.Entity<Customer>().Property(c => c.CreatedOn).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Customer>().Property(c => c.IsActive).HasDefaultValue(true);
            #endregion

            #region Transaction
            modelBuilder.Entity<Transaction>().HasOne(t => t.TransferAccount).WithMany(a => a.TransferTransactions).HasForeignKey(t => t.TransferIBAN);
            modelBuilder.Entity<Transaction>().HasOne(t => t.ReceiveAccount).WithMany(a => a.ReceivedTransactions).HasForeignKey(t => t.ReceiveIBAN);

            modelBuilder.Entity<Transaction>().Property(t => t.CreatedOn).HasDefaultValueSql("getdate()");
            #endregion
        }
    }
}
