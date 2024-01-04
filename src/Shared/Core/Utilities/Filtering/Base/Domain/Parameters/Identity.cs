
namespace Core.Utilities.Filtering.Parameters
{
    public class Identity : Identity<int>
    {

    }

    public class Identity<T>
    {
        public T Id { get; set; }
    }
}
