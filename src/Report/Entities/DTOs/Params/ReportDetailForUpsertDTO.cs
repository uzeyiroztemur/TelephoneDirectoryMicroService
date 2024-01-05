using Core.Entities.Abstract;

namespace Entities.DTOs.Params
{
    public class ReportDetailForUpsertDTO : IDTO
    {
        public string Location { get; set; }
        public long PersonCount { get; set; }
        public long PhoneNumberCount { get; set; }
    }
}