using System;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Options;

namespace AdaloExtensionPack.Core.Tables.Interfaces;

public interface IAdaloTableCacheServiceFactory
{ 
    IAdaloTableCacheService<T> Create<T>(AdaloTableOptions options) where T : IAdaloEntity;
    IAdaloTableCacheService<AdaloDynamicEntity> Create(AdaloTableOptions options);
    object Create(Type type, AdaloTableOptions options);
}