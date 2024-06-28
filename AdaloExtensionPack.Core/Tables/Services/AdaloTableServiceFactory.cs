using System;
using System.Net.Http;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using AdaloExtensionPack.Core.Tables.Options;

namespace AdaloExtensionPack.Core.Tables.Services;

public class AdaloTableServiceFactory(
    IHttpClientFactory httpClientFactory) : IAdaloTableServiceFactory
{
    public IAdaloTableService<T> Create<T>(AdaloTableOptions options) where T : IAdaloEntity
    {
        return new AdaloTableService<T>(httpClientFactory, options);
    }

    public IAdaloTableService Create(AdaloTableOptions options)
    {
        return new AdaloTableService(httpClientFactory, options);
    }

    public object Create(Type type, AdaloTableOptions options)
    {
        return Activator.CreateInstance(typeof(AdaloTableService<>).MakeGenericType(type),
            [httpClientFactory, options]);
    } 
}
