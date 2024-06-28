using Microsoft.Extensions.DependencyInjection;

namespace AdaloExtensionPack.Core.Fonts;

public static class FontsServiceCollectionExtension
{
    public static IServiceCollection AddFonts(this IServiceCollection services)
    {
        services.AddScoped<IFontsService, FontsService>();
        return services;
    }
}