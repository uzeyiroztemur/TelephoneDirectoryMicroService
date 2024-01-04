using Core.Entities.Abstract;

namespace Core.Entities.DTOs
{
    public class IdForEntityDTO<T> : IDTO
    {
        public T Id { get; set; }
    }

    public class IdForEntityDTO : IdForEntityDTO<int> { }
}
