using Business.Abstract;
using Core.Utilities.Filtering.DataTable;
using Entities.DTOs.Params;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    public class PersonsController : BaseController
    {
        private readonly IPersonService _personService;

        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] DataTableOptions options)
        {
            Log.Information("Get persons");
            var result = await _personService.ListAsync(options);
            Log.Information("Get persons completed");

            return ActionResultInstance(result);
        }

        [HttpGet("view/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Log.Information($"Get person {id}");
            var result = await _personService.GetAsync(id);
            Log.Information($"Get person {id} completed");

            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PersonForUpsertDTO model)
        {
            Log.Information($"Adding person {model.FirstName} {model.LastName}");
            var result = await _personService.AddAsync(model);
            Log.Information($"Person added {model.FirstName} {model.LastName} completed");

            return ActionResultInstance<string>(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] PersonForUpsertDTO model)
        {
            Log.Information($"Person editing {id}");
            var result = await _personService.UpdateAsync(id, model);
            Log.Information($"Person edited {id} completed");

            return ActionResultInstance<string>(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Log.Information($"Person deleting {id}");
            var result = await _personService.DeleteAsync(id);
            Log.Information($"Person deleted {id}");

            return ActionResultInstance<string>(result);
        }


        [HttpGet("report")]
        public async Task<IActionResult> Report()
        {
            Log.Information("Get person report");
            var result = await _personService.ReportAsync();
            Log.Information("Get person report completed");

            return ActionResultInstance(result);
        }
    }
}
