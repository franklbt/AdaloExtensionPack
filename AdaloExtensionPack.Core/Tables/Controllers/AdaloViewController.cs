using System.Threading.Tasks;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdaloExtensionPack.Core.Tables.Controllers;

public class AdaloViewController<TContext, TBase, TResult>(
    IAdaloViewService<TContext, TBase, TResult> adaloViewService)
    : Controller
    where TBase : AdaloEntity
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await adaloViewService.GetAllAsync();
        return Ok(result);
    }
}