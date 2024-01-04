using System;

namespace Core.Entities.Abstract
{
    public interface ICreatable
    {
        Guid CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
    }
}
