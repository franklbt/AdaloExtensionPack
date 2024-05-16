using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaloExtensionPack.Core.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloTableCacheService<T>(
        IMemoryCache memoryCache,
        IAdaloTableService<T> adaloService,
        IOptions<AdaloOptions> options,
        IServiceProvider serviceProvider)
        : IAdaloTableCacheService<T>
        where T : AdaloEntity
    {
        private readonly AdaloOptions _options = options.Value;

        public async Task<List<T>> GetAllAsync()
        {
            var tableTypes = GetAllTableTypes();

            var tableId = tableTypes[typeof(T)];
            var result = await memoryCache.GetOrCreateAsync(tableId,
                async _ => await adaloService.GetAllAsync());
            return result;
        }

        private Dictionary<Type, AdaloTableOptions> GetAllTableTypes()
        {
            return _options.Apps
                .SelectMany(x => x.TablesTypes)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public async Task<T> PostAsync(
            T payload)
        {
            var tableTypes = GetAllTableTypes();
            var tableOptions = tableTypes[typeof(T)];
            var result = await adaloService.PostAsync(payload);

            UpdateTableCache(tableOptions.TableId,
                list => list.Append(result).ToList());

            return result;
        }

        public async Task<T> GetAsync(
            int recordId)
        {
            var tableTypes = GetAllTableTypes();
            var tableOptions = tableTypes[typeof(T)];
            var exists = memoryCache.TryGetValue<List<T>>(tableOptions, out var table);

            if (!exists || table is null)
            {
                RefreshTableCache(tableOptions.TableId);
                return await adaloService.GetAsync(recordId);
            }

            var result = table.FirstOrDefault(x => x.Id == recordId);

            return result;
        }

        private void RefreshTableCache(string tableId)
        {
            Task.Run(async () =>
                {
                    using var scope = serviceProvider.CreateScope();
                    using var cache = scope.ServiceProvider.GetService<IMemoryCache>();
                    await memoryCache.GetOrCreateAsync(tableId, async _ => await adaloService.GetAllAsync());
                })
                .Ignore();
        }

        public async Task DeleteAsync(
            int recordId)
        {
            var tableTypes = GetAllTableTypes();
            var tableOptions = tableTypes[typeof(T)];
            await adaloService.DeleteAsync(recordId);
            UpdateTableCache(tableOptions.TableId,
                list => list.Where(x => x.Id != recordId).ToList());
        }

        private void UpdateTableCache(string tableId, Func<List<T>, List<T>> update)
        {
            if (memoryCache.TryGetValue<List<T>>(tableId, out var collection))
            {
                memoryCache.Set(tableId, update(collection ??= []));
            }
            else
            {
                RefreshTableCache(tableId);
            }
        }

        public async Task<T> PutAsync(
            int recordId,
            T payload)
        {
            var tableTypes = GetAllTableTypes();
            var tableOptions = tableTypes[typeof(T)];
            var result = await adaloService.PutAsync(recordId, payload);
            UpdateTableCache(tableOptions.TableId,
                list => list.Where(x => x.Id != recordId).Append(result).ToList());
            return result;
        }
    }
}
