using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloTableCacheController<T> : ODataController where T : AdaloEntity
    {
        private readonly IAdaloTableCacheService<T> _adaloTableCacheService;

        public AdaloTableCacheController(IAdaloTableCacheService<T> adaloTableCacheService)
        {
            _adaloTableCacheService = adaloTableCacheService;
        }

        [HttpGet, EnableQuery]
        public async Task<ActionResult<IEnumerable<T>>> GetAllAsync()
        {
            var result = await _adaloTableCacheService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] T payload)
        {
            var result = await _adaloTableCacheService.PostAsync(payload);
            return Ok(result);
        }

        [HttpGet("{recordId}")]
        public async Task<ActionResult<T>> GetAsync([FromRoute] int recordId)
        {
            var result = await _adaloTableCacheService.GetAsync(recordId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{recordId}")]
        public async Task<IActionResult> DeleteAsync(
            int recordId)
        {
            await _adaloTableCacheService.DeleteAsync(recordId);
            return Ok();
        }

        [HttpPut("{recordId}")]
        public async Task<ActionResult<T>> PutAsync([FromRoute] int recordId, [FromBody] T payload)
        {
            var result = await _adaloTableCacheService.PutAsync(recordId, payload);
            return Ok(result);
        }
    }
}
