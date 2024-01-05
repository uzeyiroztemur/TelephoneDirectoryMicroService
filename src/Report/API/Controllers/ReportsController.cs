using Business.Abstract;
using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.Filtering.DataTable;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly Core.CrossCuttingConcerns.Logging.ILogger _logger;

        public ReportsController(IReportService reportService, Core.CrossCuttingConcerns.Logging.ILogger logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult List([FromQuery] DataTableOptions options)
        {
            _logger.Info("Get reports");
            var result = _reportService.List(options);
            _logger.HandleResult(result, "Get reports");

            return ActionResultInstance(result);
        }

        [HttpGet("view/{id}")]
        public IActionResult Get(Guid id)
        {
            _logger.Info($"Get report {id}");
            var result = _reportService.Get(id);
            _logger.HandleResult(result, $"Get report {id}");

            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            _logger.Info($"Creating report");
            var result = await _reportService.Create();
            _logger.HandleResult(result, $"Report created");

            return ActionResultInstance<string>(result);
        }
    }
}