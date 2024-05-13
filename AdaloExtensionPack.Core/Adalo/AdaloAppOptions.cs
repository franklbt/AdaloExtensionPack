using System;
using System.Collections.Generic;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloAppOptions
    {
        internal Guid AppId { get; set; }
        internal string Token { get; set; }
        internal Dictionary<Type, AdaloTableOptions> TablesTypes { get; set; } = new();

        internal List<AdaloViewOptions> ViewTypes { get; set; } = new();

        public AdaloAppOptions AddTable<T>(string tableId, bool cached = false) where T: AdaloEntity
        {
            TablesTypes.Add(typeof(T),
                new AdaloTableOptions
                {
                    IsCached = cached,
                    TableId = tableId
                });
            return this;
        }

        public AdaloAppOptions AddView<TContext, TBase, TResult>(
            Func<IServiceProvider, TContext> context,
            Func<TContext, TBase, bool> predicate,
            Func<TContext, TBase, TResult> selector)
        {
            ViewTypes.Add(
                new AdaloViewOptions<TContext, TBase, TResult>
                {
                    Context = context,
                    Selector = selector,
                    Predicate = predicate
                });
            return this;
        }
    }
}
