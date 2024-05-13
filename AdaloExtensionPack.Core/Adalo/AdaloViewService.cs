using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloViewService<TContext, TBase, TResult> : IAdaloViewService<TContext, TBase, TResult> where TBase : AdaloEntity
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AdaloOptions _options;

        public AdaloViewService(IOptions<AdaloOptions> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }

        public async Task<List<TResult>> GetAllAsync()
        {
            var view = _options.Apps.SelectMany(x => x.ViewTypes).FirstOrDefault(x =>
            {
                var type = x.GetType();
                var compareType = typeof(AdaloViewService<TContext, TBase, TResult>);
                return type.IsGenericType && type.GenericTypeArguments.Length == 2 &&
                       type.GenericTypeArguments[1] == compareType.GenericTypeArguments[1] &&
                       type.GenericTypeArguments[2] == compareType.GenericTypeArguments[2];
            }) as AdaloViewOptions<TContext, TBase, TResult>;

            if (view == null)
                return new List<TResult>();

            var context = view.Context(_serviceProvider);
            
            var service = _serviceProvider.GetService(typeof(AdaloTableCacheService<TBase>)) 
                          ?? _serviceProvider.GetService(typeof(IAdaloTableService<TBase>));

            switch (service)
            {
                case AdaloTableCacheService<TBase> typedCacheService:
                    return (await typedCacheService.GetAllAsync())
                        .Where(x => view.Predicate(context, x))
                        .Select(x => view.Selector(context, x))
                        .ToList();
                case IAdaloTableService<TBase> typedTableService:
                    return (await typedTableService.GetAllAsync())
                        .Where(x => view.Predicate(context, x))
                        .Select(x => view.Selector(context, x))
                        .ToList();
                default:
                    return new List<TResult>();
            }
        }
    }
}
