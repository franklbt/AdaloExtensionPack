using AdaloExtensionPack.Core.ApiKey;
using Microsoft.Extensions.DependencyInjection;

namespace AdaloExtensionPack.Core.Adalo;

public class AdaloServiceCollection(IServiceCollection Services)
{
    internal IServiceCollection Services { get; init; } = Services;

    public AdaloServiceCollection WithGeneratedControllerApiKey(string apiKey)
    {
        Services.AddApiKey(opts => opts.ApiKey = apiKey);
        return this;
    }
}
