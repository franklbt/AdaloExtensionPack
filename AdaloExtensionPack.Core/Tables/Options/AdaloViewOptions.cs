using System;

namespace AdaloExtensionPack.Core.Tables.Options;

public class AdaloViewOptions
{
}

public class AdaloViewOptions<TContext, TBase, TResult> : AdaloViewOptions
{
    public Func<TContext, TBase, TResult> Selector { get; set; }
    public Func<TContext, TBase, bool> Predicate { get; set; }
    public Func<IServiceProvider, TContext> Context { get; set; }
}