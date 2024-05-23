using System.Collections.Generic;
using System.Threading.Tasks;
using AdaloExtensionPack.Core.Tables.Data;

namespace AdaloExtensionPack.Core.Tables.Interfaces;

public interface IAdaloTableCacheService<T> where T : AdaloEntity
{
    Task<List<T>> GetAllAsync();

    Task<T> PostAsync(
        T payload);

    Task<T> GetAsync(int recordId);

    Task DeleteAsync(int recordId);

    Task<T> PutAsync(int recordId, T payload);
}
