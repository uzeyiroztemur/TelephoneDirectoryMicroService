namespace Entities.DTOs.Results
{
    public class ReportForPreviewDTO : ReportForViewDTO
    {
        public ReportForPreviewDTO()
        {
            Details = new List<ReportDetailForViewDTO>();
        }

        public List<ReportDetailForViewDTO> Details { get; set; }
    }
}