using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloTableCacheController<T> : Controller where T : AdaloEntity
    {
        private readonly AdaloTableCacheService<T> _adaloTableCacheService;

        public AdaloTableCacheController(AdaloTableCacheService<T> adaloTableCacheService)
        {
            _adaloTableCacheService = adaloTableCacheService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _adaloTableCacheService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]
            T payload)
        {
            var result = await _adaloTableCacheService.PostAsync(payload);
            return Ok(result);
        }

        [HttpGet("{recordId}")]
        public async Task<IActionResult> GetAsync([FromRoute]
            int recordId)
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
        public async Task<IActionResult> PutAsync([FromRoute]
            int recordId, [FromBody]
            T payload)
        {
            var result = await _adaloTableCacheService.PutAsync(recordId, payload);
            return Ok(result);
        }
    }
}