using System;
using AdaloExtensionPack.Core.ApiKey;
using Microsoft.Extensions.DependencyInjection;

namespace AdaloExtensionPack.Core.Adalo;

public class AdaloServiceCollection(IServiceCollection Services)
{
    internal IServiceCollection Services { get; init; } = Services;

    public AdaloServiceCollection WithTableCacheControllerApiKey(string apiKey)
    {
        return WithTableCacheControllerApiKey(opts => opts.ApiKey = apiKey);
    }

    public AdaloServiceCollection WithTableCacheControllerApiKey(Action<ApiKeyAuthenticationOptions> action)
    {
        Services.AddApiKey(action);
        return this;
    }
}
