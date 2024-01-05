using Core.Entities.Abstract;
using Core.Extensions;
using Entities.Constants;

namespace Entities.DTOs.Results
{
    public class ReportForViewDTO : IDTO
    {
        public Guid Id { get; set; }
        public ReportStatus Status { get; set; }
        public string StatusName
        {
            get { return Status.GetDescription(); }
        }
        public DateTime CreatedOn { get; set; }
    }
}