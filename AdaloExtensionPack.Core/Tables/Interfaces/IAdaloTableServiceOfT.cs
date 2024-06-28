using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AdaloExtensionPack.Core.Tables.Data;

namespace AdaloExtensionPack.Core.Tables.Interfaces;

public interface IAdaloTableService<T> where T : IAdaloEntity
{
    string TableId { get; }
    Task<List<T>> GetAllAsync((Expression<Func<T, object>> Predicate, object Value)? predicate = null);
    Task<T> PostAsync(T payload);
    Task<T> GetAsync(int recordId);
    Task DeleteAsync(int recordId);
    Task<T> PutAsync(int recordId, T payload);
    Task<T> PutAsync(int recordId, Action<T> update);
}
