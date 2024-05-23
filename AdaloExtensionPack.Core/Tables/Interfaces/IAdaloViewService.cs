using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdaloExtensionPack.Core.Tables.Interfaces
{
    public interface IAdaloViewService<TContext, TBase, TResult>
    {
        Task<List<TResult>> GetAllAsync();
    }
}