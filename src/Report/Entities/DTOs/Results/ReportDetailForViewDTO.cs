using Core.Entities.Abstract;

namespace Entities.DTOs.Results
{
    public class ReportDetailForViewDTO : IDTO
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public long PersonCount { get; set; }
        public long PhoneNumberCount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}