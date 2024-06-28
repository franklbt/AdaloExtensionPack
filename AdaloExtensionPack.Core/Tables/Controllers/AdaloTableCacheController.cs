using System.Collections.Generic;
using System.Threading.Tasks;
using AdaloExtensionPack.Core.Tables.Data;
using AdaloExtensionPack.Core.Tables.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AdaloExtensionPack.Core.Tables.Controllers;

public class AdaloTableCacheController<T>(IAdaloTableCacheService<T> adaloTableCacheService) : ODataController
    where T : AdaloEntity
{
    [HttpGet, EnableQuery]
    public async Task<ActionResult<IEnumerable<T>>> GetAllAsync()
    {
        var result = await adaloTableCacheService.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] T payload)
    {
        var result = await adaloTableCacheService.PostAsync(payload);
        return Ok(result);
    }

    [HttpGet("{recordId}")]
    public async Task<ActionResult<T>> GetAsync([FromRoute] int recordId)
    {
        var result = await adaloTableCacheService.GetAsync(recordId);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{recordId}")]
    public async Task<IActionResult> DeleteAsync(
        int recordId)
    {
        await adaloTableCacheService.DeleteAsync(recordId);
        return Ok();
    }

    [HttpPut("{recordId}")]
    public async Task<ActionResult<T>> PutAsync([FromRoute] int recordId, [FromBody] T payload)
    {
        var result = await adaloTableCacheService.PutAsync(recordId, payload);
        return Ok(result);
    }
}