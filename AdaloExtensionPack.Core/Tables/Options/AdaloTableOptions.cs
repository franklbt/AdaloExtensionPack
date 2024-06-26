﻿using System;

namespace AdaloExtensionPack.Core.Tables.Options;

public class AdaloTableOptions
{
    public bool IsCached { get; set; }
    public TimeSpan? CacheDuration { get; set; }
    public string TableId { get; set; }
    public bool GenerateCacheControllers { get; set; }
    public AdaloAppOptions AppOptions { get; set; }
}