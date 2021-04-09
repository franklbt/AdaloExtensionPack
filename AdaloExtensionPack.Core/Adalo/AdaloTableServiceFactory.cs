using System;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloTableServiceFactory : IAdaloTableServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AdaloOptions _options;

        public AdaloTableServiceFactory(IHttpClientFactory httpClientFactory, IOptions<AdaloOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public IAdaloTableService<T> Create<T>(string tableId)
        {
            return new AdaloTableService<T>(_httpClientFactory, _options.AppId, _options.Token, tableId);
        }

        public object Create(Type type, string tableId)
        {
            return Activator.CreateInstance(typeof(AdaloTableService<>).MakeGenericType(type),
                new object[] {_httpClientFactory, _options.AppId, _options.Token, tableId});
        }
    }
}