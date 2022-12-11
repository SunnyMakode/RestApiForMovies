using Microsoft.EntityFrameworkCore;
using RestApiForMovies.Entities;
using System.Linq.Expressions;

namespace RestApiForMovies.DataPersistance.DataService
{
    public interface IDataService<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicates);
        Task<TEntity> Get(int id);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
    }
}
