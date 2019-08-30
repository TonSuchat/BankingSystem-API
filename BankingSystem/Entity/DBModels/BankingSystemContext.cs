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
            modelBuilder.Entity<Transaction>().HasOne(t => t.TransferAccount).WithMany(a => a.TransferTransactions).HasForeignKey(t => t.TransferIBAN);
            modelBuilder.Entity<Transaction>().HasOne(t => t.ReceiveAccount).WithMany(a => a.ReceivedTransactions).HasForeignKey(t => t.ReceiveIBAN);
        }
    }
}
