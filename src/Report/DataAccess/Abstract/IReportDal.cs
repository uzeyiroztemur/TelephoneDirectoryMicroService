using Core.DataAccess.Abstract;
using Core.Utilities.Filtering;
using Entities.Concrete;
using Entities.DTOs.Results;
using Core.Entities.DTOs;

namespace DataAccess.Abstract
{
    public interface IReportDal : IEntityRepository<Report>
    {
        IDataResult<IList<ReportForViewDTO>> List(Filter filter);
        ReportForPreviewDTO Get(Guid id);
    }
}