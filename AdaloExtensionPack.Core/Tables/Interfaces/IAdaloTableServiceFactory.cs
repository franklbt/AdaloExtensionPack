﻿using System;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Options;

namespace AdaloExtensionPack.Core.Tables.Interfaces
{
    public interface IAdaloTableServiceFactory
    {
        IAdaloTableService<T> Create<T>(AdaloTableOptions options) where T : IAdaloEntity;
        object Create(Type type, AdaloTableOptions options);

        IAdaloTableService<AdaloDynamicEntity> Create(AdaloTableOptions options);
        IAdaloTableCacheService<T> CreateCache<T>(AdaloTableOptions options) where T : IAdaloEntity;
        IAdaloTableCacheService<AdaloDynamicEntity> CreateCache(AdaloTableOptions options);
        object CreateCache(Type type, AdaloTableOptions options);
    }
}
