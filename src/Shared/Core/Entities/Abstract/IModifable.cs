using System;

namespace Core.Entities.Abstract
{
    public interface IModifable
    {
        Guid? ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }        
    }
}
