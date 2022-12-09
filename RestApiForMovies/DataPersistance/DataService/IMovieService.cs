using Microsoft.EntityFrameworkCore;
using RestApiForMovies.Entities;
using System.Linq.Expressions;

namespace RestApiForMovies.DataPersistance.DataService
{
    public interface IMovieService<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(int id);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
    }
}
