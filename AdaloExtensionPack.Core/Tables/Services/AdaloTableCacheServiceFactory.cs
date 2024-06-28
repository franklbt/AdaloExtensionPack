using System;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using AdaloExtensionPack.Core.Tables.Options;
using Microsoft.Extensions.Caching.Memory;

namespace AdaloExtensionPack.Core.Tables.Services;

public class AdaloTableCacheServiceFactory(
    AdaloTableServiceFactory adaloTableServiceFactory,
    IServiceProvider serviceProvider,
    IMemoryCache memoryCache) : IAdaloTableCacheServiceFactory
{
    public IAdaloTableCacheService<T> Create<T>(AdaloTableOptions options) where T : IAdaloEntity
    {
        return new AdaloTableCacheService<T>(memoryCache, adaloTableServiceFactory.Create<T>(options), options,
            serviceProvider);
    }

    public IAdaloTableCacheService Create(AdaloTableOptions options)
    {
        return new AdaloTableCacheService(memoryCache, adaloTableServiceFactory.Create(options), options,
            serviceProvider);
    }

    public object Create(Type type, AdaloTableOptions options)
    {
        return Activator.CreateInstance(typeof(AdaloTableCacheService<>).MakeGenericType(type),
            [memoryCache, adaloTableServiceFactory.Create(type, options), options, serviceProvider]);
    }
}
