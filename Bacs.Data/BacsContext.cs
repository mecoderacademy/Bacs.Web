using Bacs.Models;
using Bacs.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bacs.Data
{
    public class BacsContext : DbContext
    {
        public BacsContext(DbContextOptions<BacsContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().HasKey(x => x.Id);
            modelBuilder.Entity<Transaction>().HasOne(x => x.FileTransaction);
            modelBuilder.Entity<FileTransaction>().Ignore(x => x.ResponseMessage).HasMany(x=>x.Transactions);
            modelBuilder.Entity<FileTransaction>().HasKey(x => x.Id);


        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<FileTransaction> FileTransactions { get; set; }
    }
}
