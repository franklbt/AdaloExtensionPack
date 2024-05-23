﻿using System;
using System.Reflection;
using AdaloExtensionPack.Core.Tables.Interfaces;
using AdaloExtensionPack.Core.Tables.Options;
using AdaloExtensionPack.Core.Tables.Registration;
using CaseExtensions;
using Humanizer;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OData.ModelBuilder;

namespace AdaloExtensionPack.Core.Tables.Services
{
    public static class AdaloServiceCollectionExtension
    {
        private static readonly MethodInfo EntitySetMethodInfo = typeof(ODataConventionModelBuilder)
            .GetMethod(nameof(ODataConventionModelBuilder.EntitySet));

        public static AdaloServiceCollection AddAdalo(this IServiceCollection services,
            Action<AdaloOptions> optionsFactory)
        {
            services.Configure(optionsFactory);
            services.AddHttpClient();
            services.AddScoped<IAdaloTableServiceFactory, AdaloTableServiceFactory>();
            services.AddMemoryCache();

            var options = services.BuildServiceProvider().GetRequiredService<IOptions<AdaloOptions>>().Value;
            var oDataModelBuilder = new ODataConventionModelBuilder();

            foreach (var option in options.Apps)
            {
                foreach (var tablesType in option.TablesTypes)
                {
                    services.AddScoped(
                        typeof(IAdaloTableService<>).MakeGenericType(tablesType.Key),
                        s => s.GetService<IAdaloTableServiceFactory>()
                            ?.Create(tablesType.Key, option, tablesType.Value.TableId));

                    if (tablesType.Value.IsCached)
                    {
                        services.AddScoped(
                            typeof(IAdaloTableCacheService<>).MakeGenericType(tablesType.Key),
                            typeof(AdaloTableCacheService<>).MakeGenericType(tablesType.Key));

                        EntitySetMethodInfo
                            .MakeGenericMethod(tablesType.Key)
                            .Invoke(oDataModelBuilder, [tablesType.Key.Name.Pluralize().ToKebabCase()]);
                    }
                }

                foreach (var viewType in option.ViewTypes)
                {
                    services.AddScoped(
                        typeof(IAdaloViewService<,,>).MakeGenericType(viewType.GetType().GenericTypeArguments),
                        typeof(AdaloViewService<,,>).MakeGenericType(viewType.GetType().GenericTypeArguments));
                }
            }

            services
                .AddControllers(o => o.Conventions.Add(new AdaloControllerConvention()))
                .AddOData(opts => opts
                    .Select().Filter().OrderBy().Count()
                    .AddRouteComponents("tables", oDataModelBuilder.GetEdmModel()))
                .ConfigureApplicationPartManager(m =>
                    m.FeatureProviders.Add(new AdaloControllerFeatureProvider(options)));

            return new(services);
        }
    }
}