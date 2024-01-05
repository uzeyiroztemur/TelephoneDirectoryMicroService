using Core.Entities.Abstract;

namespace Entities.DTOs.Params
{
    public class LockdownForAccountDTO : IDTO
    {
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
    }
}
