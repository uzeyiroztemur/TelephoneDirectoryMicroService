using Business.Abstract;
using Core.Utilities.Filtering.DataTable;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] DataTableOptions options)
        {
            Log.Information("Get reports");
            var result = await _reportService.ListAsync(options);
            Log.Information("Get reports completed");

            return ActionResultInstance(result);
        }

        [HttpGet("view/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Log.Information($"Get report {id}");
            var result = await _reportService.GetAsync(id);
            Log.Information($"Get report {id} completed");

            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            Log.Information($"Creating report");
            var result = await _reportService.CreateAsync();
            Log.Information($"Report created completed");

            return ActionResultInstance<string>(result);
        }
    }
}