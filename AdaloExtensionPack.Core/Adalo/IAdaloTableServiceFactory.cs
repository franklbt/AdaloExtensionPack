using System;

namespace AdaloExtensionPack.Core.Adalo
{
    public interface IAdaloTableServiceFactory
    {
        IAdaloTableService<T> Create<T>(AdaloAppOptions options, string tableId);
        object Create(Type type, AdaloAppOptions options, string tableId);
    }
}
