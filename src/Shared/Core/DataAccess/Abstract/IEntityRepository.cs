using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.DataAccess.Abstract
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        T Get(Expression<Func<T, bool>> predicate);
        IList<T> GetList(Expression<Func<T, bool>> predicate = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
