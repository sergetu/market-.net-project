using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Data
{
    public class TradeMarketDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptsDetails { get; set; }

        public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }
            modelBuilder.Entity<ReceiptDetail>()
                .HasKey(rd => new { rd.ReceiptId, rd.ProductId });
        }
    }
}
