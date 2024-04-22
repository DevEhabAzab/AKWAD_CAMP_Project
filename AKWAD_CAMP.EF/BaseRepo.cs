using AKWAD_CAMP.Core;
using AKWAD_CAMP.Core.Contants;
using AKWAD_CAMP.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AKWAD_CAMP.EF
{
    public class BaseRepo <T>:IBaseRepo<T> where T : class
    {
        private readonly SalesContext _context;

        public BaseRepo(SalesContext context)
        {
            _context = context;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
           
            return entities;
        }

        public bool Any(Expression<Func<T, bool>> criteria)
        {
            return _context.Set<T>().Any(criteria);
        }

     

       

       

        public int Count(Expression<Func<T, bool>>? criteria = null)
        {
            if(criteria is null)
                return _context.Set<T>().Count();
            return _context.Set<T>().Count(criteria);
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
           
            return entity;
        }

        public T Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
           
            return entity;
        }

        public IEnumerable<T> DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
           
            return entities;
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(criteria);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>>? criteria = null, Expression<Func<T, object>>? orderBy = null, OrderDirection? orderDirection = OrderDirection.ASC, int? take = null, int? skip = null, string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (criteria is not null)
                query = query.Where(criteria);
            if (orderBy is not null)
            {
                if (orderDirection == OrderDirection.ASC)
                    query = query.OrderBy(orderBy);
                if (orderDirection == OrderDirection.DESC)
                    query = query.OrderByDescending(orderBy);
            }
            if (skip is not null)
                query = query.Skip(skip.Value);
            if(take is not null)
                query =  query.Take(take.Value);
            if (includes is not null)
                foreach (var Property in includes)
                    query = query.Include(Property);

            return query;
                
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T Find(int? id)
        {
            int i =0;
            if (id.HasValue)
            {
                i = id.Value;
                return _context.Set<T>().Find(i);
            }
            return null;
        }

        public async Task<IEnumerable<T>> ToListAsync()
        {
           return await _context.Set<T>().ToListAsync();
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
           
            return entity;
        }

        public IEnumerable<T> UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
           
            return entities;
        }

        public async Task<T> FindAsync(int? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T BusinessDelete(T entity)
        {
            var type = typeof(T);
            PropertyInfo propertyInfo = type.GetProperty("Deleted");
            if (propertyInfo != null)
                propertyInfo.SetValue(entity,true);

            return entity;

        }
    }
}
