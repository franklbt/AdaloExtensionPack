using System;
using System.Collections.Generic;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloOptions
    {
        internal List<AdaloAppOptions> Apps { get; set; } = new();

        public AdaloAppOptions AddApplication(Guid appId, string token)
        {
            var opts = new AdaloAppOptions
            {
                AppId = appId,
                Token = token
            };
            Apps.Add(opts);
            return opts;
        }
    }
}
