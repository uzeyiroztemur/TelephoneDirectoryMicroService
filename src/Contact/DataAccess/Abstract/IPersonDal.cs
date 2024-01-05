using Core.DataAccess.Abstract;
using Core.Utilities.Filtering;
using Entities.Concrete;
using Entities.DTOs.Results;
using Core.Entities.DTOs;

namespace DataAccess.Abstract
{
    public interface IPersonDal : IEntityRepository<Person>
    {
        IDataResult<IList<PersonForViewDTO>> List(Filter filter);
        PersonForPreviewDTO Get(Guid id);
        IList<PersonReportForViewDTO> Report();
    }
}