using Core.Entities.DTOs;
using Core.Utilities.Filtering.DataTable;
using Entities.DTOs.Params;
using Entities.DTOs.Results;

namespace Business.Abstract
{
    public interface IPersonService
    {
        Task<IDataResult<IList<PersonForViewDTO>>> ListAsync(DataTableOptions options);
        Task<IDataResult<PersonForPreviewDTO>> GetAsync(Guid id);
        Task<IDataResult<Guid?>> AddAsync(PersonForUpsertDTO model);
        Task<IResult> UpdateAsync(Guid id, PersonForUpsertDTO model);
        Task<IResult> DeleteAsync(Guid id);

        Task<IDataResult<IList<PersonReportForViewDTO>>> ReportAsync();
    }
}