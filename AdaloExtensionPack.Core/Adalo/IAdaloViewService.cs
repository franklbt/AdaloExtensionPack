using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdaloExtensionPack.Core.Adalo
{
    public interface IAdaloViewService<TContext, TBase, TResult>
    {
        Task<List<TResult>> GetAllAsync();
    }
}