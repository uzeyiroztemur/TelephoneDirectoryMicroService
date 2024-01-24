using Business.Abstract;
using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.Filtering.DataTable;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PersonsController : BaseController
    {
        private readonly IPersonService _personService;
        private readonly Core.CrossCuttingConcerns.Logging.ILogger _logger;

        public PersonsController(IPersonService personService, Core.CrossCuttingConcerns.Logging.ILogger logger)
        {
            _personService = personService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] DataTableOptions options)
        {
            _logger.Info("Get persons");
            var result = await _personService.ListAsync(options);
            _logger.HandleResult(result, "Get persons");

            return ActionResultInstance(result);
        }

        [HttpGet("view/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.Info($"Get person {id}");
            var result = await _personService.GetAsync(id);
            _logger.HandleResult(result, $"Get person {id}");

            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PersonForUpsertDTO model)
        {
            _logger.Info($"Adding person {model.FirstName} {model.LastName}");
            var result = await _personService.AddAsync(model);
            _logger.HandleResult(result, $"Person added {model.FirstName} {model.LastName}");

            return ActionResultInstance<string>(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] PersonForUpsertDTO model)
        {
            _logger.Info($"Person editing {id}");
            var result = await _personService.UpdateAsync(id, model);
            _logger.HandleResult(result, $"Person edited {id}");

            return ActionResultInstance<string>(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.Info($"Person deleting {id}");
            var result = await _personService.DeleteAsync(id);
            _logger.HandleResult($"Person deleted {id}");

            return ActionResultInstance<string>(result);
        }


        [HttpGet("report")]
        public async Task<IActionResult> Report()
        {
            _logger.Info("Get person report");
            var result = await _personService.ReportAsync();
            _logger.HandleResult(result, "Get person report");

            return ActionResultInstance(result);
        }
    }
}
