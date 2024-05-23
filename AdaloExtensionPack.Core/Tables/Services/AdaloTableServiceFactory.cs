using System;
using System.Net.Http;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using AdaloExtensionPack.Core.Tables.Options;

namespace AdaloExtensionPack.Core.Tables.Services
{
    public class AdaloTableServiceFactory : IAdaloTableServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdaloTableServiceFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IAdaloTableService<T> Create<T>(AdaloAppOptions options, string tableId) where T: AdaloEntity
        {
            return new AdaloTableService<T>(_httpClientFactory, options.AppId, options.Token, tableId);
        }

        public object Create(Type type, AdaloAppOptions options, string tableId)
        {
            return Activator.CreateInstance(typeof(AdaloTableService<>).MakeGenericType(type),
                new object[] {_httpClientFactory, options.AppId, options.Token, tableId});
        }
    }
}
