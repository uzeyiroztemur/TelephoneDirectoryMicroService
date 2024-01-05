using System.ComponentModel;

namespace Entities.Constants
{
    public enum ContactInfoType
    {
        [Description("Telefon Numarası")]
        Phone = 1,

        [Description("E-mail Adresi")]
        Email = 2,

        [Description("Konum")]
        Location = 3
    }
}