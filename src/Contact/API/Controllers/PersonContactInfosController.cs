using Business.Abstract;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    public class PersonContactInfosController : BaseController
    {
        private readonly IPersonContactInfoService _personContactInfoService;

        public PersonContactInfosController(IPersonContactInfoService personContactInfoService)
        {
            _personContactInfoService = personContactInfoService;
        }

        [HttpPost("{personId}")]
        public async Task<IActionResult> Add(Guid personId, [FromBody] PersonContactInfoForUpsertDTO model)
        {
            Log.Information($"Adding personContactInfo {personId}");
            var result = await _personContactInfoService.AddAsync(personId, model);
            Log.Information($"PersonContactInfo added {personId} completed");

            return ActionResultInstance<string>(result);
        }

        [HttpPut("{personId}/{id}")]
        public async Task<IActionResult> Edit(Guid personId, Guid id, [FromBody] PersonContactInfoForUpsertDTO model)
        {
            Log.Information($"PersonContactInfo editing {personId} -> {id}");
            var result = await _personContactInfoService.UpdateAsync(personId, id, model);
            Log.Information($"PersonContactInfo edited {personId} -> {id} completed");

            return ActionResultInstance<string>(result);
        }

        [HttpDelete("{personId}/{id}")]
        public async Task<IActionResult> Delete(Guid personId, Guid id)
        {
            Log.Information($"PersonContactInfo deleting {personId} -> {id}");
            var result = await _personContactInfoService.DeleteAsync(personId, id);
            Log.Information($"PersonContactInfo deleted {personId} -> {id} completed");

            return ActionResultInstance<string>(result);
        }
    }
}
