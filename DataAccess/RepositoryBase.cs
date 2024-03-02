using DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly RepositoryContext _context;

        public RepositoryBase(RepositoryContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return trackChanges
                ? _context.Set<T>()
                : _context.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindAllByCondition(bool trackChanges, Func<T, bool> condition = null)
        {
            IQueryable<T> query = FindAll(trackChanges);

            if (condition != null)
            {
                query = query.Where(condition).AsQueryable();
            }

            return query;
        }
        public IQueryable<T> FindIncludeByCondition(bool trackChanges, Func<T, bool> condition = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = FindAll(trackChanges);

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (condition != null)
            {
                query = query.Where(condition).AsQueryable();
            }

            return query;
        }

        public T? FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            if (expression == null)
            {
                return null;
            }
            else
            {
                return trackChanges
                ? _context.Set<T>().Where(expression).FirstOrDefault()
                : _context.Set<T>().Where(expression).AsNoTracking().FirstOrDefault();
            }
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public IQueryable<T> FindInclude(bool trackChanges, params Expression<Func<T, object>>[] includes)
        {
            var query = trackChanges
                ? _context.Set<T>().AsQueryable()
                : _context.Set<T>().AsNoTracking();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query;
        }
    }
}
