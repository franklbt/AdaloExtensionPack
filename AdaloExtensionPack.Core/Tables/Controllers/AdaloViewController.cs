using System.Threading.Tasks;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdaloExtensionPack.Core.Tables.Controllers
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