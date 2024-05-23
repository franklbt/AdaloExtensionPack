using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaloExtensionPack.Core.Helpers;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using AdaloExtensionPack.Core.Tables.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdaloExtensionPack.Core.Tables.Services
{ 
    public class AdaloTableCacheService<T>(
        IMemoryCache memoryCache,
        IAdaloTableService<T> adaloService,
        IOptions<AdaloOptions> options,
        IServiceProvider serviceProvider)
        : IAdaloTableCacheService<T> where T : AdaloEntity
    {
        private readonly AdaloOptions _options = options.Value;

        public async Task<List<T>> GetAllAsync()
        {
            var tableTypes = GetAllTableTypes();
            var table = tableTypes[typeof(T)];
            return await RefreshTableCacheAsync(table);
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

            UpdateTableCache(tableOptions, list => list.Append(result).ToList());

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
                RefreshTableCache(tableOptions);
                return await adaloService.GetAsync(recordId);
            }

            var result = table.FirstOrDefault(x => x.Id == recordId);

            return result;
        }

        private void RefreshTableCache(AdaloTableOptions table)
        {
            ArgumentNullException.ThrowIfNull(table);
            Task.Run(async () => await RefreshTableCacheAsync(table)).Ignore();
        }

        private async Task<List<T>> RefreshTableCacheAsync(AdaloTableOptions table)
        {
            using var scope = serviceProvider.CreateScope();
            using var cache = scope.ServiceProvider.GetService<IMemoryCache>();
            cache.Remove(table.TableId);
            return await cache.GetOrCreateAsync(table.TableId, async entry =>
            {
                if (table.IsCached && table.CacheDuration != null)
                {
                    entry.AbsoluteExpirationRelativeToNow = table.CacheDuration.Value;
                }
                return await adaloService.GetAllAsync();
            });
        }

        public async Task DeleteAsync(
            int recordId)
        {
            var tableTypes = GetAllTableTypes();
            var tableOptions = tableTypes[typeof(T)];
            await adaloService.DeleteAsync(recordId);
            UpdateTableCache(tableOptions, list => list.Where(x => x.Id != recordId).ToList());
        }

        private void UpdateTableCache(AdaloTableOptions table, Func<List<T>, List<T>> update)
        {
            if (memoryCache.TryGetValue<List<T>>(table.TableId, out var collection))
            {
                var updatedCollection = update(collection ??= []);
                if (table.CacheDuration is not null)
                {
                    memoryCache.Set(table.TableId, updatedCollection, table.CacheDuration.Value);
                }
                else
                {
                    memoryCache.Set(table.TableId, updatedCollection);
                }
            }
            else
            {
                RefreshTableCache(table);
            }
        }

        public async Task<T> PutAsync(
            int recordId,
            T payload)
        {
            var tableTypes = GetAllTableTypes();
            var tableOptions = tableTypes[typeof(T)];
            var result = await adaloService.PutAsync(recordId, payload);
            UpdateTableCache(tableOptions, list => list.Where(x => x.Id != recordId).Append(result).ToList());
            return result;
        }
    }
}
