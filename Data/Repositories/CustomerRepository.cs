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
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {

        public CustomerRepository(TradeMarketDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            var assets = Task.Run(() => DbSet
            .Include(c => c.Person)
            .Include(c => c.Receipts)
                .ThenInclude(r => r.ReceiptDetails)
            .AsEnumerable());
            return assets;
        }

        public Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return DbSet
                .Include(c => c.Person)
                .Include(c => c.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
