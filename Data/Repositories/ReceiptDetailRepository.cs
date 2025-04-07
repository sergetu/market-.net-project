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
    public class ReceiptDetailRepository : BaseRepository<ReceiptDetail>, IReceiptDetailRepository
    {
        public ReceiptDetailRepository(TradeMarketDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            var assets = Task.Run(() => DbSet
            .Include(rd => rd.Product)
                .ThenInclude(p => p.Category)
            .Include(rd => rd.Receipt)
                .ThenInclude(r => r.Customer)
            .AsEnumerable());
            return assets;
        }
    }
}
