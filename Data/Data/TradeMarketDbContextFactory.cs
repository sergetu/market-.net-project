using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    public class TradeMarketDbContextFactory : IDesignTimeDbContextFactory<TradeMarketDbContext>
    {
        public TradeMarketDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TradeMarketDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TradeMarket;Trusted_Connection=True;");

            return new TradeMarketDbContext(optionsBuilder.Options);
        }
    }
}
