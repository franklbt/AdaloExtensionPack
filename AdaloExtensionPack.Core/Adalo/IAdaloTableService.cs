using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdaloExtensionPack.Core.Adalo
{
    public interface IAdaloTableService<T>
    {
        Task<List<T>> GetAllAsync(params (Expression<Func<T, object>> Predicate, object Value)[] predicates);
        Task<T> PostAsync(T payload);
        Task<T> GetAsync(int recordId);
        Task DeleteAsync(int recordId);
        Task<T> PutAsync(int recordId, T payload);
    }
}
