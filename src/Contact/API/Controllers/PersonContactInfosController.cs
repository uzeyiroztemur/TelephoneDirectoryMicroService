using Business.Abstract;
using Core.CrossCuttingConcerns.Logging;
using Core.Entities.DTOs;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PersonContactInfosController : BaseController
    {
        private readonly IPersonContactInfoService _personContactInfoService;
        private readonly Core.CrossCuttingConcerns.Logging.ILogger _logger;

        public PersonContactInfosController(IPersonContactInfoService personContactInfoService, Core.CrossCuttingConcerns.Logging.ILogger logger)
        {
            _personContactInfoService = personContactInfoService;
            _logger = logger;
        }

        [HttpPost("{personId}")]
        public IActionResult Add(Guid personId, [FromBody] PersonContactInfoForUpsertDTO model)
        {
            _logger.Info($"Adding personContactInfo {personId}");
            var result = _personContactInfoService.Add(personId, model);
            _logger.HandleResult(result, $"PersonContactInfo added {personId}");

            return ActionResultInstance<string>(result);
        }

        [HttpPut("{personId}/{id}")]
        public IActionResult Edit(Guid personId, Guid id, [FromBody] PersonContactInfoForUpsertDTO model)
        {
            _logger.Info($"PersonContactInfo editing {personId} -> {id}");
            var result = _personContactInfoService.Update(personId, id, model);
            _logger.HandleResult(result, $"PersonContactInfo edited {personId} -> {id}");

            return ActionResultInstance<string>(result);
        }

        [HttpDelete("{personId}/{id}")]
        public IActionResult Delete(Guid personId, Guid id)
        {
            var model = new IdForEntityDTO<Guid> { Id = id };

            _logger.Info($"PersonContactInfo deleting {personId} -> {id}");
            var result = _personContactInfoService.Delete(personId, id);
            _logger.HandleResult($"PersonContactInfo deleted {personId} -> {id}");

            return ActionResultInstance<string>(result);
        }
    }
}
