using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdaloExtensionPack.Core.Tables.Controllers;
using AdaloExtensionPack.Core.Tables.Options;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace AdaloExtensionPack.Core.Tables.Registration
{
    public class AdaloControllerFeatureProvider(AdaloOptions options) : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var tables = options.Apps
                .SelectMany(x => x.Tables)
                .Where(x => x.Value.Options.IsCached && x.Value.Options.GenerateCacheControllers)
                .Select(x => x.Value.Type)
                .ToList();

            foreach (var candidate in tables)
            {
                feature.Controllers.Add(
                    typeof(AdaloTableCacheController<>).MakeGenericType(candidate).GetTypeInfo()
                );
            }

            var views = options.Apps
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
