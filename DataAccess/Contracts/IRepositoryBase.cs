using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindInclude(bool trackChanges, params Expression<Func<T, object>>[] includes);
        IQueryable<T> FindAllByCondition(bool trackChanges, Func<T, bool> condition = null);
        T? FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        void Create(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
