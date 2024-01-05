using Core.Entities.DTOs;
using Core.Utilities.Filtering.DataTable;
using Entities.DTOs.Params;
using Entities.DTOs.Results;

namespace Business.Abstract
{
    public interface IPersonService
    {
        IDataResult<IList<PersonForViewDTO>> List(DataTableOptions options);
        IDataResult<PersonForPreviewDTO> Get(Guid id);
        IResult Add(PersonForUpsertDTO model);
        IResult Update(Guid id, PersonForUpsertDTO model);
        IResult Delete(Guid id);

        IDataResult<IList<PersonReportForViewDTO>> Report();
    }
}
