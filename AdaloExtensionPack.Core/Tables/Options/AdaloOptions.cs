using System;
using System.Collections.Generic;
using System.Linq;

namespace AdaloExtensionPack.Core.Tables.Options;

public class AdaloOptions
{
    internal List<AdaloAppOptions> Apps { get; set; } = new();

    public AdaloAppOptions AddApplication(Guid appId, string token)
    {
        var opts = Apps
            .FirstOrDefault(x => x.AppId == appId, new AdaloAppOptions
            {
                AppId = appId,
                Token = token
            });
        Apps.Remove(opts);
        Apps.Add(opts);
        return opts;
    }
}