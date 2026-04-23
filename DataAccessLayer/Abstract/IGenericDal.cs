using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IGenericDal<T> where T : class
    {
        Task InsertAsync(T t);
        Task UpdateAsync(T t);
        Task DeleteAsync(T t);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        //T Get(Expression<Func<T, bool>> filter);
        //List<T> GetList(Expression<Func<T, bool>> filter);
    }
}
