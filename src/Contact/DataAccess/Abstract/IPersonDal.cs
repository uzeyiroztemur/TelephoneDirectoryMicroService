using Core.DataAccess.Abstract;
using Core.Utilities.Filtering;
using Entities.Concrete;
using Entities.DTOs.Results;
using Core.Entities.DTOs;

namespace DataAccess.Abstract
{
    public interface IPersonDal : IEntityRepository<Person>
    {
        Task<IDataResult<IList<PersonForViewDTO>>> ListAsync(Filter filter);
        Task<PersonForPreviewDTO> GetAsync(Guid id);
        Task<IList<PersonReportForViewDTO>> ReportAsync();
    }
}