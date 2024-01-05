using Core.Entities.Abstract;

namespace Entities.DTOs.Results
{
    public class PersonReportForViewDTO : IDTO
    {
        public string Location { get; set; }
        public long PersonCount { get; set; }
        public long PhoneNumberCount { get; set; }
    }
}
