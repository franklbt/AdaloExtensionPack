using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloViewController<TContext, TBase, TResult> : Controller where TBase : AdaloEntity
    {
        private readonly IAdaloViewService<TContext, TBase, TResult> _adaloViewService;

        public AdaloViewController(IAdaloViewService<TContext, TBase, TResult> adaloViewService)
        {
            _adaloViewService = adaloViewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _adaloViewService.GetAllAsync();
            return Ok(result);
        }
    }
}