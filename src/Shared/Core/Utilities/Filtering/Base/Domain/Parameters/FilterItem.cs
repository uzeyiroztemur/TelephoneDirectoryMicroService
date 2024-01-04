namespace Core.Utilities.Filtering.Parameters
{
    public class FilterItem
    {
        public List<FilterItem> Filters { get; set; }

        public string Field { get; set; }

        public Operation Operator { get; set; }

        public string Value { get; set; }
    }
}
