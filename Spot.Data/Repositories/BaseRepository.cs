using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Spot.Data.Contracts;
using Spot.Data.Entities;

namespace Spot.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseIntegerIdEntity
    {
        protected readonly ApplicationContext _context;

        public BaseRepository(ApplicationContext context)
        {
            this._context = context;
        }

        public async Task<T?> GetAsync(int id)
        {
            return await this._context.FindAsync<T>(id);
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await this._context.Set<T>().ToListAsync();
        }

        public async Task<T> SaveAsync(T entity)
        {
            EntityEntry<T> entry;
            if (entity.Id == null)
            {
                entry = this._context.Add(entity);
            }
            else
            {
                entry = this._context.Update(entity);
            }

            await this._context.SaveChangesAsync();
            return await this.GetAsync(entry.Entity.Id.Value);
        }

        public async Task SaveRangeAsync(IEnumerable<T> entities)
        {
            this._context.AddRange(entities); // TODO do the same as save async
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            this._context.Remove(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            this._context.RemoveRange(entities);
            await this._context.SaveChangesAsync();
        }
    }
}
