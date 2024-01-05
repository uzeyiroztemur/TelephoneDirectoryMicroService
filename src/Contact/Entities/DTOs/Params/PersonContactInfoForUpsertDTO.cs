using Core.Entities.Abstract;
using Entities.Constants;

namespace Entities.DTOs.Params
{
    public class PersonContactInfoForUpsertDTO : IDTO
    {
        public ContactInfoType InfoType { get; set; }
        public string InfoValue { get; set; }
        public bool IsActive { get; set; }
    }
}