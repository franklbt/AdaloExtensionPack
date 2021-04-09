using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdaloExtensionPack.Core.Adalo
{
    public static class AdaloServiceCollectionExtension
    {
        public static IServiceCollection AddAdalo(this IServiceCollection services, Action<AdaloOptions> optionsFactory)
        {
            services.Configure(optionsFactory);
            services.AddHttpClient();
            services.AddScoped<IAdaloTableServiceFactory, AdaloTableServiceFactory>();
            services.AddMemoryCache();

            var options = services.BuildServiceProvider().GetService<IOptions<AdaloOptions>>()?.Value;
            if (options == null)
                return services;
            
            foreach (var tablesType in options.TablesTypes)
            {
                services.AddScoped(
                    typeof(IAdaloTableService<>).MakeGenericType(tablesType.Key), 
                    s => s.GetService<IAdaloTableServiceFactory>()?.Create(tablesType.Key, tablesType.Value.TableId));
                if (tablesType.Value.IsCached)
                {
                    services.AddScoped(typeof(AdaloTableCacheService<>).MakeGenericType(tablesType.Key));
                }
            }
            foreach (var viewTypes in options.ViewTypes)
            {
                services.AddScoped(
                    typeof(IAdaloViewService<,,>).MakeGenericType(viewTypes.GetType().GenericTypeArguments), 
                    typeof(AdaloViewService<,,>).MakeGenericType(viewTypes.GetType().GenericTypeArguments)); 
            }

            services
                .AddControllers(o => o.Conventions.Add(new AdaloControllerConvention(options)))
                .ConfigureApplicationPartManager(m =>
                    m.FeatureProviders.Add(new AdaloControllerFeatureProvider(options)));

            return services;
        }
    }
}