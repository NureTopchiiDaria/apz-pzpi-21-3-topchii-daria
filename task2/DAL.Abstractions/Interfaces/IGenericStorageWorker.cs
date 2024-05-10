using System.Linq.Expressions;
using Core.Models;

namespace DAL.Abstractions.Interfaces
{
    public interface IGenericStorageWorker<T>
        where T : BaseModel
    {
        Task<IEnumerable<T>> GetByConditions(Expression<Func<T, bool>>[] conditions, params Expression<Func<T, object>>[] includes);

        Task<T> GetByCondition(Expression<Func<T, bool>> condition);

        Task Create(T entity);

        Task Update(T entity);

        Task Delete(T entity);
    }
}
