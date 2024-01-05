using Core.DataAccess.Concrete.EntityFramework;
using Core.Entities.DTOs;
using Core.Utilities.Filtering;
using DataAccess.Abstract;
using DataAccess.DataContext.EntityFramework.Context;
using Core.Extensions;
using Entities.Concrete;
using Entities.DTOs.Results;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class ReportDal : EfEntityRepositoryBase<Report, AppDbContext>, IReportDal
    {
        public IDataResult<IList<ReportForViewDTO>> List(Filter filter)
        {
            using var context = new AppDbContext();

            var query = from report in context.Reports
                        select new ReportForViewDTO
                        {
                            Id = report.Id,
                            CreatedOn = report.CreatedOn,
                            Status = report.Status,
                        };

            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(w => w.StatusName.ToLower().Contains(filter.Search.ToLower()));

            return query.AsNoTracking().ToFilteredList(filter.GetCriter());
        }

        public ReportForPreviewDTO Get(Guid id)
        {
            using var context = new AppDbContext();

            var query = from report in context.Reports
                        where report.Id == id
                        select new ReportForPreviewDTO
                        {
                            Id = report.Id,
                            CreatedOn = report.CreatedOn,
                            Status = report.Status,
                            Details = report.Details.Select(s => new ReportDetailForViewDTO
                            {
                                Id = s.Id,
                                CreatedOn = s.CreatedOn,
                                Location = s.Location,
                                PersonCount = s.PersonCount,
                                PhoneNumberCount = s.PhoneNumberCount,
                            }).ToList(),
                        };

            return query.AsNoTracking().FirstOrDefault();
        }
    }
}
