using AKWAD_CAMP.Core.Contants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKWAD_CAMP.Core.Interfaces
{
    public interface IBaseRepo<T> where T : class
    {
        IEnumerable<T> GetAll();
       Task<IEnumerable<T>> ToListAsync();
        
        Task<T> FirstOrDefaultAsync(Expression<Func<T,bool>> criteria);
        Task<T> FindAsync(int? id);
        T Find(int? id);
        T Update(T entity);
        T Remove(T entity);
        T Add(T entity);

        T BusinessDelete(T entity);
        IEnumerable<T> Get(Expression<Func<T, bool>>? criteria = null, Expression<Func<T, object>>? orderBy=null, OrderDirection? orderDirection = OrderDirection.ASC , int? take=null, int? skip =null, string[]? includes =null);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        IEnumerable<T> UpdateRange(IEnumerable<T> entities);
        IEnumerable<T> DeleteRange(IEnumerable<T> entities);
        int Count(Expression<Func<T, bool>>? criteria = null);
        bool Any(Expression<Func<T, bool>> criteria);
        

    }
}
