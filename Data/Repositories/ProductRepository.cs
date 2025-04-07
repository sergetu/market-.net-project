using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(TradeMarketDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            var assets = Task.Run(() => DbSet
            .Include(p => p.ReceiptDetails)
            .Include(p => p.Category)
            .AsEnumerable());
            return assets;
        }

        public Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return DbSet
                .Include(p => p.ReceiptDetails)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }

}
