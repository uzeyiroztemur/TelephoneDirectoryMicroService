using Core.Entities.DTOs;
using Core.Utilities.Filtering.DataTable;
using Entities.DTOs.Params;
using Entities.DTOs.Results;

namespace Business.Abstract
{
    public interface IReportService
    {
        Task<IDataResult<IList<ReportForViewDTO>>> ListAsync(DataTableOptions options);
        Task<IDataResult<ReportForPreviewDTO>> GetAsync(Guid id);
        Task<IDataResult<Guid?>> CreateAsync();
        Task<IResult> CreateDetailAsync(Guid reportId, IList<ReportDetailForUpsertDTO> data);
    }
}
