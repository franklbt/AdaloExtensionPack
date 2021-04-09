using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloTableCacheService<T> where T : AdaloEntity
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IAdaloTableService<T> _adaloService;
        private readonly AdaloOptions _options;

        public AdaloTableCacheService(IMemoryCache memoryCache,
            IAdaloTableService<T> adaloService,
            IOptions<AdaloOptions> options)
        {
            _memoryCache = memoryCache;
            _adaloService = adaloService;
            _options = options.Value;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var tableId = _options.TablesTypes[typeof(T)]; 
            var result = await _memoryCache.GetOrCreateAsync(tableId,
                async _ => await _adaloService.GetAllAsync());
            return result;
        }

        public async Task<T> PostAsync(
            T payload)
        {
            var tableId = _options.TablesTypes[typeof(T)]; 
            var result = await _adaloService.PostAsync(payload);
            if (_memoryCache.TryGetValue<List<T>>(tableId, out var collection))
            {
                collection.Add(result);
                _memoryCache.Set(tableId, collection);
            }

            return result;
        }

        public async Task<T> GetAsync(
            int recordId)
        {
            var tableId = _options.TablesTypes[typeof(T)]; 
            var table = await _memoryCache.GetOrCreateAsync(tableId,
                async _ => await _adaloService.GetAllAsync());

            var result = table.FirstOrDefault(x => x.Id == recordId);

            return result;
        }

        public async Task DeleteAsync(
            int recordId)
        {
            var tableId = _options.TablesTypes[typeof(T)]; 
            await _adaloService.DeleteAsync(recordId);
            if (_memoryCache.TryGetValue<List<T>>(tableId, out var collection))
            {
                var list = collection.Where(x => x.Id != recordId).ToList();
                _memoryCache.Set(tableId, list);
            }
        }

        public async Task<T> PutAsync(
            int recordId,
            T payload)
        {
            var tableId = _options.TablesTypes[typeof(T)]; 
            var result = await _adaloService.PutAsync(recordId, payload);
            if (_memoryCache.TryGetValue<List<T>>(tableId, out var collection))
            {
                collection = collection.Where(x => x.Id != recordId).Concat(new[] {result}).ToList();
                _memoryCache.Set(tableId, collection);
            }

            return result;
        }
    }
}