using System;
using System.Net.Http;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using AdaloExtensionPack.Core.Tables.Options;

namespace AdaloExtensionPack.Core.Tables.Services
{
    public class AdaloTableServiceFactory(IHttpClientFactory httpClientFactory) : IAdaloTableServiceFactory
    {
        public IAdaloTableService<T> Create<T>(AdaloAppOptions options, string tableId) where T: AdaloEntity
        {
            return new AdaloTableService<T>(httpClientFactory, options.AppId, options.Token, tableId);
        }

        public object Create(Type type, AdaloAppOptions options, string tableId)
        {
            return Activator.CreateInstance(typeof(AdaloTableService<>).MakeGenericType(type),
                new object[] {httpClientFactory, options.AppId, options.Token, tableId});
        }
    }
}
