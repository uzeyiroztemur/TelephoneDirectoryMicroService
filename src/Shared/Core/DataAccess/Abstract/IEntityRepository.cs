using Core.Entities.Abstract;
using System.Linq.Expressions;

namespace Core.DataAccess.Abstract
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        IList<T> GetList(Expression<Func<T, bool>> predicate = null);
        Task<IList<T>> GetListAsync(Expression<Func<T, bool>> predicate = null);

        T Get(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        void Add(T entity);
        Task AddAsync(T entity);

        void Update(T entity);
        Task UpdateAsync(T entity);

        void Delete(T entity);
        Task DeleteAsync(T entity);
    }
}
