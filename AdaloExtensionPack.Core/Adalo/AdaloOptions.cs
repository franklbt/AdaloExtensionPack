using System;
using System.Collections.Generic;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloOptions
    {
        public Guid AppId { get; set; }
        public string Token { get; set; }

        internal Dictionary<Type, AdaloTableOptions> TablesTypes { get; set; } =
            new Dictionary<Type, AdaloTableOptions>();

        public List<AdaloViewOptions> ViewTypes { get; set; } =
            new List<AdaloViewOptions>();

        public void AddTable<T>(string tableId, bool cached = false)
            => TablesTypes.Add(typeof(T),
                new AdaloTableOptions
                {
                    IsCached = cached,
                    TableId = tableId
                });

        public void AddView<TContext, TBase, TResult>(
            Func<IServiceProvider, TContext> context,
            Func<TContext, TBase, bool> predicate,
            Func<TContext, TBase, TResult> selector)
            => ViewTypes.Add(
                new AdaloViewOptions<TContext, TBase, TResult>
                {
                    Context = context,
                    Selector = selector,
                    Predicate = predicate
                });
    }
}