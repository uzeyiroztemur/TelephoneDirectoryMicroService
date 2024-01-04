using Core.Entities.Abstract;

namespace Core.Entities.DTOs
{
    public class ExportFileViewModel : IDTO
    {
        public string SendReportFilePath { get; set; }
        public string TemlateFilePath { get; set; }
    }
}
