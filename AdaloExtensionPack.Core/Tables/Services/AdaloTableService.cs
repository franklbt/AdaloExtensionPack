using System.Net.Http;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using AdaloExtensionPack.Core.Tables.Options;

namespace AdaloExtensionPack.Core.Tables.Services;

public class AdaloTableService(IHttpClientFactory httpClientFactory, AdaloTableOptions options)
    : AdaloTableService<AdaloDynamicEntity>(httpClientFactory, options), IAdaloTableService;