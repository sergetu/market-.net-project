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
    public class ReceiptRepository: BaseRepository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(TradeMarketDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            var assets = Task.Run(() => DbSet
            .Include(r => r.ReceiptDetails)
                .ThenInclude(rd=>rd.Product)
                    .ThenInclude(p=>p.Category)
            .Include(p => p.Customer)
                .ThenInclude(c=>c.Person).AsEnumerable());
            return assets;
        }

        public Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return DbSet
                .Include(r => r.ReceiptDetails)
                    .ThenInclude(rd => rd.Product)
                        .ThenInclude(p => p.Category)
                .Include(p => p.Customer)
                    .ThenInclude(c => c.Person)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }

}
