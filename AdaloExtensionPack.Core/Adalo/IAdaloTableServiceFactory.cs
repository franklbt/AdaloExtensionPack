using System;

namespace AdaloExtensionPack.Core.Adalo
{
    public interface IAdaloTableServiceFactory
    {
        IAdaloTableService<T> Create<T>(string tableId);
        object Create(Type type, string tableId);
    }
}