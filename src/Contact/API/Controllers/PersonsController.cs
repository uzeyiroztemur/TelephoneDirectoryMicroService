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
        public IActionResult List([FromQuery] DataTableOptions options)
        {
            _logger.Info("Get persons");
            var result = _personService.List(options);
            _logger.HandleResult(result, "Get persons");

            return ActionResultInstance(result);
        }

        [HttpGet("view/{id}")]
        public IActionResult Get(Guid id)
        {
            _logger.Info($"Get person {id}");
            var result = _personService.Get(id);
            _logger.HandleResult(result, $"Get person {id}");

            return ActionResultInstance(result);
        }

        [HttpPost]
        public IActionResult Add([FromBody] PersonForUpsertDTO model)
        {
            _logger.Info($"Adding person {model.FirstName} {model.LastName}");
            var result = _personService.Add(model);
            _logger.HandleResult(result, $"Person added {model.FirstName} {model.LastName}");

            return ActionResultInstance<string>(result);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(Guid id, [FromBody] PersonForUpsertDTO model)
        {
            _logger.Info($"Person editing {id}");
            var result = _personService.Update(id, model);
            _logger.HandleResult(result, $"Person edited {id}");

            return ActionResultInstance<string>(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _logger.Info($"Person deleting {id}");
            var result = _personService.Delete(id);
            _logger.HandleResult($"Person deleted {id}");

            return ActionResultInstance<string>(result);
        }


        [HttpGet("report")]
        public IActionResult Report()
        {
            _logger.Info("Get person report");
            var result = _personService.Report();
            _logger.HandleResult(result, "Get person report");

            return ActionResultInstance(result);
        }
    }
}
