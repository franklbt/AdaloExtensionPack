using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdaloExtensionPack.Core.Adalo
{
    public interface IAdaloTableService<T> where T : AdaloEntity
    {
        Task<List<T>> GetAllAsync((Expression<Func<T, object>> Predicate, object Value)? predicate = null);
        Task<T> PostAsync(T payload);
        Task<T> GetAsync(int recordId);
        Task DeleteAsync(int recordId);
        Task<T> PutAsync(int recordId, T payload);
        Task<T> PutAsync(int recordId, Action<T> update);
    }
}
