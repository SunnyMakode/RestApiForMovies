using Microsoft.EntityFrameworkCore;
using RestApiForMovies.BusinessLogic.ServiceInterface;
using RestApiForMovies.DataPersistance;
using System.Linq.Expressions;

namespace RestApiForMovies.UnitTest
{
    public class FakeMovieService<TEntity> : IDataService<TEntity> where TEntity : class
    {
        private readonly DataContext _context;

        public FakeMovieService(DataContext context)
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

        public void Add(TEntity entity)
        {
            this._context.Set<TEntity>().Add(entity);
        }

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
