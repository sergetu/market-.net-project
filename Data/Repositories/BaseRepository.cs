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
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly TradeMarketDbContext context;
        private readonly DbSet<T> dbSet;

        protected TradeMarketDbContext Context => context;
        protected DbSet<T> DbSet => dbSet;

        protected BaseRepository(TradeMarketDbContext context)
        {
            this.context = context;
            dbSet = this.context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
            Context.SaveChanges();
        }

        public virtual async Task DeleteByIdAsync(int id)
        {
            var entity = await DbSet.FirstOrDefaultAsync(entity => entity.Id == id);
            if (entity != null)
            {
                DbSet.Remove(entity);
                await Context.SaveChangesAsync();
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public virtual void Update(T entity)
        {
            var attachedEntityWithoutInclude = DbSet.AsNoTracking().FirstOrDefault(DBEntity => DBEntity.Id == entity.Id);

            var attachedEntity = DbSet.FirstOrDefault(DBEntity => DBEntity.Id == entity.Id);

            if (attachedEntity != null)
            {
                Context.Entry(attachedEntity).State = EntityState.Detached;
                Context.Entry(attachedEntityWithoutInclude).CurrentValues.SetValues(entity);
                DbSet.Update(attachedEntityWithoutInclude);
            }
            else
            {
                DbSet.Update(entity);
            }
            Context.SaveChanges();
        }
    }
}
