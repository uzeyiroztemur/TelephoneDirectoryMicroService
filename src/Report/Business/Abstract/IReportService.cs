using Core.Entities.DTOs;
using Core.Utilities.Filtering.DataTable;
using Entities.DTOs.Params;
using Entities.DTOs.Results;

namespace Business.Abstract
{
    public interface IReportService
    {
        IDataResult<IList<ReportForViewDTO>> List(DataTableOptions options);
        IDataResult<ReportForPreviewDTO> Get(Guid id);
        Task<IResult> Create();
        IResult CreateDetail(Guid reportId, IList<ReportDetailForUpsertDTO> data);
    }
}
