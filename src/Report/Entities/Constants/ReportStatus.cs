using System.ComponentModel;

namespace Entities.Constants
{
    public enum ReportStatus
    {
        [Description("Hazırlanıyor")]
        Preparing = 1,

        [Description("Tamamlandı")]
        Completed = 2,
    }
}