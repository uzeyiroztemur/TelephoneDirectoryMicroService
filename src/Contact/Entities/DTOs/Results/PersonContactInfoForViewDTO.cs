using Core.Entities.Abstract;
using Core.Extensions;
using Entities.Constants;

namespace Entities.DTOs.Results
{
    public class PersonContactInfoForViewDTO : IDTO
    {
        public Guid Id { get; set; }
        public ContactInfoType InfoType { get; set; }
        public string InfoTypeName
        {
            get { return InfoType.GetDescription(); }
        }
        public string InfoValue { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}