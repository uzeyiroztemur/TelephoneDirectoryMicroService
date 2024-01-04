
namespace Core.Utilities.Filtering.Parameters
{
    public class Criter : Identity
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public Filter Filter { get; set; }

        public List<Core.Utilities.Filtering.Parameters.Sort> Sort { get; set; }
    }
}
