using System;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using AdaloExtensionPack.Core.Tables.Options;
using Microsoft.Extensions.Caching.Memory;

namespace AdaloExtensionPack.Core.Tables.Services;

public class AdaloTableCacheService(
    IMemoryCache memoryCache,
    IAdaloTableService<AdaloDynamicEntity> adaloService,
    AdaloTableOptions tableOptions,
    IServiceProvider serviceProvider)
    : AdaloTableCacheService<AdaloDynamicEntity>(memoryCache, adaloService, tableOptions, serviceProvider),
        IAdaloTableCacheService;