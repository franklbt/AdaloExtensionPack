using System;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Options;

namespace AdaloExtensionPack.Core.Tables.Interfaces
{
    public interface IAdaloTableServiceFactory
    {
        IAdaloTableService<T> Create<T>(AdaloAppOptions options, string tableId) where T: AdaloEntity;
        object Create(Type type, AdaloAppOptions options, string tableId);
    }
}
