using System;

namespace Core.Entities.Abstract
{
    public interface IDeletable
    {
        bool IsDeleted { get; set; }
        Guid? DeletedBy { get; set; }
        DateTime? DeletedOn { get; set; }        
    }
}
