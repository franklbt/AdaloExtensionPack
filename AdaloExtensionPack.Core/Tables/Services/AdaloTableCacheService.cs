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
        private readonly AdaloTableOptions _tableOptions = options.Value.Apps
            .SelectMany(x => x.Tables.Select(y => y.Value))
            .Where(x => x.Options.TableId == adaloService.TableId)
            .Select(x => x.Options)
            .First();

        public async Task<List<T>> GetAllAsync()
        {  
            return await RefreshTableCacheAsync(_tableOptions);
        }
 
        public async Task<T> PostAsync(
            T payload)
        {  
            var result = await adaloService.PostAsync(payload);
            RefreshRecord(_tableOptions, result.Id, result).Ignore();
            UpdateTableCache(_tableOptions, list => list.Append(result).ToList());

            return result;
        }

        public async Task<T> GetAsync(
            int recordId)
        {  
            var exists = memoryCache.TryGetValue<T>($"{_tableOptions.TableId}-{recordId}", out var record);

            if (exists && record is not null)
            {
                return record;
            }

            exists = memoryCache.TryGetValue<List<T>>(_tableOptions.TableId, out var table);
            if (exists && table is { Count: > 0 })
            {
                return table.FirstOrDefault(x => x.Id == recordId);
            }

            RefreshTableCache(_tableOptions);
            return await RefreshRecord(_tableOptions, recordId);
        }

        private async Task<T> RefreshRecord(AdaloTableOptions table, int recordId, T record = null)
        {
            using var scope = serviceProvider.CreateScope();
            var cache = scope.ServiceProvider.GetService<IMemoryCache>();
            return await cache.GetOrCreateAsync($"{table.TableId}-{recordId}", async entry =>
            {
                var result = record ?? await adaloService.GetAsync(recordId);
                if (table.IsCached && table.CacheDuration != null)
                {
                    entry.SetAbsoluteExpiration(table.CacheDuration.Value);
                }

                return result;
            });
        }

        private void RefreshTableCache(AdaloTableOptions table, bool force = false)
        {
            ArgumentNullException.ThrowIfNull(table);
            RefreshTableCacheAsync(table, true).Ignore();
        }

        private async Task<List<T>> RefreshTableCacheAsync(AdaloTableOptions table, bool force = false)
        {
            using var scope = serviceProvider.CreateScope();
            var cache = scope.ServiceProvider.GetService<IMemoryCache>();
            if (force)
            {
                cache.Remove(table.TableId);
            }
            
            return await cache.GetOrCreateAsync(table.TableId, async entry =>
            {
                var result = await adaloService.GetAllAsync();
                if (table.IsCached && table.CacheDuration != null)
                {
                    entry.SetAbsoluteExpiration(table.CacheDuration.Value);
                }

                return result;
            }) ?? [];
        }

        public async Task DeleteAsync(
            int recordId)
        { 
            await adaloService.DeleteAsync(recordId);
            RefreshRecord(_tableOptions, recordId).Ignore();
            UpdateTableCache(_tableOptions, list => list.Where(x => x.Id != recordId).ToList());
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
                RefreshTableCache(table, true);
            }
        }

        public async Task<T> PutAsync(
            int recordId,
            T payload)
        { 
            var result = await adaloService.PutAsync(recordId, payload);
            RefreshRecord(_tableOptions, recordId, result).Ignore();
            UpdateTableCache(_tableOptions, list => list.Where(x => x.Id != recordId).Append(result).ToList());
            return result;
        }
    }
}
