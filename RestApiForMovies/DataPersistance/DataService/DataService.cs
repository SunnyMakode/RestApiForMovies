using Microsoft.EntityFrameworkCore;
using RestApiForMovies.BusinessLogic.ServiceInterface;
using System.Linq.Expressions;

namespace RestApiForMovies.DataPersistance.DataService
{
    public class DataService<TEntity> : IDataService<TEntity> where TEntity : class
    {
        private readonly DataContext _context;

        public DataService(DataContext context)
        {
            this._context = context;
        }

        public async Task<TEntity> Get(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return this._context.Set<TEntity>().Where(predicate);
        }

        public void Add(TEntity entity)
        {
            this._context.Set<TEntity>().Add(entity);
        }

        public void Remove(TEntity entity)
        {
            this._context.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            this._context.Entry<TEntity>(entity).State = EntityState.Modified;
        }
    }
}
