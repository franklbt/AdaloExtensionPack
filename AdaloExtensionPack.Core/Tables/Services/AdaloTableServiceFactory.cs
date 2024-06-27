using System;
using System.Net.Http;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using AdaloExtensionPack.Core.Tables.Options;
using Microsoft.Extensions.Caching.Memory;

namespace AdaloExtensionPack.Core.Tables.Services
{
    public class AdaloTableServiceFactory(
        IHttpClientFactory httpClientFactory,
        IServiceProvider serviceProvider,
        IMemoryCache memoryCache) : IAdaloTableServiceFactory
    {
        public IAdaloTableService<T> Create<T>(AdaloTableOptions options) where T : IAdaloEntity
        {
            return new AdaloTableService<T>(httpClientFactory, options);
        }

        public IAdaloTableService<AdaloDynamicEntity> Create(AdaloTableOptions options)
        {
            return Create<AdaloDynamicEntity>(options);
        }

        public object Create(Type type, AdaloTableOptions options)
        {
            return Activator.CreateInstance(typeof(AdaloTableService<>).MakeGenericType(type),
                [httpClientFactory, options]);
        }

        public IAdaloTableCacheService<T> CreateCache<T>(AdaloTableOptions options) where T : IAdaloEntity
        {
            return new AdaloTableCacheService<T>(memoryCache, Create<T>(options), options, serviceProvider);
        }

        public IAdaloTableCacheService<AdaloDynamicEntity> CreateCache(AdaloTableOptions options)
        {
            return CreateCache<AdaloDynamicEntity>(options);
        }

        public object CreateCache(Type type, AdaloTableOptions options)
        {
            return Activator.CreateInstance(typeof(AdaloTableCacheService<>).MakeGenericType(type),
                [memoryCache, Create(type, options), options, serviceProvider]);
        }
    }
}
