using Core.Entities.Abstract;
using Entities.Constants;

namespace Entities.Concrete
{
    public class Report : IEntity, ICreatable
    {
        public Guid Id { get; set; }
        public ReportStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }

        public virtual ICollection<ReportDetail> Details { get; set; }
    }
}