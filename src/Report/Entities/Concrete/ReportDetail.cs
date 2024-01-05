using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class ReportDetail : IEntity, ICreatable
    {
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public string Location { get; private set; }
        public long PersonCount { get; private set; }
        public long PhoneNumberCount { get; private set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Report Report { get; set; }
    }
}