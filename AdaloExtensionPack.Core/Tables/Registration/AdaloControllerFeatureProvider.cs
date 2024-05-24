using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdaloExtensionPack.Core.Tables.Controllers;
using AdaloExtensionPack.Core.Tables.Options;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace AdaloExtensionPack.Core.Tables.Registration
{
    public class AdaloControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly AdaloOptions _options;

        public AdaloControllerFeatureProvider(AdaloOptions options)
        {
            _options = options;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var tables = _options.Apps
                .SelectMany(x => x.TablesTypes)
                .Where(x => x.Value.IsCached && x.Value.GenerateCacheControllers)
                .Select(x => x.Key)
                .ToList();

            foreach (var candidate in tables)
            {
                feature.Controllers.Add(
                    typeof(AdaloTableCacheController<>).MakeGenericType(candidate).GetTypeInfo()
                );
            }

            var views = _options.Apps
                .SelectMany(x => x.ViewTypes)
                .ToList();

            foreach (var candidate in views)
            {
                feature.Controllers.Add(
                    typeof(AdaloViewController<,,>).MakeGenericType(candidate.GetType().GenericTypeArguments)
                        .GetTypeInfo()
                );
            }
        }
    }
}
